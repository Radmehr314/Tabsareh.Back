using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Tabsareh.Framework.Application.Security;
using Tabsareh.Domain.Models.Permissions;
using Tabsareh.Infrastructure.Persistance.MongoDocuments.AdminMongo;
using Tabsareh.Infrastructure.Persistance.MongoDocuments.RoleMongo;
using MongoDB.Bson;

namespace Tabsareh.Infrastructure.Persistance.Seed
{
    /// <summary>
    /// در اولین اجرا نقش‌های پیش‌فرض و یک ادمین ارشد را ایجاد می‌کند.
    /// </summary>
    public class DataSeeder : IHostedService
    {
        private readonly IMongoDatabase _database;
        private readonly IConfiguration _config;

        public DataSeeder(IMongoDatabase database, IConfiguration config)
        {
            _database = database;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var roles = _database.GetCollection<RoleDocument>("Roles");
            var admins = _database.GetCollection<AdminDocument>("Admins");

            // ---- نقش پیش‌فرض: مدیر کل با تمام دسترسی‌ها ----
            var superRole = await roles.Find(r => r.Name == "مدیر کل").FirstOrDefaultAsync(cancellationToken);
            if (superRole is null)
            {
                superRole = new RoleDocument
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "مدیر کل",
                    Permissions = Permission.All.ToList(),
                    CreatedAt = DateTime.Now
                };
                await roles.InsertOneAsync(superRole, cancellationToken: cancellationToken);
            }
            else
            {
                // همگام‌سازی دسترسی‌های مدیر کل با لیست کامل دسترسی‌ها
                var update = Builders<RoleDocument>.Update.Set(r => r.Permissions, Permission.All.ToList());
                await roles.UpdateOneAsync(r => r.Id == superRole.Id, update, cancellationToken: cancellationToken);
            }

            // ---- ادمین پیش‌فرض ----
            var hasAdmin = await admins.Find(_ => true).AnyAsync(cancellationToken);
            if (!hasAdmin)
            {
                var pepper = _config["Security:Pepper"]
                    ?? throw new InvalidOperationException("Missing Security:Pepper");
                var (hash, salt) = HashMaker.HashPassword("Admin@123", pepper);

                var admin = new AdminDocument
                {
                    Id = ObjectId.GenerateNewId(),
                    UserName = "admin",
                    FirstName = "مدیر",
                    LastName = "سیستم",
                    Phone = "09000000000",
                    Password = hash,
                    Salt = salt,
                    IsBan = false,
                    IsDeleted = false,
                    RoleId = superRole.Id.ToString(),
                    CreatedAt = DateTime.Now
                };
                await admins.InsertOneAsync(admin, cancellationToken: cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
