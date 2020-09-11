using EF.Core.Generic.Data.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Core.Generic.Data.DependencyInjection
{
    public static class UnitOfWorkServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services, ServiceType serviceType = ServiceType.Singleton)
            where TContext : DbContext
        {
            if (serviceType == ServiceType.Singleton)
            {
                services.AddSingleton<IRepositoryFactory, UnitOfWork<TContext>>();
                services.AddSingleton<IUnitOfWork, UnitOfWork<TContext>>();
                services.AddSingleton<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
            }
            else
            {
                services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
                services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
                services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
            }
            
            return services;
        }

        public static IServiceCollection AddUnitOfWork<TContext1, TContext2>(this IServiceCollection services, ServiceType serviceType = ServiceType.Singleton)
            where TContext1 : DbContext
            where TContext2 : DbContext
        {
            if (serviceType == ServiceType.Singleton)
            {
                services.AddSingleton<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
                services.AddSingleton<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
            }
            else
            {
                services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
                services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
            }
            
            return services;
        }

        public static IServiceCollection AddUnitOfWork<TContext1, TContext2, TContext3>(
            this IServiceCollection services, ServiceType serviceType = ServiceType.Singleton)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
        {
            if (serviceType == ServiceType.Singleton)
            {
                services.AddSingleton<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
                services.AddSingleton<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
                services.AddSingleton<IUnitOfWork<TContext3>, UnitOfWork<TContext3>>();
            }
            else
            {
                services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
                services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
                services.AddScoped<IUnitOfWork<TContext3>, UnitOfWork<TContext3>>();
            }

            return services;
        }

        public static IServiceCollection AddUnitOfWork<TContext1, TContext2, TContext3, TContext4>(
            this IServiceCollection services, ServiceType serviceType = ServiceType.Singleton)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext
        {
            if (serviceType == ServiceType.Singleton)
            {
                services.AddSingleton<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
                services.AddSingleton<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
                services.AddSingleton<IUnitOfWork<TContext3>, UnitOfWork<TContext3>>();
                services.AddSingleton<IUnitOfWork<TContext4>, UnitOfWork<TContext4>>();
            }
            else
            {
                services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
                services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
                services.AddScoped<IUnitOfWork<TContext3>, UnitOfWork<TContext3>>();
                services.AddScoped<IUnitOfWork<TContext4>, UnitOfWork<TContext4>>();
            }
            
            return services;
        }
    }
}