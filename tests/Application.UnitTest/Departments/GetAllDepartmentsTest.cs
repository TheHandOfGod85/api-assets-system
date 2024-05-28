using Domain;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest;

public class GetAllDepartmentsTest
{

    private readonly GetAllDepartmentsHandler _handler;
    private readonly IUnitOfWork unitOfWorkMock;

    public GetAllDepartmentsTest()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new GetAllDepartmentsHandler(unitOfWorkMock);
    }

    [Fact]
    public async Task GetAllDepartments_ReturnsAllDepartments()
    {
        //arrange
        var departments = new List<DepartmentResponse>
        {
            new DepartmentResponse{Name="Fruit"},
            new DepartmentResponse{Name="Prep"},
        };
        unitOfWorkMock.Departments.GetAllDepartmentsAsync().Returns(departments);
        var request = new GetAllDepartments();
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(departments);
    }
}
