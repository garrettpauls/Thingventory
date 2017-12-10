using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NavViewEx;
using Thingventory.ViewModels;

namespace Thingventory.Views
{
    public sealed partial class EditItemPage : Page, IHasViewModel<EditItemPageViewModel>, INavigationViewExHeaderProvider, INavigationViewExHeaderTemplateProvider
    {
        public EditItemPage()
        {
            InitializeComponent();
        }

        public EditItemPageViewModel ViewModel => DataContext as EditItemPageViewModel;
        public object Header => DataContext;
        public DataTemplate HeaderTemplate => HeaderDataTemplate;
    }
}
