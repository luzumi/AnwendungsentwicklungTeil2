using System;
using MVVM_ViewModel;
using System.Windows.Input;

namespace MVVM_ViewModel
{
    public class AddUserCommand : ICommand
    {
        public MainViewModel Parent;

        #region Implementation of ICommand

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Parent.EntryList.Add(new UserViewModel {Name = "neu", Salary = 35000.0});
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
