using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.Common.Services;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Infrastructure.Common.Services;
using ChainDegree.Infrastructure.Persistence;
using ChainDegree.Infrastructure.QuanLyBangCap.Repositories;
using ChainDegree.Infrastructure.QuanLyToChuc.BackgroundServices;
using ChainDegree.Infrastructure.QuanLyToChuc.Repositories;
using ChainDegree.Infrastructure.TuyenDung.Repositories;
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

        // Đăng ký background service
        services.AddHostedService<CoSoDaoTaoApprovedEventProcessor>();

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IBangCapRepository, BangCapRepository>();
        services.AddScoped<ISinhVienRepository, SinhVienRepository>();
        services.AddScoped<ICoSoDaoTaoRepository, CoSoDaoTaoRepository>();
        services.AddScoped<IYeuCauDangKyRepository, YeuCauDangKyRepository>();
        services.AddScoped<INhaTuyenDungRepository, NhaTuyenDungRepository>();

        services.AddScoped<ICoSoDaoTaoApprovedEventRepository,
            ICoSoDaoTaoApprovedEventRepository>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IBesuService, BesuService>();

        return services;
    }
}
