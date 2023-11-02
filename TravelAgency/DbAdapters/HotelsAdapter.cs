using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using TravelAgency.model;
using TravelAgency.view.pages;

namespace TravelAgency.DbAdapters
{
    internal static class HotelsAdapter
    {
        public static int GetLastId()
        {
            string sqlExpression = "SELECT CONVERT(int, IDENT_CURRENT('hotels'))";
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var a = command.ExecuteScalar();
                return (int)a;
            }
        }

        public static Hotel GetHotel(int id)
        {
            try
            {
                Hotel hotel = new Hotel();
                SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                string commandStr = "SELECT * FROM hotels WHERE hotel_id=" + id.ToString();
                SqlCommand command = new SqlCommand(commandStr, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                hotel.id = (int)reader["hotel_id"];
                hotel.Country = reader["country"].ToString();
                hotel.City = reader["city"].ToString();
                hotel.Stars = (byte)reader["stars"];
                hotel.Description = reader["description"].ToString();
                hotel.RoomNumber = (short)reader["room_number"];
                hotel.Name = reader["name"].ToString();
                connection.Close();
                return hotel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        public static void FillTopHotels(DataTable hotelsDataTable)
        {
            try
            {
                string sqlExpression =
                "SELECT hotels.name as Назва, hotels.city as Місто, hotels.country as Країна, AVG(responces.score) as \"Середня оцінка\", COUNT(responces.responce_id) as \"Кількість відгуків\"" +
                "FROM hotels LEFT OUTER JOIN responces on hotels.hotel_id = responces.hotel_id " +
                "GROUP BY hotels.name, hotels.city, hotels.country " +
                "ORDER BY AVG(responces.score) DESC";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataAdapter oda = new SqlDataAdapter(command);
                    hotelsDataTable.Clear();
                    oda.Fill(hotelsDataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public static void FillHotelsWithFilters(DataTable hotelsDataTable, string country, string city, string name, int minRooms, int maxRooms, int stars)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    string commandStr =
                        "SELECT * FROM hotels " +
                        "WHERE ";
                    List<string> filters = new List<string>();
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    filters.Add("country LIKE @country");
                    parameters.Add(new SqlParameter("@country", "%" + country + "%"));

                    filters.Add("city LIKE @city");
                    parameters.Add(new SqlParameter("@city", "%" + city + "%"));

                    filters.Add("name LIKE @name");
                    parameters.Add(new SqlParameter("@name", "%" + name + "%"));

                    if (minRooms != 0)
                    {
                        filters.Add("room_number >= @minRooms");
                        parameters.Add(new SqlParameter("@minRooms", minRooms));
                    }

                    if (maxRooms != 0)
                    {
                        filters.Add("room_number <= @maxRooms");
                        parameters.Add(new SqlParameter("@maxRooms", maxRooms));
                    }

                    if (stars != 0)
                    {
                        filters.Add("stars = @stars");
                        parameters.Add(new SqlParameter("@stars", stars));
                    }

                    commandStr += filters[0];
                    for (int i = 1; i < filters.Count; i++)
                    {
                        commandStr += " AND ";
                        commandStr += filters[i];
                    }

                    SqlCommand command = new SqlCommand(commandStr, connection);
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        command.Parameters.Add(parameters[i]);
                    }

                    SqlDataAdapter oda = new SqlDataAdapter(command);
                    hotelsDataTable.Clear();
                    oda.Fill(hotelsDataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public static void FillTopServices(DataTable servicesDataTable)
        {
            try
            {
                string sqlExpression =
                "SELECT service.name as 'Назва сервісу', COUNT(DISTINCT hotel_id) as 'Кількість готелів' " +
                "FROM service LEFT OUTER JOIN hotel_service ON hotel_service.service_id = service.service_id " +
                "GROUP BY service.service_id, service.name";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataAdapter oda = new SqlDataAdapter(command);
                    servicesDataTable.Clear();
                    oda.Fill(servicesDataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public static void FillHotels(DataTable hotelsDataTable)
        {
            try
            {
                string sqlExpression =
                "SELECT * FROM hotels";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataAdapter oda = new SqlDataAdapter(command);
                    hotelsDataTable.Clear();
                    oda.Fill(hotelsDataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static int InsertHotel(Hotel hotel, List<Service> services)
        {
            int hotelId = -1;
            if (hotel != null)
            {
                try
                {
                    string sqlExpression =
              "INSERT INTO hotels (name, country, city, stars, description, room_number) " +
              "VALUES (@name, @country, @city, @stars, @description, @roomNumber)";
                    using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.Parameters.Add(new SqlParameter("@name", hotel.Name));
                        command.Parameters.Add(new SqlParameter("@country", hotel.Country));
                        command.Parameters.Add(new SqlParameter("@city", hotel.City));
                        command.Parameters.Add(new SqlParameter("@stars", hotel.Stars));
                        command.Parameters.Add(new SqlParameter("@description", hotel.Description));
                        command.Parameters.Add(new SqlParameter("@roomNumber", hotel.RoomNumber));
                        command.ExecuteNonQuery();
                        hotelId = GetLastId();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return -1;
                }
            }
            if (InsertServicesByHotel(hotelId, services) == -1)
                return -1;
            return hotelId;
        }

        public static List<Service> GetServices()
        {
            try
            {
                List<Service> services = new List<Service>();
                string sqlExpression =
                    "SELECT * FROM service";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["service_id"];
                            string name = reader["name"].ToString();
                            services.Add(new Service(id, name));
                        }
                    }
                    connection.Close();
                }
                return services;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
            
        }

        public static List<Service> GetServicesByHotel(Hotel hotel)
        {
            try
            {
                List<Service> services = new List<Service>();
                string sqlExpression =
                    "SELECT DISTINCT service.service_id, service.name " +
                    "FROM service INNER JOIN hotel_service ON service.service_id = hotel_service.service_id " +
                    "WHERE hotel_service.hotel_id = @hotelId";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@hotelId", hotel.id));
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["service_id"];
                            string name = reader["name"].ToString();
                            services.Add(new Service(id, name));
                        }
                    }
                    connection.Close();
                }
                return services;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
            
        }

        public static void UpdateHotel(Hotel hotel, List<Service> services)
        {
            try
            {
                string sqlExpression =
              "UPDATE hotels SET name = @name, country = @country, city = @city, stars = @stars, description = @description, room_number = @roomNumber " +
              "WHERE hotel_id = @hotelId";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@name", hotel.Name));
                    command.Parameters.Add(new SqlParameter("@country", hotel.Country));
                    command.Parameters.Add(new SqlParameter("@city", hotel.City));
                    command.Parameters.Add(new SqlParameter("@stars", hotel.Stars));
                    command.Parameters.Add(new SqlParameter("@description", hotel.Description));
                    command.Parameters.Add(new SqlParameter("@roomNumber", hotel.RoomNumber));
                    command.Parameters.Add(new SqlParameter("@hotelId", hotel.id));
                    command.ExecuteNonQuery();
                }
                DeleteServiceByHotel(hotel);
                InsertServicesByHotel(hotel.id, services);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void DeleteServiceByHotel(Hotel hotel)
        {
            try
            {
                SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                string commandStr = "DELETE FROM hotel_service WHERE hotel_id = " + hotel.id;
                connection.Open();
                SqlCommand command = new SqlCommand(commandStr, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public static int InsertServicesByHotel(int hotelId, List<Service> services)
        {
            if (services != null && services.Count > 0 && hotelId != -1)
            {
                try
                {
                    string sqlExpression =
              "INSERT INTO hotel_service (service_id, hotel_id) " +
              "VALUES (@serviceId, @hotelId)";
                    using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                    {
                        connection.Open();
                        foreach (Service service in services)
                        {
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            command.Parameters.Add(new SqlParameter("@serviceId", service.Id));
                            command.Parameters.Add(new SqlParameter("@hotelId", hotelId));
                            command.ExecuteNonQuery();
                        }

                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return -1;
                }
            }
            return 1;
        }

        public static void DeleteHotel(int hotelId)
        {
            try
            {
                SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                connection.Open();
                string commandStr = "DELETE FROM hotel_service WHERE hotel_id = " + hotelId;
                SqlCommand command = new SqlCommand(commandStr, connection);
                command.ExecuteNonQuery();
                commandStr = "DELETE FROM hotels WHERE hotel_id = " + hotelId;
                command = new SqlCommand(commandStr, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static List<string> GetCountries()
        {
            try
            {
                List<string> countries = new List<string>();
                string sqlExpression =
                    "SELECT DISTINCT country FROM hotels";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string name = reader["country"].ToString();
                            countries.Add(name);
                        }
                    }
                    connection.Close();
                }

                return countries;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
    }
}
