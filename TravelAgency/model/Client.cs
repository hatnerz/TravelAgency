using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }
        public Manager Manager { get; set; }
        public Client() { }

        public Client(int id, string firstName, string lastName, string patronymicName, string phoneNumber, string email, string passport, Manager manager)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PatronymicName = patronymicName;
            PhoneNumber = phoneNumber;
            Email = email;
            Passport = passport;
            Manager = manager;
        }

        public Client(DataRow selectedClient)
        {
            this.Id = (int)selectedClient["client_id"];
            FirstName = selectedClient["first_name"].ToString();
            LastName = selectedClient["last_name"].ToString();
            PatronymicName = selectedClient["patronymic_name"].ToString();
            PhoneNumber = selectedClient["phone"].ToString();
            Email = selectedClient["email"].ToString();
            Passport = selectedClient["passport"].ToString();
            if(selectedClient["manager_id"].GetType() != typeof(DBNull)) Manager = new Manager((int)selectedClient["manager_id"]);
        }
    }
}
