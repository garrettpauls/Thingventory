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
            var errors = args.Errors.Where(err => err.PropertyName == PropertyName);
            Errors.AddRange(errors, true);
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
