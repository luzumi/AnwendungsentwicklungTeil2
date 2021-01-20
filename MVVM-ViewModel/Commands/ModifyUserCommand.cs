using System;
using System.Windows.Input;

namespace MVVM_ViewModel
{
    public class ModifyUserCommand : ICommand
    {
        #region Implementation of ICommand

        public MainViewModel Parent;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Parent.EntryList[(int)parameter].Name = "geändert";
        } 


        #endregion
    }
}
