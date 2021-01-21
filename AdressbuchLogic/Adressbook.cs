using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AdressbuchLogic.Commands;

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
            AddUser("Test0");
            AddUser("Test1");
            AddUser("Test2");
            Command_AddUser = new AddUserCommand(this);
        }

        public void AddUser(string pName)
        {
            contactList.Add(new Contact(pName));
        }


        public void EditUser(Contact pContact)
        {

        }


        public void DeleteUser(Contact pContact)
        {

        }

        public void LoadBrowser(Enum eNetworkTypeEnum)
        {
            
        }
    }
}
