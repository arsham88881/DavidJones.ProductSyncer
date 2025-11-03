using DavidJones.ProductSyncer.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.ProductSyncer;

public static class DependencyManager
{

    public static IServiceCollection AddDiLayers(this IServiceCollection services)
    {
        services.AddDiHelpersService();
        services.AddDiServices();
        services.AddDiWorkers();

        return services;
    }
    public static IServiceCollection AddDiHelpersService(this IServiceCollection services)
    {


        return services;
    }

    public static IServiceCollection AddDiServices(this IServiceCollection services)
    {


        return services;
    }

    public static IServiceCollection AddDiWorkers(this IServiceCollection services)
    {
        services.AddHostedService<Worker>();

        return services;
    }

}

