using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency
{
    public class Manager
    {
        public int Id { get; private set; }
        private string login { get; set; }
        private string password { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PatronymicName { get; private set; }
        public bool Admin { get; private set; }
        public string OfficePhone { get; private set; }
        public Manager(int id, string login, string password, string firstName, string lastName, string patronymicName, bool admin, string officePhone)
        {
            this.Id = id;
            this.login = login;
            this.password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PatronymicName = patronymicName;
            this.Admin = admin;
            this.OfficePhone = officePhone;
        }
        public string GetFullName
        {
            get
            {
                return FirstName + " " + LastName + " " + PatronymicName;
            }
        }
    }
}
