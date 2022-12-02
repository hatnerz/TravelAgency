using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    public class TourSubscription
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal MaxPrice { get; set; }
        public Client Client { get; set; }

        public TourSubscription(int id, DateTime registrationDate, string country, string city, decimal maxPrice, Client client)
        {
            Id = id;
            RegistrationDate = registrationDate;
            Country = country;
            City = city;
            MaxPrice = maxPrice;
            Client = client;
        }
    }
}
