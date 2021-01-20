using System;
using System.Windows.Input;

namespace MVVM_ViewModel
{
    public class LoginCommand : ICommand
    {
        #region Implementation of ICommand
        public event EventHandler CanExecuteChanged;
        public MainViewModel Parent;


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Parent.EntryList[Parent.EntryList.Count-1].Name = "eingeloggt";
        }


        #endregion
    }
}
