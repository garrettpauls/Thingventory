using Template10.Mvvm;

namespace Thingventory.Models
{
    public abstract class ChangeTrackingModel : BindableBase
    {
        private bool mHasChanges;

        protected ChangeTrackingModel()
        {
            PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
        }

        public bool HasChanges
        {
            get => mHasChanges;
            private set => Set(ref mHasChanges, value);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName != nameof(HasChanges))
            {
                HasChanges = true;
            }
        }

        public void ResetHasChanges()
        {
            HasChanges = false;
        }
    }
}
