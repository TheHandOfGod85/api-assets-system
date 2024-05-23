using MediatR;
using SharedKernel;

namespace Application;

public class GetAllDepartments : IRequest<Result<IEnumerable<DepartmentResponse>>>
{

}


public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartments, Result<IEnumerable<DepartmentResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllDepartmentsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<DepartmentResponse>>> Handle(GetAllDepartments request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Departments.GetAllDepartmentsAsync();
        return Result.Success(result);
    }
}
