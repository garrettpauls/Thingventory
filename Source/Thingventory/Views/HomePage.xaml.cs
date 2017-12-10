using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using NavViewEx;
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

        private async void _HandleCommandTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(sender is FrameworkElement elem) ||
                !(elem.DataContext is HomePageLocationViewModel vm))
            {
                return;
            }

            switch (elem.Tag?.ToString())
            {
                case "AddItem":
                    e.Handled = true;
                    await vm.AddItemAsync();
                    break;
                case "RenameLocation":
                    e.Handled = true;
                    await vm.RenameAsync();
                    break;
            }
        }
    }
}
