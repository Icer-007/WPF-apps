namespace Icer.Commons
{
    /// <summary>
    /// Wrapper to put a single value into a view model
    /// </summary>
    /// <typeparam name="T">Type of the wrapped value</typeparam>
    public class Wrapper<T> : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Build a <see cref="Wrapper{T}"/> and set the <see cref="object.ToString"/> result of
        /// <paramref name="value"/> into <see cref="AlternateDisplay"/>.
        /// </summary>
        /// <param name="value">Value to wrap</param>
        public Wrapper(T value) : this(value, value?.ToString()) { }

        /// <summary>
        /// Build a <see cref="Wrapper{T}"/> with an alternate display value
        /// </summary>
        /// <param name="value">Value to wrap</param>
        /// <param name="alternateDisplay">
        /// String value to display instead of the <see cref="object.ToString"/> result of <paramref name="value"/>.
        /// </param>
        public Wrapper(T value, string? alternateDisplay)
        {
            this.Value = value;
            this.AlternateDisplay = alternateDisplay;
        }

        /// <summary>
        /// String display of <see cref="Value"/> or alternate display if provided in <see
        /// cref="Wrapper{T}.Wrapper(T, string?)"/>
        /// </summary>
        public string? AlternateDisplay { get; }

        /// <summary>
        /// The wrapped value
        /// </summary>
        public T Value { get; }
    }
}
