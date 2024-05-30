using Common;
using Domain;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Application.UnitTest;

public class ChangeDepartmentNameTest
{
    private readonly IUnitOfWork unitOfWorkMock;
    private readonly ChangeDepartmentNameHandler _handler;
    public ChangeDepartmentNameTest()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new ChangeDepartmentNameHandler(unitOfWorkMock);
    }
    [Fact]
    public void Command_Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new ChangeDepartmentName { Name = string.Empty };
        var result = ValidatorHelper.ValidateModel(model);
        result.Should().ContainSingle(x => x.ErrorMessage == "Department name is required");
    }
    [Fact]
    public async Task Handler_Should_Have_Error_When_Department_Name_Is_Unique()
    {
        //arrange
        var isUniqueException = new DepartmentIsUniqueException("Department name must be unique");
        var request = new ChangeDepartmentName { Name = "Fruit", NewName = "Filling" };
        unitOfWorkMock.Departments.ChangeDepartmentNameAsync(request.Name, request.NewName)
        .ThrowsAsyncForAnyArgs(isUniqueException);
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.Error.Should().Be(DepartmentsErrors.DepartmentNotUnique);
    }
    [Fact]
    public async Task Handler_Should_Have_Error_When_Department_Not_Found()
    {
        //arrange
        var request = new ChangeDepartmentName { Name = "Fruit", NewName = "Filling" };
        unitOfWorkMock.Departments.ChangeDepartmentNameAsync(request.Name, request.NewName)
        .Returns(Task.FromResult<bool>(false));
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.Error.Should().Be(DepartmentsErrors.NotFound(request.Name));
    }
    [Fact]
    public async Task Handler_Should_return_Success()
    {
        //arrange
        var request = new ChangeDepartmentName { Name = "Fruit", NewName = "Filling" };
        unitOfWorkMock.Departments.ChangeDepartmentNameAsync(request.Name, request.NewName)
        .Returns(Task.FromResult<bool>(true));
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.IsSuccess.Should().BeTrue();
    }

}
