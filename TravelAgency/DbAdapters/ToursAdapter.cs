using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;
using MaterialDesignThemes.Wpf;
using System.Reflection;
using TravelAgency.view.pages;
using iTextSharp.text.pdf;
using System.Windows;

namespace TravelAgency.DbAdapters
{
    internal static class ToursAdapter
    {
        public static int GetLastId()
        {
            string sqlExpression = "SELECT CONVERT(int, IDENT_CURRENT('tours'))";
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var a = command.ExecuteScalar();
                return (int)a;
            }
        }

        public static Tour GetTour(int id)
        {
            try
            {
                Tour tour = new Tour();
                SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                string commandStr = "SELECT * FROM tours WHERE tour_id=" + id.ToString();
                SqlCommand command = new SqlCommand(commandStr, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                tour.Id = (int)reader["tour_id"];
                tour.DepartureDate = (DateTime)reader["departure_date"];
                tour.ArrivingDate = (DateTime)reader["arriving_date"];
                tour.BaseCost = (decimal)reader["base_cost"];
                tour.FlightCost = (decimal)reader["flight_cost"];
                tour.FoodCost = (decimal)reader["food_cost"];
                if (reader["hotel_id"].GetType() != typeof(DBNull))
                    tour.Hotel = HotelsAdapter.GetHotel((int)reader["hotel_id"]);
                connection.Close();
                return tour;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        public static void FillClientsByTour(Tour tour, DataTable toursDataTable)
        {
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                string commandStr = 
                    "SELECT ROW_NUMBER() OVER(ORDER BY clients.first_name ASC) as '№', clients.first_name as 'Прізвище', clients.last_name as 'Ім`я', clients.patronymic_name 'По-батькові', "+
                    "trips.food_type 'Тип харчування', trips.plane_class 'Клас у літаку' "+
                    "FROM trips INNER JOIN clients on trips.client_id = clients.client_id "+
                    "WHERE trips.tour_id = " + tour.Id;
                SqlCommand command = new SqlCommand(commandStr, connection);
                SqlDataAdapter oda = new SqlDataAdapter(command);
                toursDataTable.Clear();
                oda.Fill(toursDataTable);
                connection.Close();
            }
        }
    }
}
