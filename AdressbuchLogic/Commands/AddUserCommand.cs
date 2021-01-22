using System;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class AddUserCommand : ICommand
    {
        #region Implementation of ICommand

        private readonly Adressbook Parent;

        public AddUserCommand(Adressbook pParent)
        {
            Parent = pParent;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Parent.VisibilityItem = Parent.VisibilityItem;
            Parent.ContactList.Add(new Contact("testen"));
        }

        public event EventHandler CanExecuteChanged;

        #endregion

        
    }
}
