using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.External.ChainDegreeBlockchainService;
using ChainDegree.Application.External.ChainDegreeBlockchainService.Services;
using ChainDegree.Application.External.ChainDegreeFileService;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Application.QuanLyToChuc.Services;
using ChainDegree.Infrastructure.External.ChainDegreeBlockchainService.Services;
using ChainDegree.Infrastructure.External.ChainDegreeFileService;
using ChainDegree.Infrastructure.Persistence;
using ChainDegree.Infrastructure.QuanLyBangCap.Repositories;
using ChainDegree.Infrastructure.QuanLyToChuc.BackgroundServices;
using ChainDegree.Infrastructure.QuanLyToChuc.Repositories;
using ChainDegree.Infrastructure.TuyenDung.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

        // Đăng ký HttpClient với thời gian chờ cụ thể
        services.AddHttpClient<BesuService>()
            .ConfigureHttpClient((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<BesuOptions>>();
                client.Timeout = TimeSpan.FromMilliseconds(
                    options.Value.RpcTimeoutMs);
            });

        // Đăng ký services
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IBesuService, BesuService>();

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

        services.AddScoped<ICoSoDaoTaoApprovedEventRepository, CoSoDaoTaoApprovedEventRepository>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IBesuService, BesuService>();

        return services;
    }
}
