using Autofac;
using FluentValidation;
using Intro.Application.Services;
using Intro.Application.Validation;
using Intro.DataAccess.Connection;
using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using Intro.Security;
using Intro.Serialization;
using Intro.Utility.DotNetWrappers;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Nancy.Responses;
using ServiceStack.Text;
using System.Reflection;

namespace Intro.Configuration
{
    //public class DevelopmentRootPathProvider : IRootPathProvider
    //{
    //    public string GetRootPath()
    //    {
    //        return @"C:\Projects\Intro\Intro.Web\";
    //    }
    //}

    public class IntroBootstrapper : AutofacNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/IntroBootstrapper
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            AddDirectoryConvention(nancyConventions, "Views");
            AddDirectoryConvention(nancyConventions, "Scripts");
            AddDirectoryConvention(nancyConventions, "bower_components");
            AddDirectoryConvention(nancyConventions, "polymers", "Views/polymers");
            AddDirectoryConvention(nancyConventions, "Tests");
            AddDirectoryConvention(nancyConventions, "fonts", "fonts");

            base.ConfigureConventions(nancyConventions);
        }

        protected override IRootPathProvider RootPathProvider
        {
            get
            {
                return base.RootPathProvider;
                //return new DevelopmentRootPathProvider();
            }
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
                ServiceStack.Text.JsConfig.DateHandler = DateHandler.ISO8601;

                var config = NancyInternalConfiguration.WithOverrides(cfg =>
                {
                    cfg.Serializers.Remove(typeof (DefaultJsonSerializer));
                    cfg.Serializers.Add(typeof (IntroJsonSerializer));
                });

                return config;
            }
        }

        private static void AddDirectoryConvention(NancyConventions nancyConventions, string requestedPath, string contentPath = null)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory(requestedPath, contentPath));
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during application startup.

            //Nancy.Security.Csrf.Enable(pipelines);

            pipelines.AfterRequest += ctx => ctx.Response.WithHeader("Access-Control-Allow-Origin", "*");
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            // Perform registration that should have an application lifetime

            var builder = new ContainerBuilder();

            RegisterInterfaces(builder);
            RegisterGenerics(builder);

            //RegisterConcretes(builder);
            RegisterSingletons(builder);

            RegisterServices(builder);
            RegisterEntities(builder);

            builder.Update(existingContainer.ComponentRegistry);
        }

        private static void RegisterEntities(ContainerBuilder builder)
        {
            var assembly = typeof(IEntity).Assembly;
            builder.RegisterAssemblyTypes(assembly).Where(t => t.IsSubclassOf(typeof(Entity))).As<Entity>();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            var assembly = typeof (IInventoryService).Assembly;
            RegisterByConvention(builder, assembly, "Service");
        }

        private static void RegisterByConvention(ContainerBuilder builder, Assembly assembly, string typeNameEndsWith)
        {
            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith(typeNameEndsWith))
                   .AsImplementedInterfaces();
        }

        private static void RegisterGenerics(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(EntityValidationResult<>)).As(typeof(IEntityValidationResult<>));
            builder.RegisterGeneric(typeof(EntityValidator<>)).As(typeof(IEntityValidator<>));
            RegisterByConvention(builder, typeof (IRepository<>).Assembly, "Repository");
        }

        private static void RegisterInterfaces(ContainerBuilder builder)
        {
            // Misc
            //builder.RegisterType<Recaptcha>().As<IRecaptcha>();
            //builder.RegisterType<WebClient>().As<IWebClient>();
            builder.RegisterType<RealClock>().As<IClock>();

            // DataAccess
            builder.RegisterType<MongoDbAccess>().As<IMongoDbAccess>();
            builder.RegisterType<MongoConnectionStringProvider>().As<IConnectionStringProvider>();
            //builder.RegisterType<EntityIdFactory>().As<IEntityIdFactory>();

            // ValidationResult
            builder.RegisterType<IntroValidationResult>().As<IValidationResult>();
            builder.RegisterType<EntityValidationResult>().As<IEntityValidationResult>();

            // Security
            builder.RegisterType<Tokenizer>().As<ITokenizer>();
            builder.RegisterType<UserIdentity>().As<IUserIdentityAdaptor>();
            builder.RegisterType<UserIdentityAdaptorFactory>().As<IUserIdentityAdaptorFactory>();
        }

        //private static void RegisterConcretes(ContainerBuilder builder)
        //{
        //    builder.RegisterType<EmailSubscription>().AsSelf();
        //}

        private static void RegisterSingletons(ContainerBuilder builder)
        {
            //builder.RegisterType<RecaptchaConfiguration>().As<IRecaptchaConfiguration>().SingleInstance();
            builder.RegisterType<IntroValidatorFactory>().As<IValidatorFactory>();
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            // Perform registrations that should have a request lifetime
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during request startup.
            //TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(container.Resolve<ITokenizer>()));
        }
    }
}