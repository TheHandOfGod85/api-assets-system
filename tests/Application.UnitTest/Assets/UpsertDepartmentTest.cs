using Common;
using Domain;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Application.UnitTest;

public class UpsertDepartmentTest
{
    private readonly IUnitOfWork unitOfWorkMock;
    private readonly UpsertDepartmentHandler _handler;

    public UpsertDepartmentTest()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new UpsertDepartmentHandler(unitOfWorkMock);
    }
    [Fact]
    public void Command_Should_Have_Error_When_SerialNumber_Is_Empty()
    {
        var model = new UpsertDepartment { DepartmentName = string.Empty };
        var result = ValidatorHelper.ValidateModel(model);
        result.Should().ContainSingle(x => x.ErrorMessage == "Department is required");
    }

    [Fact]
    public async Task handler_Should_Have_Error_Asset_Not_Found()
    {
        //arrange
        var request = new UpsertDepartment { Id = Guid.NewGuid(), DepartmentName = "prep" };
        unitOfWorkMock.Assets.UpsertDepartmentAsync(request.Id, request.DepartmentName)
        .Returns(Task.FromResult<UpsertDepartmentInfo?>(null));
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.Error.Should().Be(AssetErrors.NotFound(request.Id));
    }
    [Fact]
    public async Task handler_Should_Have_Error_Department_Not_Found()
    {
        //arrange
        var departmentNotFoundException = new DepartmentNotFoundException("Department not found");
        var request = new UpsertDepartment { Id = Guid.NewGuid(), DepartmentName = "prep" };
        unitOfWorkMock.Assets.UpsertDepartmentAsync(request.Id, request.DepartmentName)
        .ThrowsAsyncForAnyArgs(departmentNotFoundException);
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.Error.Should().Be(DepartmentsErrors.NotFound(request.DepartmentName));
    }
    [Fact]
    public async Task Handler_Should_Return_UpsertDepartmentInfo_Success()
    {
        //arrange
        var request = new UpsertDepartment { Id = Guid.NewGuid(), DepartmentName = "prep" };
        var response = new UpsertDepartmentInfo(request.DepartmentName);
        unitOfWorkMock.Assets.UpsertDepartmentAsync(request.Id, request.DepartmentName)
        .Returns(Task.FromResult<UpsertDepartmentInfo?>(response));
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(response);
    }
}
