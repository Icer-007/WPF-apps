namespace Icer.Commons
{
    public class RelayCommand : ICommandRaisable
    {
        private readonly Action? action;

        private readonly Func<bool>? canExecuteAction;

        private readonly Func<object?, bool>? canExecuteParamAction;

        private readonly bool isParameterized;

        private readonly Action<object?>? paramAction;

        /// <summary>
        /// Build a <see cref="RelayCommand"/>
        /// </summary>
        /// <param name="action"><see cref="Action"/> to execute</param>
        /// <param name="canExecuteAction">
        /// <see cref="Func{bool}"/> that determines if <paramref name="action"/> can be executed.
        /// If this parameter is null, <see cref="CanExecute(object?)"/> will always return <see cref="true"/>
        /// </param>
        public RelayCommand(Action action, Func<bool>? canExecuteAction = null)
            : this(action, canExecuteAction, null, null) { }

        /// <summary>
        /// Build a <see cref="RelayCommand"/>
        /// </summary>
        /// <param name="action"><see cref="Action{object?}"/> to execute</param>
        /// <param name="canExecuteAction">
        /// <see cref="Func{object?, bool}"/> that determines if <paramref name="action"/> can be
        /// executed. If this parameter is null, <see cref="CanExecute(object?)"/> will always
        /// return <see cref="true"/>
        /// </param>
        public RelayCommand(Action<object?> action, Func<object?, bool>? canExecuteAction = null)
            : this(null, null, action, canExecuteAction) { }

        protected RelayCommand(
            Action? action,
            Func<bool>? canExecuteAction,
            Action<object?>? paramAction,
            Func<object?, bool>? canExecuteParamAction)
        {
            if (action == null && paramAction == null)
            {
                throw new ArgumentNullException($"{nameof(action)} and {nameof(paramAction)}");
            }

            this.isParameterized = paramAction != null;

            this.action = action;
            this.canExecuteAction = canExecuteAction;

            this.paramAction = paramAction;
            this.canExecuteParamAction = canExecuteParamAction;
        }

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object? parameter = null)
        {
            return (this.isParameterized
                    ? this.canExecuteParamAction?.Invoke(parameter)
                    : this.canExecuteAction?.Invoke())
                   ?? true;
        }

        /// <inheritdoc/>
        public void Execute(object? parameter)
        {
            if (this.CanExecute(parameter))
            {
                if (this.isParameterized)
                    this.paramAction?.Invoke(parameter);
                else
                    this.action?.Invoke();
            }
        }

        /// <summary>
        /// Raise a <see cref="CanExecuteChanged"/> event
        /// </summary>
        public void RaiseCanExecuteChanged()
            => this.CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}
