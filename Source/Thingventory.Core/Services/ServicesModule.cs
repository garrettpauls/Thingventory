using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Thingventory.Core.Services
{
    public interface IService
    {
    }

    public sealed class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(ServicesModule).GetTypeInfo().Assembly)
                .InNamespaceOf<ServicesModule>()
                .AssignableTo<IService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
