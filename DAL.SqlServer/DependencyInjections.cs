using DAL.SqlServer.Context;
using DAL.SqlServer.Infrastructure;
using DAL.SqlServer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Common;
using Repository.Repositories;

namespace DAL.SqlServer;

public static class DependencyInjections
{
    public static IServiceCollection AddSqlServerServices(this IServiceCollection services, string connectionstring)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionstring));
        services.AddScoped<IPaymentRepository, SqlStripePaymentRepository>();

        services.AddScoped<IUnitOfWork, SqlUnitOfWork>(opt =>
        {
            var dbContext = opt.GetRequiredService<AppDbContext>();
            return new SqlUnitOfWork(connectionstring, dbContext);
        });

        return services;
    }
}
