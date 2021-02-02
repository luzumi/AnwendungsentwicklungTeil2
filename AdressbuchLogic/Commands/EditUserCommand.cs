using System.Linq;

namespace AdressbuchLogic
{
    public class EditUserCommand : BaseCommand
    {
        public EditUserCommand(AdressbuchViewModel pParent) : base(pParent) { }

        #region Implementation of BaseCommand

        public override bool CanExecute(object pArameter)
        {
            return _parent.ThisContact != null;

        }


        public override void Execute(object pArameter)
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
