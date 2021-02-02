using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AdressbuchLogic
{
    public class AdressbuchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CommandAddUser { get; set; }
        public ICommand CommandDeleteUser { get; set; }
        public ICommand CommandEditUser { get; set; }
        public ICommand CommandWeb { get; set; }
        public DataStorage logic = new();



        public AdressbuchViewModel()
        {
            _contactList = new ObservableCollection<ContactViewModel>();
            CommandAddUser = new AddUserCommand(this);
            CommandEditUser = new EditUserCommand(this);
            CommandDeleteUser = new DeleteUserCommand(this);
            CommandWeb = new WebCommand(this);
            ContactList = new(logic.Load());
        }


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


        public string ContentFilter
        {
            get => _contentFilter;
            set
            {
                if (_contentFilter != value)
                {
                    _contentFilter = value;
                    ReloadContacts();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContentFilter)));
                }
            }
        }
        private string _contentFilter;


        public string InternetAdress
        {
            get => _internetAdress;
            set
            {
                if (_internetAdress != value)
                {
                    _internetAdress = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InternetAdress)));
                }
            }
        }
        private string _internetAdress;


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
            get => _changeView;
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


        private void ReloadContacts()
        {
            ContactList.Clear();
            foreach (var item in ContactList)
            {
                if (item.FirstName.Contains(_contentFilter) ||
                    item.LastName.Contains(_contentFilter) ||
                    item.City.Contains(_contentFilter) ||
                    item.Street.Contains(_contentFilter) ||
                    item.HouseNumber.Contains(_contentFilter) ||
                    item.Email.Contains(_contentFilter) ||
                    item.Twitter.Contains(_contentFilter) ||
                    item.Facebook.Contains(_contentFilter) ||
                    item.LinkedIn.Contains(_contentFilter) ||
                    item.Xing.Contains(_contentFilter) ||
                    item.Instagram.Contains(_contentFilter) ||
                    item.Reddit.Contains(_contentFilter))
                {
                    ContactList.Add(item);
                }
            }
        }
    }
}
