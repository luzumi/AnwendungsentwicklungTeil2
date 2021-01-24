using System;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class WebCommand : BaseCommand
    {
        #region Implementation of BaseCommand

        public override void Execute(object parameter)
        {
            
        }
        
        public WebCommand( AdressbookViewModel pParent) : base(pParent){}

        #endregion
    }
}
