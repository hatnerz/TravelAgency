using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    public class Hotel
    {
        private int id { get; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public byte Stars { get; set; }
        public string Description { get; set; }
        public short RoomNumber { get; set; }
        public Hotel(int id, string city, string country, string name, byte stars, string description, short roomNumber)
        {
            this.id = id;
            City = city;
            Country = country;
            Name = name;
            Stars = stars;
            Description = description;
            RoomNumber = roomNumber;
        }
        public Hotel(DataRowView selectedHotel)
        {
            this.id = (int)selectedHotel["hotel_id"];
            Country = selectedHotel["country"].ToString();
            City = selectedHotel["city"].ToString();
            Stars = (byte)selectedHotel["stars"];
            Description = selectedHotel["description"].ToString();
            RoomNumber = (short)selectedHotel["room_number"];
            Name = selectedHotel["name"].ToString();
        }
    }
}
