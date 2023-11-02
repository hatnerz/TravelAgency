using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TravelAgency.DbAdapters
{
    internal static class ManagersAdapter
    {
        public static void FillManagers(DataTable staffDataTable)
        {
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string command = "SELECT managers.manager_id,managers.login, managers.password, managers.first_name, managers.last_name, managers.patronymic_name, " +
            "managers.admin, managers.office_phone, COUNT(clients.client_id) clients_count " +
            "FROM managers LEFT OUTER JOIN clients on managers.manager_id = clients.manager_id GROUP BY managers.manager_id,managers.login, managers.password, managers.first_name, managers.last_name, managers.patronymic_name, " +
            "managers.admin, managers.office_phone";
            connection.Open();
            SqlDataAdapter oda = new SqlDataAdapter(command, connection);
            staffDataTable.Clear();
            oda.Fill(staffDataTable);
            connection.Close();
        }

        public static void FillManagersWithFilters(DataTable staffDataTable, string firstName, string lastName, string patronymicName)
        {
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                string commandStr =

                    "SELECT managers.manager_id,managers.login, managers.password, managers.first_name, managers.last_name, managers.patronymic_name, " +
                    "managers.admin, managers.office_phone, COUNT(clients.client_id) clients_count " +
                    "FROM managers LEFT OUTER JOIN clients on managers.manager_id = clients.manager_id "+
                    "WHERE ";
                List<string> filters = new List<string>();
                List<SqlParameter> parameters = new List<SqlParameter>();

                filters.Add("managers.first_name LIKE @firstName");
                parameters.Add(new SqlParameter("@firstName", "%" + firstName + "%"));
               
                filters.Add("managers.last_name LIKE @lastName");
                parameters.Add(new SqlParameter("@lastName", "%" + lastName + "%"));
                
                filters.Add("managers.patronymic_name LIKE @patronymicName");
                parameters.Add(new SqlParameter("@patronymicName", "%" + patronymicName + "%"));

                commandStr += filters[0];
                for (int i = 1; i < filters.Count; i++)
                {
                    commandStr += " AND ";
                    commandStr += filters[i];
                }
                commandStr += " GROUP BY managers.manager_id,managers.login, managers.password, managers.first_name, managers.last_name, managers.patronymic_name, " +
                    "managers.admin, managers.office_phone ";
                SqlCommand command = new SqlCommand(commandStr, connection);
                for (int i = 0; i < parameters.Count; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }
                
                SqlDataAdapter oda = new SqlDataAdapter(command);
                staffDataTable.Clear();
                oda.Fill(staffDataTable);
                connection.Close();
            }
        }
   
        public static Manager GetManager(int id)
        {
            try
            {
                Manager manager = new Manager();
                SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                string commandStr = "SELECT * FROM managers WHERE manager_id=" + id.ToString();
                SqlCommand command = new SqlCommand(commandStr, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                manager.Id = (int)reader["manager_id"];
                manager.login = reader["login"].ToString();
                manager.password = reader["password"].ToString();
                manager.FirstName = reader["first_name"].ToString();
                manager.LastName = reader["last_name"].ToString();
                manager.PatronymicName = reader["patronymic_name"].ToString();
                manager.OfficePhone = reader["office_phone"].ToString();
                manager.Admin = (bool)reader["admin"];
                connection.Close();
                return manager;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
    }
}
