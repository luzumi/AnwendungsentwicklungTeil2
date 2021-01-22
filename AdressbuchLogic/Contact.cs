﻿using System;
using System.Collections.Generic;

namespace AdressbuchLogic
{
    public class Contact
    {
        private string city;
        private string street;
        private string houseNumber;
        private string email;
        private string twitter;
        private string facebook;
        private string linkedIn;
        private string xing;
        private string instagram;
        private string reddit;
        

        public string Name { get; set; }

        public string City
        {
            get => city;
            set => city = value;
        }

        public string Street
        {
            get => street;
            set => street = value;
        }

        public string HouseNumber
        {
            get => houseNumber;
            set => houseNumber = value;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }

        public string Twitter
        {
            get => twitter;
            set => twitter = value;
        }

        public string Facebook
        {
            get => facebook;
            set => facebook = value;
        }

        public string LinkedIn
        {
            get => linkedIn;
            set => linkedIn = value;
        }

        public string Xing
        {
            get => xing;
            set => xing = value;
        }

        public string Instagram
        {
            get => instagram;
            set => instagram = value;
        }

        public string Reddit
        {
            get => reddit;
            set => reddit = value;
        }

        public Contact(string pName)
        {
            Name = pName;
        }
    }
}
