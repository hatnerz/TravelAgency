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

namespace TravelAgency.DbAdapters
{
    internal static class ToursAdapter
    {
        static public int GetLastId()
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

        static public void FillClientsByTour(Tour tour, DataTable toursDataTable)
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
