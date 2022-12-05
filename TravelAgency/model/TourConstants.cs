using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    internal class TourConstants
    {
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
    }
}
