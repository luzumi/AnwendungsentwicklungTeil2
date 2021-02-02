using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AdressbuchLogic
{
    public class DataStorage
    {


        public void Save(List<ContactViewModel> pListContacts)
        {
            SQLiteConnectionStringBuilder builder = new();
            builder.Version = 3;
            builder.DataSource = "Contacts.db";

            using (SQLiteConnection con = new(builder.ToString()))
            {
                con.Open();
                // check if new db-file or existing

                var command = con.CreateCommand();
                command.CommandText = "SELECT count(name) FROM sqlite_master WHERE type = 'table' AND name = 'Contacts'";
                var result = command.ExecuteScalar();
                if ((Int64)result == 0)
                {
                    // tabelle neu aufbauen
                    command.CommandText = "create table Contacts (" +
                                          "fname varchar(50) , " +
                                          "lname varchar(50) , " +
                                          "city varchar(50) , " +
                                          "street varchar(50) , " +
                                          "houseno  varchar(50) , " +
                                          "email  varchar(50) , " +
                                          "twitter  varchar(50) , " +
                                          "facebook varchar(50) , " +
                                          "linkedIn varchar(50) , " +
                                          "xing varchar(50) , " +
                                          "instagram varchar(50) , " +
                                          "reddit varchar(50) )";
                    command.ExecuteNonQuery();
                }

                command.CommandText = "delete from Contacts;"; // sqlite kennt kein truncate als extra befehl. delete ohne where wird automatisch zu tuncate
                command.ExecuteNonQuery();
                foreach (var item in pListContacts)
                {
                    command.CommandText = "insert into Contacts values (" +
                                          "@fname, @lname, @city, @street, @houseno, @email, " +
                                          "@twitter, @facebook, @linkedIn, @xing, @instagram, @reddit);";
                    command.Parameters.AddWithValue("fname", item.FirstName);
                    command.Parameters.AddWithValue("lname", item.LastName);
                    command.Parameters.AddWithValue("city", item.City);
                    command.Parameters.AddWithValue("street", item.Street);
                    command.Parameters.AddWithValue("houseno", item.HouseNumber);
                    command.Parameters.AddWithValue("email", item.Email);
                    command.Parameters.AddWithValue("twitter", item.Twitter);
                    command.Parameters.AddWithValue("facebook", item.Facebook);
                    command.Parameters.AddWithValue("linkedIn", item.LinkedIn);
                    command.Parameters.AddWithValue("xing", item.Xing);
                    command.Parameters.AddWithValue("instagram", item.Instagram);
                    command.Parameters.AddWithValue("reddit", item.Reddit);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ContactViewModel> Load()
        {
            SQLiteConnectionStringBuilder builder = new();
            builder.Version = 3;
            builder.DataSource = "Contacts.db";

            List<ContactViewModel> resultList = new();
            using (SQLiteConnection con = new(builder.ToString()))
            {
                con.Open();
                var command = con.CreateCommand();
                // check if new db-file or existing
                command.CommandText = "SELECT count(name) FROM sqlite_master WHERE type = 'table' AND name = 'Contacts'";
                var result = command.ExecuteScalar();
                if ((Int64)result != 0)
                {
                    command.CommandText = "select fname, lname, city, street, houseno, email, " +
                                          "twitter, facebook, linkedIn, xing, instagram, reddit from contacts order by fname;";
                    using var reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        resultList.Add(new ContactViewModel(
                            reader.IsDBNull(0) ? "" : reader.GetString(0),
                            reader.IsDBNull(1) ? "" : reader.GetString(1),
                            reader.IsDBNull(2) ? "" : reader.GetString(2),
                            reader.IsDBNull(3) ? "" : reader.GetString(3),
                            reader.IsDBNull(4) ? "" : reader.GetString(4),
                            reader.IsDBNull(5) ? "" : reader.GetString(5),
                            reader.IsDBNull(6) ? "" : reader.GetString(6),
                            reader.IsDBNull(7) ? "" : reader.GetString(7),
                            reader.IsDBNull(8) ? "" : reader.GetString(8),
                            reader.IsDBNull(9) ? "" : reader.GetString(9),
                            reader.IsDBNull(10) ? "" : reader.GetString(10),
                            reader.IsDBNull(11) ? "" : reader.GetString(11)));
                    }
                }


            }
            return resultList;
        }
    }
}
