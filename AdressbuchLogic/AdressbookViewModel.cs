using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AdressbuchLogic;

namespace AdressbuchLogic
{
    public class AdressbookViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ContactViewModel> contactList;
        public ICommand Command_AddUser { get; set; }


        public ObservableCollection<ContactViewModel> ContactList
        {
            get => contactList;
            set
            {
                if (contactList != value)

                {
                    contactList = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContactList)));
                }
            }
        }

        public AdressbookViewModel()
        {
            contactList = new ObservableCollection<ContactViewModel>();
            Command_AddUser = new AddUserCommand(this);
        }

        private ContactViewModel _thisContact;
        public ContactViewModel ThisContact
        {
            get => _thisContact;
            set
            {

                if (_thisContact != value)

                {
                    _thisContact = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThisContact)));
                }
            }
        }

        //private Visibilitys _visibilitys;
        //public Visibilitys VisibilityItem
        //{
        //    get => _visibilitys;
        //    set
        //    {
        //        switch (value)
        //        {
        //            case Visibilitys.Collapsed:

        //                _visibilitys = Visibilitys.Visible;
        //                break;
        //            case Visibilitys.Visible:
        //                _visibilitys = Visibilitys.Collapsed;
        //                break;
        //        }
        //    }
        //}



        private bool _ChangeView;
        public bool ChangeView
        {
            get
            {
                return _ChangeView;
            }
            set
            {
                if (_ChangeView != value)

                {
                    _ChangeView = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangeView)));
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
