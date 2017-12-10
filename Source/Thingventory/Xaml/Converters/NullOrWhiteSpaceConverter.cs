using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Thingventory.Xaml.Converters
{
    public sealed class NullOrWhiteSpaceConverter : IValueConverter
    {
        public object IfNullOrEmpty { get; set; }
        public object Otherwise { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value is string str && string.IsNullOrWhiteSpace(str))
            {
                return IfNullOrEmpty;
            }

            return Otherwise;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
