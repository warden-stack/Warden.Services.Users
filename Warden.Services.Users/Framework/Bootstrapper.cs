using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using NLog;
using RawRabbit;
using RawRabbit.vNext;
using RawRabbit.Configuration;
using Warden.Common.Commands;
using Warden.Common.Events;
using Warden.Common.Exceptionless;
using Warden.Common.Extensions;
using Warden.Common.Handlers;
using Warden.Common.Mongo;
using Warden.Common.Nancy;
using Warden.Common.RabbitMq;
using Warden.Services.Users.Repositories;
using Warden.Services.Users.Services;
using Warden.Services.Users.Settings;

namespace Warden.Services.Users.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IExceptionHandler _exceptionHandler;
        private readonly IConfiguration _configuration;
        public static ILifetimeScope LifetimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);
            container.Update(builder =>
            {
                builder.RegisterInstance(_configuration.GetSettings<MongoDbSettings>());
                builder.RegisterInstance(_configuration.GetSettings<RawRabbitConfiguration>());
                builder.RegisterModule<MongoDbModule>();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterType<DatabaseSeeder>().As<IDatabaseSeeder>();
                var rawRabbitConfiguration = _configuration.GetSettings<RawRabbitConfiguration>();
                builder.RegisterInstance(rawRabbitConfiguration).SingleInstance();
                builder.RegisterInstance(BusClientFactory.CreateDefault(rawRabbitConfiguration))
                    .As<IBusClient>();
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<ApiKeyRepository>().As<IApiKeyRepository>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterType<ApiKeyService>().As<IApiKeyService>();
                builder.RegisterType<Encrypter>().As<IEncrypter>().SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<FacebookSettings>());
                builder.RegisterInstance(AutoMapperConfig.InitializeMapper());
                builder.RegisterModule<MongoDbModule>();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterType<Encrypter>().As<IEncrypter>().SingleInstance();
                builder.RegisterType<OneTimeSecuredOperationRepository>().As<IOneTimeSecuredOperationRepository>();
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<UserSessionRepository>().As<IUserSessionRepository>();
                builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
                builder.RegisterType<FacebookClient>().As<IFacebookClient>();
                builder.RegisterType<FacebookService>().As<IFacebookService>();
                builder.RegisterType<OneTimeSecuredOperationService>().As<IOneTimeSecuredOperationService>();
                builder.RegisterType<PasswordService>().As<IPasswordService>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterType<Handler>().As<IHandler>().SingleInstance();
                RabbitMqContainer.Register(builder, _configuration.GetSettings<RawRabbitConfiguration>());
                builder.RegisterInstance(_configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
                builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();
                var assembly = typeof(Startup).GetTypeInfo().Assembly;
                builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
                builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            });
            LifetimeScope = container;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseSettings = container.Resolve<MongoDbSettings>();
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();
            if (databaseSettings.Seed)
            {
                var seeder = container.Resolve<IDatabaseSeeder>();
                seeder.SeedAsync();
            }
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            _exceptionHandler = container.Resolve<IExceptionHandler>();
            Logger.Info("Warden.Services.Users API has started.");
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                _exceptionHandler.Handle(ex, ctx.ToExceptionData(),
                    "Request details", "Warden", "Service", "Users");

                return ctx.Response;
            });
        }
    }
}