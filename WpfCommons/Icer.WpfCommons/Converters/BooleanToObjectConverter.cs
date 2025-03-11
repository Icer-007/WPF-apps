using System.Globalization;
using System.Windows.Data;

namespace Icer.WpfCommons.Converters
{
    /// <summary>
    /// Converter that can return two <see cref="object"/> intances, regarding the provided <see
    /// cref="bool"/> value
    /// </summary>
    public class BooleanToObjectConverter : IValueConverter
    {
        /// <summary>
        /// Result value of <see cref="Convert(object, Type, object, CultureInfo)"/> when value to
        /// convert is not "true"
        /// </summary>
        public object? FalseValue { get; set; }

        /// <summary>
        /// Result value of <see cref="Convert(object, Type, object, CultureInfo)"/> when value to
        /// convert is "true"
        /// </summary>
        public object? TrueValue { get; set; }

        /// <summary>
        /// Regarding the provided <paramref name="value"/>, returns <see cref="TrueValue"/> or <see cref="FalseValue"/>
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>
        /// <see cref="TrueValue"/> if <paramref name="value"/> is "true", <see cref="FalseValue"/> otherwise
        /// </returns>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value as bool? is true
               ? this.TrueValue
               : this.FalseValue;

        /// <summary>
        /// Converts the provided <paramref name="value"/> into a boolean
        /// </summary>
        /// <param name="value">The value to convert back</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>
        /// True if <paramref name="value"/> is equal to <see cref="TrueValue"/>, false otherwise
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value == this.TrueValue;
    }
}
