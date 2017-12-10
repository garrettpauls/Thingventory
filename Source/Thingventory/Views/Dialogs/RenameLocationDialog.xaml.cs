using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Thingventory.Views.Dialogs
{
    public sealed partial class RenameLocationDialog : ContentDialog
    {
        public RenameLocationDialog()
        {
            InitializeComponent();
        }

        public string LocationName
        {
            get => NameBox.Text;
            set
            {
                NameBox.Text = value ?? "";
                NameBox.SelectAll();
                _HandleNameChanged(NameBox, null);
            }
        }

        public string LocationNotes
        {
            get => NotesBox.Text;
            set => NotesBox.Text = value ?? "";
        }

        private void _HandleNameChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                NameIsEmptyWarning.Visibility = Visibility.Visible;
                IsPrimaryButtonEnabled = false;
            }
            else
            {
                NameIsEmptyWarning.Visibility = Visibility.Collapsed;
                IsPrimaryButtonEnabled = true;
            }
        }
    }
}
