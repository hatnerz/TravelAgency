﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.model;

namespace TravelAgency.DbAdapters
{
    internal static class ClientsAdapter
    {
        public static int GetLastId()
        {
            string sqlExpression = "SELECT CONVERT(int, IDENT_CURRENT('clients'))";
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var a = command.ExecuteScalar();
                return (int)a;
            }
        }

        public static int InsertClient(Client client)
        {
            if(client != null)
            {
                try
                {
                    string sqlExpression =
              "INSERT INTO clients (first_name, last_name, patronymic_name, phone, email, passport, manager_id) " +
              "VALUES (@firstName, @lastName, @patronymicName, @phone, @email, @passport, @managerId)";
                    using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.Parameters.Add(new SqlParameter("@firstName", client.FirstName));
                        command.Parameters.Add(new SqlParameter("@lastName", client.LastName));
                        command.Parameters.Add(new SqlParameter("@patronymicName", client.PatronymicName));
                        command.Parameters.Add(new SqlParameter("@phone", client.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@email", client.Email));
                        command.Parameters.Add(new SqlParameter("@passport", client.Passport));
                        command.Parameters.Add(new SqlParameter("@managerId", client.Manager.Id));
                        command.ExecuteNonQuery();
                    }
                    return GetLastId();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return -1;
                }
            }
            return -1;
        }

        public static int InsertClientWithoutManager(Client client)
        {
            if (client != null)
            {
                try
                {
                    string sqlExpression =
              "INSERT INTO clients (first_name, last_name, patronymic_name, phone, email, passport) " +
              "VALUES (@firstName, @lastName, @patronymicName, @phone, @email, @passport)";
                    using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.Parameters.Add(new SqlParameter("@firstName", client.FirstName));
                        command.Parameters.Add(new SqlParameter("@lastName", client.LastName));
                        command.Parameters.Add(new SqlParameter("@patronymicName", client.PatronymicName));
                        command.Parameters.Add(new SqlParameter("@phone", client.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@email", client.Email));
                        command.Parameters.Add(new SqlParameter("@passport", client.Passport));
                        command.ExecuteNonQuery();
                        return GetLastId();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return -1;
                }
            }
            return -1;
        }
        
        public static void FillClientsByManager(Manager manager, DataTable clientsDataTable)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    string commandStr = "SELECT clients.*, CONCAT(managers.first_name,CONCAT(' ',managers.last_name)) manager_name FROM clients LEFT OUTER JOIN managers on clients.manager_id = managers.manager_id WHERE clients.manager_id=@managerId OR clients.manager_id is null;";
                    SqlCommand command = new SqlCommand(commandStr, connection);
                    command.Parameters.Add(new SqlParameter("@managerId", manager.Id));
                    SqlDataAdapter oda = new SqlDataAdapter(command);
                    clientsDataTable.Clear();
                    oda.Fill(clientsDataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void FillClientsWithTripsCount(DataTable clientsDataTable)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    string commandStr =
                        "SELECT clients.first_name as \"Прізвище\", clients.last_name as \"Ім'я\", clients.patronymic_name as \"По-батькові\", " +
                        "clients.phone as \"Телефон\", clients.email as \"Електронна пошта\", \r\nCOUNT(trips.trip_id)  as \"Оформлено турів\", " +
                        "CONCAT(managers.first_name,CONCAT(' ',managers.last_name))  as \"Менеджер\" \r\n" +
                        "FROM trips FULL OUTER JOIN clients on trips.client_id = clients.client_id LEFT OUTER JOIN managers on clients.manager_id = managers.manager_id \r\n" +
                        "GROUP BY trips.client_id, clients.client_id, clients.first_name, clients.last_name, clients.patronymic_name, clients.phone, clients.email,\r\n" +
                        "CONCAT(managers.first_name,CONCAT(' ',managers.last_name))\r\nORDER BY CONCAT(managers.first_name,CONCAT(' ',managers.last_name)) DESC";
                    SqlCommand command = new SqlCommand(commandStr, connection);
                    SqlDataAdapter oda = new SqlDataAdapter(command);
                    clientsDataTable.Clear();
                    oda.Fill(clientsDataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void DeleteClient(Client client)
        {
            try
            {
                SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                string commandStr = "DELETE FROM clients WHERE client_id = " + client.Id;
                connection.Open();
                SqlCommand command = new SqlCommand(commandStr, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void FillClientByManagerWithFilters(Manager manager, DataTable clientsDataTable, string firstName, string lastName, string patronymicName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    string commandStr = 
                        "SELECT clients.*, CONCAT(managers.first_name,CONCAT(' ',managers.last_name))  manager_name " +
                        "FROM clients LEFT OUTER JOIN managers on clients.manager_id = managers.manager_id " +
                        "WHERE (clients.manager_id=@managerId OR clients.manager_id is null)";
                    List<string> filters = new List<string>();
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    if (firstName != "" && firstName != null)
                    {
                        filters.Add("clients.first_name LIKE @firstName");
                        parameters.Add(new SqlParameter("@firstName", "%" + firstName + "%"));
                    }
                    if (lastName != "" && lastName != null)
                    {
                        filters.Add("clients.last_name LIKE @lastName");
                        parameters.Add(new SqlParameter("@lastName", "%" + lastName + "%"));
                    }
                    if (patronymicName != "" && patronymicName != null)
                    {
                        filters.Add("clients.patronymic_name LIKE @patronymicName");
                        parameters.Add(new SqlParameter("@patronymicName", "%" + patronymicName + "%"));
                    }

                    for (int i = 0; i < filters.Count; i++)
                    {
                        commandStr += " AND ";
                        commandStr += filters[i];
                    }
                    SqlCommand command = new SqlCommand(commandStr, connection);
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        command.Parameters.Add(parameters[i]);
                    }
                    command.Parameters.Add(new SqlParameter("@managerId", manager.Id));
                    SqlDataAdapter oda = new SqlDataAdapter(command);
                    clientsDataTable.Clear();
                    oda.Fill(clientsDataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static Client GetClient(int id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                string commandStr = "SELECT * FROM clients WHERE client_id=" + id.ToString();
                SqlCommand command = new SqlCommand(commandStr, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                Client client = new Client();
                client.Id = (int)reader["client_id"];
                client.FirstName = reader["first_name"].ToString();
                client.LastName = reader["last_name"].ToString();
                client.PatronymicName = reader["patronymic_name"].ToString();
                client.PhoneNumber = reader["phone"].ToString();
                client.Email = reader["email"].ToString();
                client.Passport = reader["passport"].ToString();
                if (reader["manager_id"].GetType() != typeof(DBNull)) 
                    client.Manager = ManagersAdapter.GetManager((int)reader["manager_id"]);
                connection.Close();
                return client;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        public static void UpdateClient(Client client)
        {
            try
            {
                string sqlExpression =
              "UPDATE clients SET first_name = @firstName, last_name = @lastName, patronymic_name = @patronymicName, phone = @phone, email = @email, passport = @passport, manager_id = @managerId " +
              "WHERE client_id = @clientId";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@firstName", client.FirstName));
                    command.Parameters.Add(new SqlParameter("@lastName", client.LastName));
                    command.Parameters.Add(new SqlParameter("@patronymicName", client.PatronymicName));
                    command.Parameters.Add(new SqlParameter("@phone", client.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@email", client.Email));
                    command.Parameters.Add(new SqlParameter("@passport", client.Passport));
                    command.Parameters.Add(new SqlParameter("@clientId", client.Id));
                    if(client.Manager == null) command.Parameters.Add(new SqlParameter("@managerId", DBNull.Value));
                    else command.Parameters.Add(new SqlParameter("@managerId", client.Manager.Id));
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
