using System;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class DeleteUserCommand : BaseCommand
    {
        public DeleteUserCommand(AdressbuchViewModel pParent) : base(pParent) {}

        #region Implementation of BaseCommand

        public override bool CanExecute(object parameter)
        {
            return _parent.ThisContact != null;
        }
        
        public override void Execute(object parameter)
        {
            _parent.ContactList.Remove(_parent.ThisContact);
            _parent.ThisContact = null;
        }
        

        #endregion
    }
}
// string pFirstName = "fname",
// string pLastName = "lname",
// string pCity = "city",
// string pStreet = "street",
// string pHouseNumber = "N°",
// string pEmail = "email",
// string pTwitter = "twitter",
// string pFacebook = "facebook",
// string pLinkedIn = "linkedIn",
// string pXing = "xing",
// string pInstagram = "instagram",
// string pReddit = "reddit" )
