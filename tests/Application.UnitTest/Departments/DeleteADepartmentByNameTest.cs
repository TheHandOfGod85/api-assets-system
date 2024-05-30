using Domain;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

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
    public async Task Handler_Should_Return_Error_If_Cannot_Delete()
    {
        //arrange
        var cannotDeleteException = new CannotDeleteDepartmentException("Cannot delete");
        var name = "Fruit";
        var request = new DeleteADepartmentByName { Name = name };
        unitOfWork.Departments.DeleteADepartmentByNameAsync(name)
        .ThrowsAsyncForAnyArgs(cannotDeleteException);
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(DepartmentsErrors.CannotDelete(name));
    }
    [Fact]
    public async Task Handler_Should_Return_Success()
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
