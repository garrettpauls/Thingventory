using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using NavViewEx;
using Thingventory.Core.Models;
using Thingventory.ViewModels;

namespace Thingventory.Views
{
    public sealed partial class HomePage : Page, IHasViewModel<HomePageViewModel>, INavigationViewExHeaderProvider, INavigationViewExHeaderTemplateProvider
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public HomePageViewModel ViewModel => DataContext as HomePageViewModel;
        public object Header => DataContext;
        public DataTemplate HeaderTemplate => HeaderDataTemplate;

        private async void _HandleCommandClicked(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement elem) ||
                !(elem.DataContext is HomePageLocationViewModel vm))
            {
                return;
            }

            await _HandleItemCommand(elem.Tag?.ToString(), vm);
        }

        private async void _HandleCommandTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(sender is FrameworkElement elem) ||
                !(elem.DataContext is HomePageLocationViewModel vm))
            {
                return;
            }

            e.Handled = await _HandleItemCommand(elem.Tag?.ToString(), vm);
        }

        private void _HandleItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ItemSummary item)
            {
                ViewModel.EditItem(item);
            }
        }

        private async Task<bool> _HandleItemCommand(string action, HomePageLocationViewModel vm)
        {
            var handled = false;

            switch (action)
            {
                case "AddItem":
                    handled = true;
                    await vm.AddItemAsync();
                    break;
                case "DeleteLocation":
                    handled = true;
                    await ViewModel.DeleteLocationAsync(vm);
                    break;
                case "RenameLocation":
                    handled = true;
                    await vm.RenameAsync();
                    break;
            }

            return handled;
        }
    }
}
