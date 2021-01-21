using System;
using System.Collections.Generic;

namespace AdressbuchLogic
{
    public class Adressbook
    {
        private List<Contact> contactList;

        public List<Contact> ContactList
        {
            get => contactList;
            set => contactList = value;
        }

        public Adressbook()
        {
            contactList = new List<Contact>();
            AddUser("Test0");
            AddUser("Test1");
            AddUser("Test2");
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
