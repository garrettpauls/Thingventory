using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FluentValidation.Results;
using Template10.Utils;
using Thingventory.Core.Models;

namespace Thingventory.Xaml
{
    public sealed class ValidationWrapper : ContentControl
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target", typeof(IValidatingModel), typeof(ValidationWrapper),
            new PropertyMetadata(default(IValidatingModel), _HandleTargetChanged));

        public static readonly DependencyProperty ErrorsProperty = DependencyProperty.Register(
            "Errors", typeof(ObservableCollection<ValidationFailure>), typeof(ValidationWrapper),
            new PropertyMetadata(default(ObservableCollection<ValidationFailure>)));

        public static readonly DependencyProperty ErrorVisibilityProperty = DependencyProperty.Register(
            "ErrorVisibility", typeof(Visibility), typeof(ValidationWrapper), new PropertyMetadata(Windows.UI.Xaml.Visibility.Collapsed));

        public static readonly DependencyProperty FullErrorMessageProperty = DependencyProperty.Register(
            "FullErrorMessage", typeof(string), typeof(ValidationWrapper), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(ValidationWrapper), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            "PropertyName", typeof(string), typeof(ValidationWrapper), new PropertyMetadata(default(string)));

        public ValidationWrapper()
        {
            DefaultStyleKey = typeof(ValidationWrapper);

            Errors = new ObservableCollection<ValidationFailure>();
        }

        public ObservableCollection<ValidationFailure> Errors
        {
            get => (ObservableCollection<ValidationFailure>) GetValue(ErrorsProperty);
            private set => SetValue(ErrorsProperty, value);
        }

        public Visibility ErrorVisibility
        {
            get => (Visibility) GetValue(ErrorVisibilityProperty);
            private set => SetValue(ErrorVisibilityProperty, value);
        }

        public string FullErrorMessage
        {
            get => (string) GetValue(FullErrorMessageProperty);
            private set => SetValue(FullErrorMessageProperty, value);
        }

        public string Header
        {
            get => (string) GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public string PropertyName
        {
            get => (string) GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public IValidatingModel Target
        {
            get => (IValidatingModel) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        private void _HandlePropertyErrorsChanged(IValidatingModel sender, ValidationResult args)
        {
            var errors = sender
                .GetCurrentValidationFor(PropertyName)
                .Errors.Where(err => err.PropertyName == PropertyName)
                .ToArray();
            Errors.AddRange(errors, true);
            FullErrorMessage = string.Join(" ", errors.Select(err => err.ErrorMessage));
            ErrorVisibility = errors.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void _HandleTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ValidationWrapper wrapper))
            {
                return;
            }

            if (e.OldValue is IValidatingModel oldModel)
            {
                oldModel.PropertyErrorsChanged -= wrapper._HandlePropertyErrorsChanged;
            }

            if (e.NewValue is IValidatingModel newModel)
            {
                newModel.PropertyErrorsChanged += wrapper._HandlePropertyErrorsChanged;
            }
        }
    }
}
