using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Thingventory.Xaml.Converters
{
    public sealed class IsNullConverter : IValueConverter
    {
        public bool NullValue { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? NullValue : !NullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
