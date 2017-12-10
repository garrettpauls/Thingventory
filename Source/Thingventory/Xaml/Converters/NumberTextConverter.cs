using System;
using System.Globalization;
using Windows.UI.Xaml.Data;
using Autofac;
using Thingventory.Core;

namespace Thingventory.Xaml.Converters
{
    public sealed class NumberTextConverter : IValueConverter
    {
        public string NumberFormat { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            if (targetType == typeof(string))
            {
                return _ConvertToString(value, parameter, language);
            }

            return _ConvertToNumber(value, targetType, language);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }

        private object _ConvertToNumber(object value, Type targetType, string language)
        {
            object result;

            if (targetType.IsClosedTypeOf(typeof(Nullable<>)))
            {
                try
                {
                    var actualTargetType = Nullable.GetUnderlyingType(targetType);
                    result = System.Convert.ChangeType(value, actualTargetType, _GetFormatProvider(language));
                }
                catch (Exception ex)
                {
                    LoggingModule.GetLog<NumberTextConverter>().Warn($"Failed to convert {value} to {targetType}", ex);
                    result = null;
                }
            }
            else
            {
                result = System.Convert.ChangeType(value, targetType, _GetFormatProvider(language));
            }

            return result;
        }

        private string _ConvertToString(object rawValue, object parameter, string language)
        {
            var format = parameter as string ?? NumberFormat;
            var provider = _GetFormatProvider(language);

            switch (rawValue)
            {
                case string value:
                    return value;
                case IFormattable value:
                    return value.ToString(format, provider);
                default:
                    return rawValue.ToString();
            }
        }

        private static CultureInfo _GetFormatProvider(string language)
        {
            var provider = CultureInfo.CurrentUICulture;
            if (!string.IsNullOrWhiteSpace(language))
            {
                provider = new CultureInfo(language);
            }
            return provider;
        }
    }
}
