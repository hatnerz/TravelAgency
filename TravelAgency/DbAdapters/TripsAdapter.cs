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
using TravelAgency.view.pages;

namespace TravelAgency.DbAdapters
{
    internal static class TripsAdapter
    {
        public static void InsertTrip(Trip trip)
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

        public static void InsertTripWithoutSettings(Trip trip)
        {
            try
            {
                string sqlExpression =
               "INSERT INTO trips (registration_date, tour_id, client_id) " +
               "VALUES (@registrationDate, @tourId, @clientId)";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@registrationDate", trip.RegistrationDate));
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

        public static void FillTripsByClient(Client client, DataTable tripsDataTable)
        {
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                string commandStr =
                    "SELECT trips.registration_date, trips.trip_id, tours.tour_id, trips.client_id, hotels.country, hotels.city, hotels.name, tours.departure_date, tours.arriving_date, trips.food_type, trips.plane_class " +
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

        public static void UpdateTrip(Trip trip)
        {
            try
            {
                string sqlExpression =
              "UPDATE trips SET registration_date = @registrationDate, food_type = @foodType, plane_class = @planeClass, tour_id = @tourId, client_id = @clientId " +
              "WHERE trip_id = @tripId";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@registrationDate", trip.RegistrationDate));

                    if (trip.FoodType == null) command.Parameters.Add(new SqlParameter("@foodType", DBNull.Value));
                    else command.Parameters.Add(new SqlParameter("@foodType", trip.FoodType));

                    if (trip.PlaneClass == null) command.Parameters.Add(new SqlParameter("@planeClass", DBNull.Value));
                    else command.Parameters.Add(new SqlParameter("@planeClass", trip.PlaneClass));

                    command.Parameters.Add(new SqlParameter("@clientId", trip.Client.Id));
                    command.Parameters.Add(new SqlParameter("@tourId", trip.Tour.Id));

                    command.Parameters.Add(new SqlParameter("@tripId", trip.Id));
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
