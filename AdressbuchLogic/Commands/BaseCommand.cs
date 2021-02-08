using System;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public abstract class BaseCommand : ICommand
    {
        protected readonly AdressbuchViewModel _parent;
        public event EventHandler CanExecuteChanged;
        public abstract void Execute(object parameter);

        protected BaseCommand(AdressbuchViewModel parent)
        {
            _parent = parent;
        }

        public virtual bool CanExecute(object parameter) { return true; }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
