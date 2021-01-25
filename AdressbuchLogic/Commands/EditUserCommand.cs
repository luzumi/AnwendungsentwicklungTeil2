using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class EditUserCommand : BaseCommand
    {
        public EditUserCommand(AdressbookViewModel pParent) : base(pParent) { }

        #region Implementation of BaseCommand

        public override bool CanExecute(object parameter)
        {
            // return _parent.ThisContact != null;
            return true;
        }


        public override void Execute(object parameter)
        {
            if (!_parent.ChangeView)
            {
                ContactViewModel cm = new ContactViewModel();
                _parent.ThisContact = cm;
            }
            else
            {
                _parent.logic.Save(_parent.ContactList.ToList());
            }

            _parent.ChangeView = !_parent.ChangeView;
        }

        #endregion
    }
}
