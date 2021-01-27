using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AdressbuchLogic
{
    public class ContactViewModel : INotifyPropertyChanged
    {
        private string _firstName = "Daniel";
        private string _lastName = "Neubieser";
        private string _street;
        private string _city;
        private string _houseNumber;
        private string _email;
        private string _twitter;
        private string _facebook;
        private string _linkedIn;
        private string _xing;
        private string _instagram;
        private string _reddit;


        public ObservableCollection<string> WebProfiles { get; set; }


        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
                }
            }
        }
        
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
                }
            }
        }

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(City)));
                }
            }
        }

        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                if (_street != value)
                {
                    _street = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Street)));
                }
            }
        }

        public string HouseNumber
        {
            get
            {
                return _houseNumber;
            }
            set
            {
                if (_city != value)
                {
                    _houseNumber = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HouseNumber)));
                }
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public string Twitter
        {
            get
            {
                return _twitter;
            }
            set
            {
                if (_twitter != value)
                {
                    _twitter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Twitter)));
                }
            }
        }

        public string Facebook
        {
            get
            {
                return _facebook;
            }
            set
            {
                if (_facebook != value)
                {
                    _facebook = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((nameof(Facebook))));
                }
            }
        }

        public string LinkedIn
        {
            get
            {
                return _linkedIn;
            }
            set
            {
                if (_linkedIn != value)
                {
                    _linkedIn = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LinkedIn)));
                }
            }
        }

        public string Xing
        {
            get
            {
                return _xing;
            }
            set
            {
                if (_xing != value)
                {
                    _xing = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Xing)));
                }
            }
        }

        public string Instagram
        {
            get
            {
                return _instagram;
            }
            set
            {
                if (_instagram != value)
                {
                    _instagram = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Instagram)));
                }
            }
        }

        public string Reddit
        {
            get
            {
                return _reddit;
            }
            set
            {
                if (_reddit != value)
                {
                    _reddit = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reddit)));
                }
            }
        }

        public ContactViewModel(string pFirstName = "",
                                string pLastName = "",
                                string pCity = "",
                                string pStreet = "",
                                string pHouseNumber = "",
                                string pEmail = "",
                                string pTwitter = "",
                                string pFacebook = "",
                                string pLinkedIn = "",
                                string pXing = "",
                                string pInstagram = "",
                                string pReddit = "")
        {
            _firstName = pFirstName;
            _lastName = pLastName;
            _city = pCity;
            _street = pStreet;
            _houseNumber = pHouseNumber;
            _email = pEmail;
            _twitter = pTwitter;
            _facebook = pFacebook;
            _linkedIn = pLinkedIn;
            _xing = pXing;
            _instagram = pInstagram;
            _reddit = pReddit;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
