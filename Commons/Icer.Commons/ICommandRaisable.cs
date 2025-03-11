using System.Windows.Input;

namespace Icer.Commons
{
    public interface ICommandRaisable : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
