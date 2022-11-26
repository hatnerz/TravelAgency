using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TravelAgency.model
{
    public class Tour
    {
        public int Id { get; private set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivingDate { get; set; }
        public decimal BaseCost { get; set; }
        public decimal FlightCost { get; set; }
        public decimal FoodCost { get; set; }
        public Hotel Hotel { get; set; }
        public decimal MinPrice
        {
            get { return BaseCost + FlightCost; }
        }

        public Tour(int id, DateTime departureDate, DateTime arrivingDate, decimal baseCost, decimal flightCost, decimal foodCost, Hotel hotel)
        {
            Id = id;
            DepartureDate = departureDate;
            ArrivingDate = arrivingDate;
            BaseCost = baseCost;
            FlightCost = flightCost;
            FoodCost = foodCost;
            Hotel = hotel;
        }

        public Tour(int id)
        {
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string commandStr = "SELECT * FROM tours WHERE tour_id=" + id.ToString();
            SqlCommand command = new SqlCommand(commandStr, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.Id = (int)reader["tour_id"];
            DepartureDate = (DateTime)reader["departure_date"];
            ArrivingDate = (DateTime)reader["arriving_date"];
            BaseCost = (decimal)reader["base_cost"];
            FlightCost = (decimal)reader["flight_cost"];
            FoodCost = (decimal)reader["food_cost"];
            Hotel = new Hotel((int)reader["hotel_id"]);
            connection.Close();
        }
        
        public Tour(DataRow selectedTour)
        {
            this.Id = (int)selectedTour["tour_id"];
            DepartureDate = (DateTime)selectedTour["departure_date"];
            ArrivingDate = (DateTime)selectedTour["arriving_date"];
            BaseCost = (decimal)selectedTour["base_cost"];
            FlightCost = (decimal)selectedTour["flight_cost"];
            FoodCost = (decimal)selectedTour["food_cost"];
            Hotel = new Hotel((int)selectedTour["hotel_id"]);
        }
    }
}
