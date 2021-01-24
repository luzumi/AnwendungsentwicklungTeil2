﻿using System;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class AddUserCommand : BaseCommand
    {
        #region Implementation of BaseCommand

        public AddUserCommand(AdressbookViewModel pParent) : base(pParent) { }

        
        public override void Execute(object parameter)
        {
            if (!_parent.ChangeView)
            {
                ContactViewModel cm = new ContactViewModel();

                _parent.ContactList.Add(cm);
                _parent.ThisContact = cm;
            }
            _parent.ChangeView = !_parent.ChangeView;
        }

        #endregion
    }
}
