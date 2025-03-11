using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Icer.Commons
{
    /// <summary>
    /// Base class with <see cref="INotifyPropertyChanged"/> handling.
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object?> propValueByName;

        protected NotifyPropertyChangedBase()
        {
            this.propValueByName = [];
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        protected T? GetProp<T>([CallerMemberName] string propertyName = "")
            => this.PrivGetProp<T>(propertyName);

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void SetProp<T>(T newValue, [CallerMemberName] string propertyName = "")
        {
            var currentValue = this.PrivGetProp<T>(propertyName);

            if (!object.Equals(currentValue, newValue))
            {
                this.propValueByName[propertyName] = newValue;
                this.OnPropertyChanged(propertyName);
            }
        }

        protected bool SetPropWithCheck<T>(T newValue, [CallerMemberName] string propertyName = "")
        {
            var currentValue = this.PrivGetProp<T>(propertyName);

            if (!object.Equals(currentValue, newValue))
            {
                this.propValueByName[propertyName] = newValue;
                this.OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        private T? PrivGetProp<T>(string propertyName)
            => this.propValueByName.TryGetValue(propertyName, out object? value)
               ? (T?)value
               : default;
    }
}
