using Windows.UI.Xaml.Controls;

namespace Thingventory
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            InitializeComponent();
        }

        public Frame NavigationFrame => NavView.NavigationFrame;

        public ShellViewModel ViewModel => DataContext as ShellViewModel;
    }
}
