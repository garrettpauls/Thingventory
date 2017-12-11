using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Thingventory.Xaml
{
    public sealed class TextBoxEx
    {
        public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnFocus", typeof(bool), typeof(TextBoxEx),
            new PropertyMetadata(default(bool), _HandleSelectAllOnFocusChanged));

        private static void _GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private static void _HandleSelectAllOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBox textBox))
            {
                return;
            }

            textBox.GotFocus -= _GotFocus;
            if (e.NewValue is bool enabled && enabled)
            {
                textBox.GotFocus += _GotFocus;
            }
        }

        public static bool GetSelectAllOnFocus(DependencyObject element)
        {
            return (bool) element.GetValue(SelectAllOnFocusProperty);
        }

        public static void SetSelectAllOnFocus(DependencyObject element, bool value)
        {
            element.SetValue(SelectAllOnFocusProperty, value);
        }
    }
}
