using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class ChangeSerialNumber : IRequest<Result<ChangeSerialNumberInfo?>>
{
    public Guid Id { get; set; }
    [Required]
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result.Failure<ChangeSerialNumberInfo?>(AssetErrors.SerialNumberNotUnique);
        }
    }
}

public record ChangeSerialNumberInfo(string serialNumber);
