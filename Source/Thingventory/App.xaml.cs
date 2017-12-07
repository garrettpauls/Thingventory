using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Thingventory.Services;
using Thingventory.ViewModels;
using Thingventory.Views;

namespace Thingventory
{
    public sealed partial class App : BootStrapper
    {
        private IContainer mContainer;

        private IContainer _BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<ViewModelsModule>();
            builder.Register(_GetNavigationService).As<INavigationService>().InstancePerDependency();

            return builder.Build();

            object _GetNavigationService(IComponentContext arg)
            {
                return WindowWrapper.Current().NavigationServices.FirstOrDefault();
            }
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var shell = new Shell();
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, shell.NavigationFrame);
            shell.DataContext = new ShellViewModel(nav);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = shell
            };
        }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            mContainer = _BuildContainer();
            return base.OnInitializeAsync(args);
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            return NavigationService.NavigateAsync(typeof(HomePage));
        }

        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            var hasVM = page.GetType().GetInterfaces().FirstOrDefault(i => i.IsClosedTypeOf(typeof(IHasViewModel<>)));
            var vmType = hasVM?.GenericTypeArguments[0];

            if (vmType != null)
            {
                return (INavigable) mContainer.Resolve(vmType);
            }

            return base.ResolveForPage(page, navigationService);
        }
    }
}
