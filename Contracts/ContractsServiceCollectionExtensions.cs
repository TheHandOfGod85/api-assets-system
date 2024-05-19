
using Microsoft.Extensions.DependencyInjection;

namespace Contracts;

public static class ContractsServiceCollectionExtensions
{
    public static IServiceCollection AddContracts(this IServiceCollection services)
    {
        // services.AddValidatorsFromAssemblyContaining<IContractsMarker>(ServiceLifetime.Scoped);
        // services.AddFluentValidationAutoValidation(fv => fv.DisableDataAnnotationsValidation = true);
        return services;
    }
}
