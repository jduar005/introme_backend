using System;
using System.Reflection;
using Autofac;
using Intro.Application.Services;
using Intro.Application.Validation;
using Intro.DataAccess.Connection;
using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using Intro.Domain.ViewModels;
using Intro.Security;
using Intro.Utility.DotNetWrappers;
using Intro.Web.Serialization;
using MongoDB.Driver.GeoJsonObjectModel;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Responses;

namespace Intro.Web.Configuration
{
    //public class DevelopmentRootPathProvider : IRootPathProvider
    //{
    //    public string GetRootPath()
    //    {
    //        return @"C:\Projects\Punched\Punched.Web\";
    //    }
    //}

    public class Bootstrapper : AutofacNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/PunchedBootstrapper
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
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

        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        {
            get
            {
                var config = NancyInternalConfiguration.WithOverrides(cfg =>
                {
                    cfg.Serializers.Remove(typeof(DefaultJsonSerializer));
                    //                    cfg.Serializers.Remove(typeof(JsonBodyDeserializer));
                    //                    cfg.Serializers.Add(typeof (PunchedServiceStackJsonSerializer));

                    cfg.Serializers.Add(typeof(IntroNewtonsoftJsonSerializer));
                    //                    cfg.Serializers.Add(typeof (PunchedNewtonsoftJsonBodyDeserializer));
                });

                return config;
            }
        }

        private static void AddDirectoryConvention(NancyConventions nancyConventions, string requestedPath,
            string contentPath = null)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory(requestedPath,
                contentPath));
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during application startup.

            Nancy.Security.Csrf.Enable(pipelines);

            pipelines.AfterRequest += ctx => ctx.Response.WithHeader("Access-Control-Allow-Origin", "*");
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            // Perform registration that should have an application lifetime

            var builder = new ContainerBuilder();

            // TODO auth
//                builder.RegisterInstance<ITokenizer>(new Tokenizer(
//                    cfg =>
//                    {
//                        cfg.KeyExpiration(() => TimeSpan.FromDays(91));
//                        cfg.TokenExpiration(() => TimeSpan.FromDays(90));
//                    }));

            RegisterInterfaces(builder);
            RegisterGenerics(builder);

            //RegisterConcretes(builder);
            RegisterSingletons(builder);

            RegisterRepositories(builder);
            RegisterServices(builder);
            RegisterEntities(builder);

            RegisterProxyProcessors(builder);

            builder.Update(existingContainer.ComponentRegistry);
        }

        private static void RegisterInterfaces(ContainerBuilder builder)
        {
            // Logging
//                builder.RegisterType<NLogLoggingService>().As<ILoggingService>().SingleInstance();

            // Facebook access
//                builder.RegisterType<FacebookTokenAccess>().As<IFacebookTokenAccess>();

            // Misc
//                builder.RegisterType<RealClock>().As<IClock>();
//                builder.RegisterType<RestClient>().As<IRestClient>();

            // DataAccess
            builder.RegisterType<MongoDbAccess>().As<IMongoDbAccess>();
            builder.RegisterType<MongoConnectionStringProvider>().As<IConnectionStringProvider>();
            //builder.RegisterType<EntityIdFactory>().As<IEntityIdFactory>();

            // ValidationResult
            builder.RegisterType<IntroValidationResult>().As<IValidationResult>();
            builder.RegisterType<EntityValidationResult>().As<IEntityValidationResult>();

            // Security
//                builder.RegisterType<Tokenizer>().As<ITokenizer>();
            builder.RegisterType<UserIdentity>().As<IUserIdentityAdaptor>();
            builder.RegisterType<UserIdentityAdaptorFactory>().As<IUserIdentityAdaptorFactory>();
        }

        private static void RegisterProxyProcessors(ContainerBuilder builder)
        {
//                builder.RegisterType<Proxy>().As<IProxy>();
//                builder.RegisterType<ForwardingRequestCreator>().As<IForwardingRequestCreator>();
        }

        private static void RegisterEntities(ContainerBuilder builder)
        {
            var assembly = typeof(IEntity).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetTypeInfo().IsSubclassOf(typeof(Entity)))
                .As<Entity>();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            var assembly = typeof(IInventoryService).GetTypeInfo().Assembly;
            RegisterByConvention(builder, assembly, "Service");
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            var assembly = typeof(IUserRepository).GetTypeInfo().Assembly;
            RegisterByConvention(builder, assembly, "Repository");
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
        }

        //private static void RegisterConcretes(ContainerBuilder builder)
        //{
        //    builder.RegisterType<EmailSubscription>().AsSelf();
        //}

        private static void RegisterSingletons(ContainerBuilder builder)
        {
            //builder.RegisterType<RecaptchaConfiguration>().As<IRecaptchaConfiguration>().SingleInstance();
            //builder.RegisterType<PunchedValidatorFactory>().As<IValidatorFactory>();
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during request startup.
            // TODO auth
//                TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(container.Resolve<ITokenizer>()));
//
//                pipelines.OnError += HandleExceptions;
//
//                base.RequestStartup(container, pipelines, context);
        }

        private static Response HandleExceptions(NancyContext ctx, Exception exception)
        {
            // The magic strings "Exception"/"Validation" here become the name of the property in the Json
            //            var result = new JsonResponse(new PunchedValidationResult("Exception", $"{exception.Message}\n\nStack Trace: {exception.ToString()}"), new PunchedServiceStackJsonSerializer());

            // TODO handle exceptions
            throw new NotImplementedException();
//                var result = new JsonResponse(new EntityValidationResult(), new ServiceStackJsonSerializer());
//
//                if (exception is NotImplementedException)
//                {
//                    result.WithStatusCode(HttpStatusCode.NotImplemented);
//                }
//                else if (exception is UnauthorizedAccessException)
//                {
//                    result.WithStatusCode(HttpStatusCode.Unauthorized);
//                }
//                else if (exception is ArgumentException)
//                {
//                    result.WithStatusCode(result.StatusCode = HttpStatusCode.BadRequest);
//                }
//                else
//                {
//                    result.WithStatusCode(result.StatusCode = HttpStatusCode.InternalServerError);
//                }
//
//                return result;
        }

        // TODO find diagnostics
//            protected override DiagnosticsConfiguration DiagnosticsConfiguration => new DiagnosticsConfiguration { Password = @"reverbADMIN77" };
    }
}
