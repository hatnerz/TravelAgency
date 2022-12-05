using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class TourSubscriptionViewModel
    {
        public TourSubscription CurrentTourSubscription;
        public string ClientFullName
        {
            get
            {
                return CurrentTourSubscription.Client.FirstName + " " + CurrentTourSubscription.Client.LastName + " " + CurrentTourSubscription.Client.PatronymicName;
            }
        }

        public string Email
        {
            get => CurrentTourSubscription.Client.Email;
        }

        public string Country
        {
            get => CurrentTourSubscription.Country;
            set => CurrentTourSubscription.Country = value;
        }

        public string City
        {
            get => CurrentTourSubscription.City;
            set => CurrentTourSubscription.City = value;
        }

        public decimal MaxPrice
        {
            get => CurrentTourSubscription.MaxPrice;
            set => CurrentTourSubscription.MaxPrice = value;
        }


        public TourSubscriptionViewModel(TourSubscription currentTourSubscription)
        {
            CurrentTourSubscription = currentTourSubscription;
        }
    }
}
