using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class ChangeSerialNumber : IRequest<Result<ChangeSerialNumberInfo?>>
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Serial number is required")]
    public string SerialNumber { get; set; } = default!;
}

public class ChangeSerialNumberHandler : IRequestHandler<ChangeSerialNumber, Result<ChangeSerialNumberInfo?>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeSerialNumberHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ChangeSerialNumberInfo?>> Handle(ChangeSerialNumber request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _unitOfWork.Assets.ChangeSerialNumberAsync(request.Id, request.SerialNumber);
            if (result is null) return Result.Failure<ChangeSerialNumberInfo?>(AssetErrors.NotFound(request.Id));
            return Result.Success<ChangeSerialNumberInfo?>(result);
        }
        catch (SerialNumberIsUniqueExceptions ex)
        {
            Console.WriteLine(ex.Message);
            return Result.Failure<ChangeSerialNumberInfo?>(AssetErrors.SerialNumberNotUnique);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}

public record ChangeSerialNumberInfo(string serialNumber);
