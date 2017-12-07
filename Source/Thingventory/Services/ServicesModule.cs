using Autofac;

namespace Thingventory.Services
{
    public interface IService
    {
    }

    public sealed class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(ServicesModule).Assembly)
                .InNamespaceOf<ServicesModule>()
                .AssignableTo<IService>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
