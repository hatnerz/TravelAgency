using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class ClientViewModel
    {
        public Client CurrentClient;
        public string FirstName 
        {
            get => CurrentClient.FirstName;
            set => CurrentClient.FirstName = value;
        }
        public string LastName
        {
            get => CurrentClient.LastName;
            set => CurrentClient.LastName = value;
        }
        public string PatronymicName
        {
            get => CurrentClient.PatronymicName;
            set => CurrentClient.PatronymicName = value;
        }
        public string PhoneNumber
        {
            get => CurrentClient.PhoneNumber; 
            set => CurrentClient.PhoneNumber = value;
        }
        public string Email
        {
            get => CurrentClient.Email;
            set => CurrentClient.Email = value;
        }
        public string Passport
        {
            get => CurrentClient.Passport; 
            set => CurrentClient.Passport = value;
        }
        public ClientViewModel(Client currentClient)
        {
            CurrentClient = currentClient;
        }
    }
}
