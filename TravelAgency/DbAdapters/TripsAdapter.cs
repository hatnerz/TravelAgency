using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using TravelAgency.model;

namespace TravelAgency.DbAdapters
{
    internal class TripsAdapter
    {
        static public void InsertTrip(Trip trip)
        {
            try
            {
                string sqlExpression =
               "INSERT INTO trips (registration_date, food_type, plane_class, tour_id, client_id) " +
               "VALUES (@registrationDate, @foodType, @planeClass, @tourId, @clientId)";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@registrationDate", trip.RegistrationDate));
                    command.Parameters.Add(new SqlParameter("@foodType", trip.FoodType));
                    command.Parameters.Add(new SqlParameter("@planeClass", trip.PlaneClass));
                    command.Parameters.Add(new SqlParameter("@tourId", trip.Tour.Id));
                    command.Parameters.Add(new SqlParameter("@clientId", trip.Client.Id));
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        static public void FillTripsByClient(Client client, DataTable tripsDataTable)
        {
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                string commandStr =
                    "SELECT tours.tour_id, hotels.country, hotels.city, hotels.name, tours.departure_date, tours.arriving_date, trips.food_type, trips.plane_class " +
                    "FROM hotels INNER JOIN tours on hotels.hotel_id = tours.hotel_id INNER JOIN trips ON tours.tour_id = trips.tour_id " +
                    "WHERE trips.client_id = @clientId";
                SqlCommand command = new SqlCommand(commandStr, connection);
                command.Parameters.Add(new SqlParameter("@clientId", client.Id));
                SqlDataAdapter oda = new SqlDataAdapter(command);
                tripsDataTable.Clear();
                oda.Fill(tripsDataTable);
                connection.Close();
            }
        }
    }
}
