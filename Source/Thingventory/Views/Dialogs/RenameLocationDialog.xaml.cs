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
            set => NameBox.Text = value ?? "";
        }

        public string LocationNotes
        {
            get => NotesBox.Text;
            set => NotesBox.Text = value ?? "";
        }
    }
}
