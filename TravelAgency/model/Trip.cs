using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    public class Trip
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string FoodType { get; set; }
        public string PlaneClass { get; set; }
        public Tour Tour { get; set; }
        public Client Client { get; set; }
        public Trip(int id, DateTime registrationDate, string foodType, string planeClass, Tour tour, Client client)
        {
            Id = id;
            RegistrationDate = registrationDate;
            FoodType = foodType;
            PlaneClass = planeClass;
            Tour = tour;
            Client = client;
        }
    }
}
