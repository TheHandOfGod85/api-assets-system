using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class DeleteADepartmentByName : IRequest<Result>
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = default!;
}

public class DeleteADepartmentByNameHandler : IRequestHandler<DeleteADepartmentByName, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteADepartmentByNameHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteADepartmentByName request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Departments.DeleteADepartmentByNameAsync(request.Name);
        return result ? Result.Success() : Result.Failure(DepartmentsErrors.NotFound(request.Name));
    }
}
