using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Thingventory.Core.Models
{
    [DataContract]
    public abstract class ChangeTrackingModel : INotifyPropertyChanged
    {
        private bool mHasChanges;

        [IgnoreDataMember]
        public bool HasChanges
        {
            get => mHasChanges;
            private set => Set(ref mHasChanges, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName != nameof(HasChanges))
            {
                HasChanges = true;
            }
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ResetHasChanges()
        {
            HasChanges = false;
        }

        protected bool Set<TProperty>(ref TProperty backingField, TProperty value, [CallerMemberName] string propertyName = null)
        {
            var changed = !Equals(backingField, value);

            if (changed)
            {
                backingField = value;
                RaisePropertyChanged(propertyName);
            }

            return changed;
        }
    }
}
