using System.Collections.Concurrent;
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
        ValidationResult GetCurrentValidationFor(string propertyName);
        event TypedEventHandler<IValidatingModel, ValidationResult> PropertyErrorsChanged;
        ValidationResult ValidateAll();
        ValidationResult ValidateProperty(string propertyName);
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

        public ValidationResult GetCurrentValidationFor(string propertyName)
        {
            if (mResults.TryGetValue(propertyName, out var result))
            {
                return result;
            }

            return new ValidationResult();
        }

        public bool IsValid
        {
            get => mIsValid;
            private set => Set(ref mIsValid, value);
        }

        public event TypedEventHandler<IValidatingModel, ValidationResult> PropertyErrorsChanged;

        public ValidationResult ValidateAll()
        {
            var allResults = GetType()
                .GetProperties()
                .Select(property => property.Name)
                .Select(ValidateProperty)
                .SelectMany(result => result.Errors)
                .ToArray();

            return new ValidationResult(allResults);
        }

        public ValidationResult ValidateProperty(string propertyName)
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
                PropertyErrorsChanged?.Invoke(this, actualResult);
            }

            return actualResult;
        }

        private bool _AreEqual(ValidationResult left, ValidationResult right)
        {
            return left.IsValid == right.IsValid && left.Errors.SequenceEqual(right.Errors);
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
}
