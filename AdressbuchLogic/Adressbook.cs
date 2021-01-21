using System;
using System.Collections.Generic;
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
