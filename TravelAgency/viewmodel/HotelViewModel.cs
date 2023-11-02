using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class HotelViewModel
    {
        public Hotel CurrentHotel;

        public string City
        {
            get => CurrentHotel.City;
            set => CurrentHotel.City = value;
        }

        public string Country
        {
            get => CurrentHotel.Country;
            set => CurrentHotel.Country = value;
        }

        public string Description
        {
            get => CurrentHotel.Description;
            set => CurrentHotel.Description = value;
        }

        public byte Stars
        {
            get => CurrentHotel.Stars;
            set => CurrentHotel.Stars = value;
        }

        public string Name
        {
            get => CurrentHotel.Name;
            set => CurrentHotel.Name = value;
        }

        public short RoomNumber
        {
            get => CurrentHotel.RoomNumber;
            set => CurrentHotel.RoomNumber = value;
        }

        public HotelViewModel(Hotel currenthotel)
        {
            CurrentHotel = currenthotel;
        }
    }
}
