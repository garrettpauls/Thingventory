using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using Common.Logging;
using Common.Logging.Simple;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Thingventory.Core;
using Thingventory.Core.Models;
using Thingventory.Core.Services;
using Thingventory.ViewModels;
using Thingventory.Views;
using UnhandledExceptionEventArgs = Windows.UI.Xaml.UnhandledExceptionEventArgs;

namespace Thingventory
{
    public sealed partial class App : BootStrapper
    {
        private IContainer mContainer;
        private ILifetimeScope mLifetime;
        private ILog mLog;

        public App()
        {
            mLog = LoggingModule.GetLog<App>();

            UnhandledException += _HandleUnhandledException;
        }

        private IContainer _BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<LoggingModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<ViewModelsModule>();
            builder.Register(_GetNavigationService).As<INavigationService>().InstancePerDependency();

            return builder.Build();

            object _GetNavigationService(IComponentContext arg)
            {
                return WindowWrapper.Current().NavigationServices.FirstOrDefault();
            }
        }

        private void _HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            mLog.Error($"Unhandled exception: {e.Message}", e.Exception);
        }

        private void _InitializeLogging()
        {
#if DEBUG
            LogManager.Adapter = new DebugLoggerFactoryAdapter(LogLevel.All, false, true, true, "s");
#else
            LogManager.Adapter = new NoOpLoggerFactoryAdapter();
#endif

            mLog = LoggingModule.GetLog<App>();
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

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            _InitializeLogging();
            mContainer = _BuildContainer();

            try
            {
                var invService = mContainer.Resolve<IInventoryService>();
                var inv = await invService.GetCurrentInventoryAsync();
                await invService.MigrateInventoryAsync(inv);

                mLifetime = mContainer.BeginLifetimeScope(builder => { builder.RegisterInstance(inv).As<Inventory>().SingleInstance(); });
            }
            catch (Exception ex)
            {
                mLog.Error("Initialization failed", ex);
                throw;
            }
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await NavigationService.NavigateAsync(typeof(HomePage));
        }

        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            var hasVM = page.GetType().GetInterfaces().FirstOrDefault(i => i.IsClosedTypeOf(typeof(IHasViewModel<>)));
            var vmType = hasVM?.GenericTypeArguments[0];

            if (vmType != null)
            {
                return (INavigable) mLifetime.Resolve(vmType);
            }

            return base.ResolveForPage(page, navigationService);
        }
    }
}
