using System;
using System.Windows.Input;

namespace AdressbuchLogic.Commands
{
    public class AddUserCommand : ICommand
    {
        #region Implementation of ICommand

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

        }

        public event EventHandler CanExecuteChanged;

        #endregion

        public AddUserCommand(Adressbook pAdressbook)
        {
            pAdressbook.AddUser("createTest");
        }
    }
}
