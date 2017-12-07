using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Template10.Common;
using Template10.Controls;
using Thingventory.Views;

namespace Thingventory
{
    public sealed partial class App : BootStrapper
    {
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

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            return NavigationService.NavigateAsync(typeof(HomePage));
        }
    }
}
