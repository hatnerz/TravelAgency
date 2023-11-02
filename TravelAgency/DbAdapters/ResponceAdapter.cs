using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.model;
using TravelAgency.view.pages;

namespace TravelAgency.DbAdapters
{
    internal static class ResponceAdapter
    {
        public static void InsertResponce(Responce responce)
        {
            try
            {
                string sqlExpression =
               "INSERT INTO responces (score, comment, hotel_id, trip_id) " +
               "VALUES (@score, @comment, @hotelId, @tripId)";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@score", responce.Score));
                    command.Parameters.Add(new SqlParameter("@comment", responce.Comment));
                    command.Parameters.Add(new SqlParameter("@hotelId", responce.Hotel.id));
                    command.Parameters.Add(new SqlParameter("@tripId", responce.Trip.Id));
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
