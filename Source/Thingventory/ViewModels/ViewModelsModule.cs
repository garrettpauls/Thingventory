using Autofac;
using Template10.Mvvm;

namespace Thingventory.ViewModels
{
    public sealed class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(ViewModelsModule).Assembly)
                .InNamespaceOf<ViewModelsModule>()
                .AssignableTo<ViewModelBase>()
                .AsSelf()
                .InstancePerDependency();
        }
    }
}
