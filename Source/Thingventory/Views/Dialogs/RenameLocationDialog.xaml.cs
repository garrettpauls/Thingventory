using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Thingventory.Core.Models;

namespace Thingventory.Views.Dialogs
{
    public sealed partial class RenameLocationDialog : ContentDialog
    {
        private readonly string mOriginalName;
        private readonly string mOriginalNotes;

        public RenameLocationDialog(Location location)
        {
            Location = location;

            mOriginalName = location.Name;
            mOriginalNotes = location.Notes;

            IsPrimaryButtonEnabled = Location.ValidateAll().IsValid;

            Location.PropertyChanged += _HandleLocationPropertyChanged;

            Closing += _HandleClosing;

            InitializeComponent();
        }

        public RenameLocationDialog()
            : this(new Location(0))
        {
        }

        public Location Location { get; }

        private void _HandleClosing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            Location.PropertyChanged -= _HandleLocationPropertyChanged;

            if (Location.HasChanges && !Location.ValidateAll().IsValid)
            {
                _RevertChanges();
            }
        }

        private void _HandleLocationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Location.IsValid))
            {
                IsPrimaryButtonEnabled = Location.IsValid;
            }
        }

        private void _HandleSecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _RevertChanges();
        }

        private void _RevertChanges()
        {
            Location.Name = mOriginalName;
            Location.Notes = mOriginalNotes;
            Location.ResetHasChanges();
        }
    }
}
