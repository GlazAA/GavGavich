using GavGavich.Application.Abstractions;
using GavGavich.Infrastructure.Persistence;
using GavGavich.Infrastructure.Persistence.Seed;
using GavGavich.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GavGavich.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString, sqlite => sqlite.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ILegalService, LegalService>();
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        await DatabaseSeeder.SeedAsync(services);
    }
}
