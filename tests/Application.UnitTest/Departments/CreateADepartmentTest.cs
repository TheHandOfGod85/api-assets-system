using Common;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest;

public class CreateADepartmentTest
{
    private readonly CreateADepartmentHandler _handler;
    private readonly IUnitOfWork unitOfWorkMock;

    public CreateADepartmentTest()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new CreateADepartmentHandler(unitOfWorkMock);
    }

    [Fact]
    public void Command_Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateADepartment { Name = string.Empty };
        var result = ValidatorHelper.ValidateModel(model);
        result.Should().ContainSingle(x => x.ErrorMessage == "Department name is required");
    }

    [Fact]
    public async Task Handler_Should_Have_Error_When_A_Department_Name_Is_Unique()
    {
        var request = new CreateADepartment { Name = "Fruit" };
        //arrange
        unitOfWorkMock.Departments.CheckIfIsDepartmentIsUniqueAsync(request.Name).Returns(false);
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.Error.Should().Be(DepartmentsErrors.DepartmentNotUnique);
    }

    [Fact]
    public async Task Handler_Should_Have_Error_When_Cannot_Create_A_Department()
    {
        //arrange
        var request = new CreateADepartment { Name = "Fruit" };
        var department = new Department(request.Name);
        unitOfWorkMock.Departments.CheckIfIsDepartmentIsUniqueAsync(request.Name)
        .Returns(true);
        unitOfWorkMock.Departments.CreateADepartmentAsync(department)
        .Returns(Task.FromResult<DepartmentResponse?>(null));
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.Error.Should().Be(DepartmentsErrors.DepartmentCreationError);
    }
    // [Fact]
    // public async Task Handler_Should_Return_DepartmentResponse_When_Ok_Create_A_Department()
    // {
    //     //arrange
    //     var request = new CreateADepartment { Name = "Fruit" };
    //     var departmentRespone = new DepartmentResponse { Name = request.Name };
    //     unitOfWorkMock.Departments.CheckIfIsDepartmentIsUniqueAsync(request.Name)
    //     .Returns(true);
    //     unitOfWorkMock.Departments.CreateADepartmentAsync(departmentRespone.MapToDepartment())
    //     .Returns(Task.FromResult<DepartmentResponse?>(departmentRespone));
    //     //act
    //     var result = await _handler.Handle(request, default);
    //     //assert
    //     result.IsSuccess.Should().BeTrue();
    //     result.Value.Should().BeEquivalentTo(departmentRespone);
    // }
}
