using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using InvoiceApp.Data.Data;
using InvoiceApp.Data.Repositories;
using InvoiceApp.Data.Services;

namespace InvoiceApp.Data;

public static class ServiceCollectionExtensions
{
    public static Task AddStorageAsync(this IServiceCollection services, string dbPath, string userInfoPath, string settingsPath)
    {
        if (string.IsNullOrWhiteSpace(dbPath))
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dataDir = Path.Combine(appData, "InvoiceApp");
            Directory.CreateDirectory(dataDir);
            dbPath = Path.Combine(dataDir, "app.db");
        }

        services.AddSingleton<WalPragmaInterceptor>();

        services.AddDbContext<AppDbContext>((sp, o) =>
            o.UseSqlite($"Data Source={dbPath}")
             .AddInterceptors(sp.GetRequiredService<WalPragmaInterceptor>()));

        services.AddDbContextFactory<AppDbContext>((sp, o) =>
            o.UseSqlite($"Data Source={dbPath}")
             .AddInterceptors(sp.GetRequiredService<WalPragmaInterceptor>()));
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
        services.AddScoped<ITaxRateRepository, TaxRateRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddSingleton<ILogService, SerilogLogService>();
        services.AddSingleton<IUserInfoService>(_ => new UserInfoService(userInfoPath));
        services.AddSingleton<ISettingsService>(_ => new SettingsService(settingsPath));
        var sessionPath = Path.Combine(Path.GetDirectoryName(settingsPath)!, "session.json");
        services.AddSingleton<ISessionService>(_ => new SessionService(sessionPath));
        services.AddSingleton<INumberingService, NumberingService>();
        services.AddScoped<IBackupService>(_ => new FileBackupService(dbPath, userInfoPath, settingsPath));
        services.AddScoped<IDbHealthService, DbHealthService>();
        services.AddSingleton<IDatabaseRecoveryService>(sp =>
            new DatabaseRecoveryService(dbPath, sp.GetRequiredService<IDbContextFactory<AppDbContext>>(), sp.GetRequiredService<ILogService>()));
        services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();

        return Task.CompletedTask;
    }
}
