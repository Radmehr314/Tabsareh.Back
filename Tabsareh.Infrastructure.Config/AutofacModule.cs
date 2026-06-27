using Autofac;
using Microsoft.Extensions.Configuration;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Handlers.CommandHandlers;
using Tabsareh.Application.Handlers.QueryHandlers;
using Tabsareh.Domain;
using Tabsareh.Infrastructure.Persistance;
using Tabsareh.Infrastructure.Persistance.Repositories;
using Tabsareh.Infrastructure.Persistance.Services;

namespace Tabsareh.Infrastructure.Config
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _cfg;
        public AutofacModule(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AdminCommandHandler).Assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(ICommandHandler<>))))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(AuthQueryHandler).Assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(IQueryHandler<,>))))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(AdminRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
            builder.RegisterType<UserInfoService>().As<IUserInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<LicenseProvisioningService>().As<ILicenseProvisioningService>().InstancePerLifetimeScope();
            builder.RegisterType<SmsIrService>().As<ISmsService>().InstancePerLifetimeScope();

            builder.RegisterType<AutofacCommandBus>().As<ICommandBus>().InstancePerLifetimeScope();
            builder.RegisterType<AutofacQueryBus>().As<IQueryBus>().InstancePerLifetimeScope();
        }
    }
}
