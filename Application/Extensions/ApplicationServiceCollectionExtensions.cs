﻿using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly);

        });
        services.AddScoped<CurrentAppUser>();
        services.AddScoped<IdentityService>();
        return services;
    }
}
