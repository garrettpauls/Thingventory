using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Thingventory.Core.Models
{
    [DataContract]
    public abstract class ChangeTrackingModel : INotifyPropertyChanged
    {
        private readonly HashSet<string> mIgnoreChangedPropertyNames;
        private bool mHasChanges;

        protected ChangeTrackingModel(params string[] ignoreChangedPropertyNames)
        {
            mIgnoreChangedPropertyNames = new HashSet<string>(ignoreChangedPropertyNames);
            mIgnoreChangedPropertyNames.Add(nameof(HasChanges));
        }

        [IgnoreDataMember]
        public bool HasChanges
        {
            get => mHasChanges;
            private set => Set(ref mHasChanges, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (!mIgnoreChangedPropertyNames.Contains(propertyName))
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
