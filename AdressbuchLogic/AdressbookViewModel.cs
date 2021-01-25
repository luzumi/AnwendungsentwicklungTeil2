using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AdressbuchLogic;

namespace AdressbuchLogic
{
    public class AdressbookViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BaseCommand CommandAddUser { get; set; }
        public BaseCommand CommandDeleteUser { get; set; }
        public BaseCommand CommandEditUser { get; set; }
        public BaseCommand CommandWeb { get; set; }


        public ObservableCollection<ContactViewModel> ContactList
        {
            get => _contactList;
            set
            {
                if (_contactList != value)

                {
                    _contactList = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContactList)));
                }
            }
        }
        private ObservableCollection<ContactViewModel> _contactList;


        public AdressbookViewModel()
        {
            _contactList = new ObservableCollection<ContactViewModel>();
            CommandAddUser = new AddUserCommand(this);
            CommandEditUser = new EditUserCommand(this);
            CommandDeleteUser = new DeleteUserCommand(this);
            ContactList = new();
        }


        public ContactViewModel ThisContact
        {
            get => _thisContact;
            set
            {
                if (_thisContact != value)
                {
                    _thisContact = value;
                    
                    (CommandDeleteUser as DeleteUserCommand)?.RaiseCanExecuteChanged(); 
                    (CommandEditUser as EditUserCommand)?.RaiseCanExecuteChanged(); 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThisContact)));
                }
            }
        }
        private ContactViewModel _thisContact;


        public bool ChangeView
        {
            get
            {
                return _changeView;
            }
            set
            {
                if (_changeView != value)

                {
                    _changeView = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangeView)));
                }
            }
        }
        private bool _changeView;

        public bool ChangeEditView
        {
            get
            {
                return _changeEditView;
            }
            set
            {
                if (_changeEditView != value)

                {
                    _changeEditView = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangeEditView)));
                }
            }
        }
        private bool _changeEditView;

        
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
