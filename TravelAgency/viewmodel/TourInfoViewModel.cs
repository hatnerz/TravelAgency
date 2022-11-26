using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class TourInfoViewModel
    {
        public Tour CurrentTour;
        public TourInfoViewModel(Tour tour)
        {
            CurrentTour = tour;
        }
        public string RestHotel { get => "Відпочинок у " + CurrentTour.Hotel.Name; }
        public string Location { get => "ᐁ " + CurrentTour.Hotel.Country + " " + CurrentTour.Hotel.City; }
        public string Description { get => CurrentTour.Hotel.Description; }
        public string Stars { get => "Кількість зірок: " + CurrentTour.Hotel.Stars.ToString(); }
        public string RoomNumber { get => "Кількість номерів: " + CurrentTour.Hotel.RoomNumber.ToString(); }
        public string MinPrice { get => "Вартість: від " + CurrentTour.MinPrice.ToString(); }
        public string DepartureDate { get => "Дата вильоту: " + CurrentTour.DepartureDate.ToString(); }
        public string ArrivingDate { get => "Дата повернення: " + CurrentTour.ArrivingDate.ToString(); }
    }
}
