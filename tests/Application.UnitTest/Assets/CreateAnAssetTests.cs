using Domain;
using FluentAssertions;
using NSubstitute;
using SharedKernel;
using Common;

namespace Application.UnitTests;

public class CreateAnAssetTests
{
    private static readonly CreateAnAsset Command = new CreateAnAsset
    {
        Name = "Turbo",
        SerialNumber = "THYUI099",
        Description = "some description",
        Department = "fruit"
    };
    private readonly CreateAnAssetHandler _handler;
    private readonly IUnitOfWork unitOfWorkMock;

    public CreateAnAssetTests()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new CreateAnAssetHandler(unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenSerialNumberIsUnique()
    {
        //arrange
        unitOfWorkMock.Assets.CheckIfSerialNumberIsUniqueAsync(Arg.Is<string>(x => x == Command.SerialNumber))
        .Returns(false);
        //act
        Result result = await _handler.Handle(Command, default);
        //assert
        result.Error.Should().Be(AssetErrors.SerialNumberNotUnique);
    }
    [Fact]
    public async Task Handle_Should_ReturnSuccessIfDepartmentIsNull()
    {
        //arrange
        unitOfWorkMock.Assets.CheckIfSerialNumberIsUniqueAsync(Arg.Is<string>(x => x == Command.SerialNumber))
        .Returns(true);
        CreateAnAsset command = new CreateAnAsset
        {
            Name = "Turbo",
            SerialNumber = "THYUI099",
            Description = "some description",
            Department = null
        };
        //act
        Result result = await _handler.Handle(command, default);
        //assert
        result.IsSuccess.Should().BeTrue();
    }
    [Fact]
    public async Task Handle_Should_ReturnErrorIfDepartmentIsNotNull_DepartmentNotExists()
    {
        //arrange
        unitOfWorkMock.Assets.CheckIfSerialNumberIsUniqueAsync(Arg.Is<string>(x => x == Command.SerialNumber))
        .Returns(true);
        unitOfWorkMock.Departments.CheckIfADepartmentExistsAsync(Arg.Is<string>(x => x == Command.Department))
        .Returns(false);
        CreateAnAsset command = new CreateAnAsset
        {
            Name = "Turbo",
            SerialNumber = "THYUI099",
            Description = "some description",
            Department = "local"
        };
        //act
        Result result = await _handler.Handle(command, default);
        //assert
        result.Error.Should().Be(DepartmentsErrors.NotFound(command.Department));
    }
    [Fact]
    public async Task Handle_Should_ReturnSuccessIfDepartmentExists()
    {
        //arrange
        unitOfWorkMock.Assets.CheckIfSerialNumberIsUniqueAsync(Arg.Is<string>(x => x == Command.SerialNumber))
        .Returns(true);
        unitOfWorkMock.Departments.CheckIfADepartmentExistsAsync(Arg.Is<string>(x => x == Command.Department))
        .Returns(true);
        CreateAnAsset command = new CreateAnAsset
        {
            Name = "Turbo",
            SerialNumber = "THYUI099",
            Description = "some description",
            Department = "fruit"
        };
        //act
        Result result = await _handler.Handle(command, default);
        //assert
        result.IsSuccess.Should().BeTrue();
    }
    [Fact]
    public async Task Handle_Should_Call_GetDepartmentByNameAsync_WhenDepartmentExists()
    {
        //arrange
        unitOfWorkMock.Assets.CheckIfSerialNumberIsUniqueAsync(Arg.Is<string>(x => x == Command.SerialNumber))
        .Returns(true);
        unitOfWorkMock.Departments.CheckIfADepartmentExistsAsync(Arg.Is<string>(x => x == Command.Department))
        .Returns(true);
        CreateAnAsset command = new CreateAnAsset
        {
            Name = "Turbo",
            SerialNumber = "THYUI099",
            Description = "some description",
            Department = "fruit"
        };
        //act
        await _handler.Handle(command, default);
        //assert
        await unitOfWorkMock.Departments.Received(1).GetDepartmentByNameAsync(Arg.Any<string>());
    }

    [Fact]
    public void Command_Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateAnAsset { Name = string.Empty, SerialNumber = "SN12345" };
        var result = ValidatorHelper.ValidateModel(model);
        result.Should().ContainSingle(x => x.ErrorMessage == "Name is required");
    }
}
