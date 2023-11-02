using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace TravelAgency
{
    public class Manager : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string OfficePhone { get; set; }
        public bool Admin { get; set; }

        private string firstName;
        private string lastName;
        private string patronymicName;

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged("GetFullName");
            }
        }
        public string LastName
        {
            get => lastName;
            set 
            {
                lastName = value; 
                OnPropertyChanged("GetFullName");
            }
        }
        public string PatronymicName 
        { 
            get => patronymicName;
            set
            {
                patronymicName = value; 
                OnPropertyChanged("GetFullName");
            }
        }

        public string GetFullName
        {
            get
            {
                return FirstName + " " + LastName + " " + PatronymicName;
            }
        }

        public Manager(int id, string login, string password, string firstName, string lastName, string patronymicName, bool admin, string officePhone)
        {
            this.Id = id;
            this.login = login;
            this.password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PatronymicName = patronymicName;
            this.Admin = admin;
            this.OfficePhone = officePhone;
        }

        public Manager(DataRowView selectedManager)
        {
            this.Id = (int)selectedManager["manager_id"];
            login = selectedManager["login"].ToString();
            password = selectedManager["password"].ToString();
            FirstName = selectedManager["first_name"].ToString();
            LastName = selectedManager["last_name"].ToString();
            PatronymicName = selectedManager["patronymic_name"].ToString();
            OfficePhone = selectedManager["office_phone"].ToString();
            Admin = (bool)selectedManager["admin"];
        }

        public Manager() { }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs p = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, p);
            }
        }

        public void UpdateTable()
        {
            try
            {
                string sqlExpression =
               "UPDATE managers " +
               "SET login = @login_, password = @password, first_name = @first_name, last_name = @last_name," +
               "patronymic_name = @patronymic_name, admin = @admin, office_phone = @office_phone " +
               "WHERE manager_id = @manager_id";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@login_", login));
                    command.Parameters.Add(new SqlParameter("@password", password));
                    command.Parameters.Add(new SqlParameter("@first_name", FirstName));
                    command.Parameters.Add(new SqlParameter("@last_name", LastName));
                    command.Parameters.Add(new SqlParameter("@patronymic_name", PatronymicName));
                    command.Parameters.Add(new SqlParameter("@admin", Admin));
                    command.Parameters.Add(new SqlParameter("@office_phone", OfficePhone));
                    command.Parameters.Add(new SqlParameter("@manager_id", Id));
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
