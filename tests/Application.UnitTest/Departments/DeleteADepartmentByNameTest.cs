using Domain;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest;

public class DeleteADepartmentByNameTest
{
    private readonly IUnitOfWork unitOfWork;
    private readonly DeleteADepartmentByNameHandler _handler;

    public DeleteADepartmentByNameTest()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteADepartmentByNameHandler(unitOfWork);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_If_Department_NotFound()
    {
        //arrange
        var name = "Fruit";
        var request = new DeleteADepartmentByName { Name = name };
        unitOfWork.Departments.DeleteADepartmentByNameAsync(name)
        .Returns(Task.FromResult(false));
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(DepartmentsErrors.NotFound(name));
    }
    [Fact]
    public async Task Handler_Should_Return_Success_If_Department_Was_Found()
    {
        //arrange
        var name = "Fruit";
        var request = new DeleteADepartmentByName { Name = name };
        unitOfWork.Departments.DeleteADepartmentByNameAsync(name)
        .Returns(Task.FromResult(true));
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.IsSuccess.Should().BeTrue();
    }
}
