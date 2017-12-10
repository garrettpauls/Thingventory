using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Windows.Foundation;
using FluentValidation;
using FluentValidation.Results;

namespace Thingventory.Core.Models
{
    public interface IValidatingModel : INotifyPropertyChanged
    {
        bool IsValid { get; }
        event TypedEventHandler<IValidatingModel, PropertyValidationResult> PropertyErrorsChanged;
        IReadOnlyCollection<PropertyValidationResult> ValidateAll();
        PropertyValidationResult ValidateProperty(string propertyName);
    }

    public abstract class ValidatingModel<TSelf> : ChangeTrackingModel, IValidatingModel
        where TSelf : ValidatingModel<TSelf>
    {
        private readonly ConcurrentDictionary<string, ValidationResult> mResults = new ConcurrentDictionary<string, ValidationResult>();
        private readonly IValidator<TSelf> mValidator;
        private bool mIsValid;

        protected ValidatingModel(IValidator<TSelf> validator)
            : base(nameof(IsValid))
        {
            mValidator = validator;
        }

        public bool IsValid
        {
            get => mIsValid;
            private set => Set(ref mIsValid, value);
        }

        public event TypedEventHandler<IValidatingModel, PropertyValidationResult> PropertyErrorsChanged;

        public IReadOnlyCollection<PropertyValidationResult> ValidateAll()
        {
            return GetType()
                .GetProperties()
                .Select(property => property.Name)
                .Select(ValidateProperty)
                .ToArray();
        }

        public PropertyValidationResult ValidateProperty(string propertyName)
        {
            bool isDifferent = false;
            var actualResult = mResults.AddOrUpdate(propertyName, _ =>
            {
                isDifferent = true;
                return mValidator.Validate(_GetSelf(), propertyName);
            }, (_, oldResults) =>
            {
                var results = mValidator.Validate(_GetSelf(), propertyName);
                isDifferent = !_AreEqual(oldResults, results);

                return results;
            });

            _UpdateIsValid();

            if (isDifferent)
            {
                PropertyErrorsChanged?.Invoke(this, new PropertyValidationResult(propertyName, actualResult));
            }

            return new PropertyValidationResult(propertyName, actualResult);
        }

        private bool _AreEqual(ValidationResult left, ValidationResult right)
        {
            return left.IsValid != right.IsValid || !left.Errors.SequenceEqual(right.Errors);
        }

        private TSelf _GetSelf() => (TSelf) this;

        private void _UpdateIsValid()
        {
            IsValid = mResults.IsEmpty || mResults.Values.All(r => r.IsValid);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            ValidateProperty(propertyName);
        }
    }

    public sealed class PropertyValidationResult
    {
        public PropertyValidationResult(string propertyName, ValidationResult result)
        {
            PropertyName = propertyName;
            Result = result;
        }

        public string PropertyName { get; }
        public ValidationResult Result { get; }
    }
}
