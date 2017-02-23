using System;
using Autofac;
using Intro.Web.Configuration.Autofac;
using Nancy.Configuration;

namespace Intro.Web.Configuration
{

    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override INancyEnvironmentConfigurator GetEnvironmentConfigurator()
        {
            throw new NotImplementedException();
        }

        public override INancyEnvironment GetEnvironment()
        {
            throw new NotImplementedException();
        }

        protected override void RegisterNancyEnvironment(ILifetimeScope container, INancyEnvironment environment)
        {
            throw new NotImplementedException();
        }
    }
}