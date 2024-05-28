using Common;
using Domain;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Application.UnitTest;

public class ChangeSerialNumberTest
{
    private readonly IUnitOfWork unitOfWorkMock;
    private readonly ChangeSerialNumberHandler _handler;

    public ChangeSerialNumberTest()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new ChangeSerialNumberHandler(unitOfWorkMock);
    }

    [Fact]
    public void Command_Should_Have_Error_When_SerialNumber_Is_Empty()
    {
        var model = new ChangeSerialNumber { SerialNumber = string.Empty };
        var result = ValidatorHelper.ValidateModel(model);
        result.Should().ContainSingle(x => x.ErrorMessage == "Serial number is required");
    }

    [Fact]
    public async Task Handler_Should_Have_Error_If_Not_Found()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new ChangeSerialNumber
        {
            SerialNumber = "THHSGGA90",
        };
        unitOfWorkMock.Assets.ChangeSerialNumberAsync
        (id, request.SerialNumber)
        .Returns(Task.FromResult<ChangeSerialNumberInfo?>(null));
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AssetErrors.NotFound(Arg.Any<Guid>()));
    }
    [Fact]
    public async Task Handler_Should_Have_Error_If_SerialNumber_Is_Unique()
    {
        //arrange
        var uniqueException = new SerialNumberIsUniqueExceptions("Serial number must be unique");
        var id = Guid.NewGuid();
        var request = new ChangeSerialNumber
        {
            SerialNumber = "THHSGGA90",
        };
        unitOfWorkMock.Assets.ChangeSerialNumberAsync
        (id, request.SerialNumber)
        .ThrowsForAnyArgs(uniqueException);
        //act
        var result = await _handler.Handle(request, default);
        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AssetErrors.SerialNumberNotUnique);
    }
}
