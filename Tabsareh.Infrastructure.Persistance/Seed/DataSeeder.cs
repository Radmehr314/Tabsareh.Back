using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tabsareh.Framework.Application.Security;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.Permissions;
using Tabsareh.Domain.Models.Roles;

namespace Tabsareh.Infrastructure.Persistance.Seed
{
    /// <summary>
    /// در اولین اجرا مایگریشن‌ها را اعمال و نقش‌های پیش‌فرض و یک ادمین ارشد را ایجاد می‌کند.
    /// </summary>
    public class DataSeeder : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;

        public DataSeeder(IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TabsarehDbContext>();

            // اعمال مایگریشن‌ها (ساخت دیتابیس/جداول در صورت نبود)
            await db.Database.MigrateAsync(cancellationToken);

            // ---- نقش پیش‌فرض: مدیر کل با تمام دسترسی‌ها ----
            var superRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "مدیر کل", cancellationToken);
            if (superRole is null)
            {
                superRole = new Role("مدیر کل", Permission.All.ToList());
                superRole.SetId(Guid.NewGuid().ToString("N"));
                db.Roles.Add(superRole);
                await db.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // همگام‌سازی دسترسی‌های مدیر کل با لیست کامل دسترسی‌ها
                superRole.Update("مدیر کل", Permission.All.ToList());
                await db.SaveChangesAsync(cancellationToken);
            }

            // ---- ادمین پیش‌فرض ----
            var hasAdmin = await db.Admins.AnyAsync(cancellationToken);
            if (!hasAdmin)
            {
                var pepper = _config["Security:Pepper"]
                    ?? throw new InvalidOperationException("Missing Security:Pepper");
                var (hash, salt) = HashMaker.HashPassword("Admin@123", pepper);

                var admin = new Admin("admin", "مدیر", "سیستم", "09000000000", hash, salt, false, superRole.Id);
                admin.SetId(Guid.NewGuid().ToString("N"));
                db.Admins.Add(admin);
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
