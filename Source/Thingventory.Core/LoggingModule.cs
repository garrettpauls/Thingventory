using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Common.Logging;

namespace Thingventory.Core
{
    public sealed class LoggingModule : Module
    {
        private static readonly ILogManager mLogManager = new LogManager();

        private void _HandleRegistrationPreparing(object sender, PreparingEventArgs e)
        {
            var limitType = e.Component.Activator.LimitType;

            e.Parameters = e
                .Parameters
                .Union(new[]
                {
                    new ResolvedParameter(
                        (p, ctx) => p.ParameterType == typeof(ILog),
                        (p, ctx) => ctx.Resolve<ILogManager>().GetLogger(limitType))
                });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += _HandleRegistrationPreparing;
        }

        public static ILog GetLog(Type type) => mLogManager.GetLogger(type);
        public static ILog GetLog<T>() => GetLog(typeof(T));

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterInstance(mLogManager)
                .As<ILogManager>()
                .SingleInstance();
        }
    }
}
