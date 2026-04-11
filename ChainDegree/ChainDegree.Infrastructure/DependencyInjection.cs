using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Infrastructure.Persistence;
using ChainDegree.Infrastructure.QuanLyBangCap.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChainDegree.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence
        services.AddDbContext<ChainDegreeDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly("ChainDegree.Infrastructure")));

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IBangCapRepository, BangCapRepository>();
        services.AddScoped<ISinhVienRepository, SinhVienRepository>();
        services.AddScoped<ICoSoDaoTaoRepository, CoSoDaoTaoRepository>();
        return services;
    }
}
