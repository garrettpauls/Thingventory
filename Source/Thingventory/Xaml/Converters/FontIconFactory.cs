using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Thingventory.Xaml.Converters
{
    public sealed class FontIconFactory : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var fontIcon = new FontIcon
            {
                Glyph = value?.ToString() ?? parameter?.ToString() ?? ""
            };

            return fontIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
