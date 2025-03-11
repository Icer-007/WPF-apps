using System.Globalization;
using System.Windows.Data;

namespace Icer.WpfCommons.Converters
{
    /// <summary>
    /// Converter that returns the inverse of provided <see cref="bool"/> value
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts <paramref name="value"/> to its inverse
        /// </summary>
        /// <param name="value"><see cref="bool"/> value to convert</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>False if <paramref name="value"/> is not "true", true otherwise</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value as bool? is not true;

        /// <summary>
        /// Converts back <paramref name="value"/> to its inverse
        /// </summary>
        /// <param name="value"><see cref="bool"/> value to convert back</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>False if <paramref name="value"/> is not "true", true otherwise</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value as bool? is not true;
    }
}
