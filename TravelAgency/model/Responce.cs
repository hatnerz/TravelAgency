using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    public class Responce
    {
        public int Id;
        public byte Score;
        public string Comment;
        public Hotel Hotel;
        public Trip Trip;

        public Responce(int id, byte score, string comment, Hotel hotel, Trip trip)
        {
            Id = id;
            Score = score;
            Comment = comment;
            Hotel = hotel;
            Trip = trip;
        }
        
        public Responce()
        { }
    }
}
