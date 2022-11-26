using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class TripSettingsViewModel : ViewModelBase
    {
        public Trip FormedTrip;

        public Dictionary<string, string> PlaneClassesShort = new Dictionary<string, string>()
        {
            { "Business", "B"},
            { "Econom", "E"},
            { "First", "F"}
        };

        public Dictionary<string, string> PlaneClassesFull = new Dictionary<string, string>()
        {
            { "B", "Business"},
            { "E", "Econom"},
            { "F", "First"}
        };

        public Dictionary<string, decimal> PlaneClassPrice = new Dictionary<string, decimal>()
        {
            { "E", 1.0m},
            { "B", 2.5m},
            { "F", 6.0m}
        };

        public Dictionary<string, string> FoodTypeShort = new Dictionary<string, string>()
        {
            { "Room Only", "RO"},
            { "Bed & Breakfast", "BB"},
            { "Half Board", "HB"},
            { "Full Board", "FB"},
            { "All Inclusive", "AI"},
        };

        public Dictionary<string, string> FoodTypeFull = new Dictionary<string, string>()
        {
            { "RO", "Room Only"},
            { "BB", "Bed & Breakfast"},
            { "HB", "Half Board"},
            { "FB", "Full Board"},
            { "AI", "All Inclusive"},
        };

        public Dictionary<string, decimal> FoodTypePrice = new Dictionary<string, decimal>()
        {
            { "RO", 0m},
            { "BB", 0.3m},
            { "HB", 0.6m},
            { "FB", 1m},
            { "AI", 1.5m},
        };

    
        public TripSettingsViewModel(Trip formedTrip)
        {
            FormedTrip = formedTrip;
        }
        public decimal BaseCost
        {
            get => FormedTrip.Tour.BaseCost;
            set
            {
                FormedTrip.Tour.BaseCost = value;
            }
        }
        public string PlaneClass
        {
            get => PlaneClassesFull[FormedTrip.PlaneClass];
            set
            {
                FormedTrip.PlaneClass = PlaneClassesShort[value];
                OnPropertyChanged(nameof(PlaneCost));
                OnPropertyChanged(nameof(FullCost));
            }
        }
        public decimal PlaneCost
        {
            get => FormedTrip.Tour.FlightCost * PlaneClassPrice[FormedTrip.PlaneClass];
        }

        public string FoodType
        {
            get => FoodTypeFull[FormedTrip.FoodType];
            set
            {
                FormedTrip.FoodType = FoodTypeShort[value];
                OnPropertyChanged(nameof(FoodCost));
                OnPropertyChanged(nameof(FullCost));
            }
        }

        public decimal FoodCost
        {
            get => FormedTrip.Tour.FoodCost * FoodTypePrice[FormedTrip.FoodType];
        }
        public decimal FullCost
        {
            get => FoodCost + PlaneCost + FormedTrip.Tour.BaseCost;
        }

    }
}
