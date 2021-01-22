using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class Adressbook
    {
        private ObservableCollection<Contact> contactList;
        public ICommand Command_AddUser { get; set; }


        public ObservableCollection<Contact> ContactList
        {
            get => contactList;
            set => contactList = value;
        }

        public Adressbook()
        {
            contactList = new ObservableCollection<Contact>();
            Command_AddUser = new AddUserCommand(this);
        }

        public Contact ThisContact { get; set; }

        private Visibilitys _visibilitys;
        public Visibilitys VisibilityItem
        {
            get => _visibilitys;
            set
            {
                switch (value)
                {
                    case Visibilitys.Collapsed:
                        
                        _visibilitys = Visibilitys.Visible;
                        break;
                    case Visibilitys.Visible:
                        _visibilitys = Visibilitys.Collapsed;
                        break;
                }
            }
        }

        public enum Visibilitys
        {
            Collapsed,
            Hidden,
            Visible
        }

        public ICommand AddUser
        {
            get;
            set;
        }


        public ICommand EditUser
        {
            get;
            set;
        }


        public ICommand DeleteUser
        {
            get;
            set;
        }

        public ICommand LoadBrowser
        {
            get;
            set;
        }
    }
}
