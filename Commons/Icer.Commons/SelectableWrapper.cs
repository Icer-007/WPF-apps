using System.Windows.Input;

namespace Icer.Commons
{
    /// <summary>
    /// Wrapper to put a single value into a view model that exposes a <see cref="IsSelected"/>
    /// boolean property (selectable version of <see cref="Wrapper{T}"/>)
    /// </summary>
    /// <typeparam name="T">Type of the wrapped value</typeparam>
    public class SelectableWrapper<T> : Wrapper<T>
    {
        /// <summary>
        /// Build a <see cref="SelectableWrapper{T}"/> and set the <see cref="object.ToString"/>
        /// result of <paramref name="value"/> into <see cref="AlternateDisplay"/>.
        /// </summary>
        /// <param name="value">Value to wrap</param>
        public SelectableWrapper(T value) : base(value)
        {
            this.CommandToggleSelection = new RelayCommand(() => this.IsSelected = !this.IsSelected);
        }

        /// <summary>
        /// Build a <see cref="SelectableWrapper{T}"/> with an alternate display value
        /// </summary>
        /// <param name="value">Value to wrap</param>
        /// <param name="alternateDisplay">
        /// String value to display instead of the <see cref="object.ToString"/> result of <paramref name="value"/>.
        /// </param>
        public SelectableWrapper(T value, string alternateDisplay) : base(value, alternateDisplay)
        {
            this.CommandToggleSelection = new RelayCommand(() => this.IsSelected = !this.IsSelected);
        }

        /// <summary>
        /// Built in command to toggle <see cref="IsSelected"/> value
        /// </summary>
        public ICommand CommandToggleSelection { get; }

        /// <summary>
        /// Indicates if instance is selected or not
        /// </summary>
        public bool IsSelected
        {
            get => this.GetProp<bool>();
            set => this.SetProp(value);
        }
    }
}
