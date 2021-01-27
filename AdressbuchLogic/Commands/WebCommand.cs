using System;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class WebCommand : BaseCommand
    {
        #region Implementation of BaseCommand

        public override void Execute(object parameter)
        {
            _parent.InternetAdress = parameter as string;
        }
        
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public WebCommand( AdressbuchViewModel pParent) : base(pParent){}

        #endregion
    }
}
