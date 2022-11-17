using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace TravelAgency
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TravelAgency;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string expression = string.Format("SELECT * FROM managers WHERE login='{0}' AND password='{1}'", login, password);
                SqlCommand command = new SqlCommand(expression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int id = (int)reader["manager_id"];
                    string login_ = reader["login"].ToString();
                    string password_ = reader["password"].ToString();
                    string firstName = reader["first_name"].ToString();
                    string lastName = reader["last_name"].ToString();
                    string patronymicName = reader["patronymic_name"].ToString();
                    bool admin = (bool)reader["admin"];
                    string officePhone = reader["office_phone"].ToString();
                    Manager currentUser = new Manager(id, login_, password_, firstName, lastName, patronymicName, admin, officePhone);
                    MainWindow mainWindow = new MainWindow(currentUser);
                    mainWindow.Show();
                    this.Owner = mainWindow;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Введено невірний логін чи пароль.");
                }
            }
        }
    }
}
