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
            contactList.Add(new Contact("Test"));
        }
    }
}
