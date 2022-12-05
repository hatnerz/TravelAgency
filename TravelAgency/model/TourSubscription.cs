using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.DbAdapters;

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

        public TourSubscription(DataRow subscriptionRow)
        {
            this.Id = (int)subscriptionRow["tour_subscription_id"];
            RegistrationDate = (DateTime)subscriptionRow["registration_date"];
            Country = subscriptionRow["country"].ToString();
            City = subscriptionRow["city"].ToString();
            MaxPrice = (decimal)subscriptionRow["max_price"];
            Client = ClientsAdapter.GetClient((int)subscriptionRow["client_id"]);
        }

        public TourSubscription(Client client)
        {
            Client = client;
        }
        public TourSubscription()
        { }
    }
}
