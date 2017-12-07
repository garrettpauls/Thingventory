using Windows.UI.Xaml.Controls;
using Thingventory.ViewModels;

namespace Thingventory.Views
{
    public sealed partial class HomePage : Page, IHasViewModel<HomePageViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public HomePageViewModel ViewModel => DataContext as HomePageViewModel;
    }
}
