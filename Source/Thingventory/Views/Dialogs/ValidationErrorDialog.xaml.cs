using Windows.UI.Xaml.Controls;
using FluentValidation.Results;

namespace Thingventory.Views.Dialogs
{
    public sealed partial class ValidationErrorDialog : ContentDialog
    {
        public ValidationErrorDialog(string message, ValidationResult result)
        {
            InitializeComponent();

            Message = message;
            Result = result;
        }

        public ValidationErrorDialog()
            : this("", new ValidationResult())
        {
        }

        public string Message { get; }
        public ValidationResult Result { get; }
    }
}
