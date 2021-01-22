using System;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class AddUserCommand : ICommand
    {
        #region Implementation of ICommand

        private readonly AdressbookViewModel Parent;

        public AddUserCommand(AdressbookViewModel pParent)
        {
            Parent = pParent;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!Parent.ChangeView)
            {
                ContactViewModel cm = new ContactViewModel();

                Parent.ContactList.Add(cm);
                Parent.ThisContact = cm;
            }

            Parent.ChangeView = !Parent.ChangeView;
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
