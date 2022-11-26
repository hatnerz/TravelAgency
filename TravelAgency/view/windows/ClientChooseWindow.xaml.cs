using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using TravelAgency.model;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для ClientChooseWindow.xaml
    /// </summary>
    public partial class ClientChooseWindow : Window
    {
        public Client ChoosenClient;
        public ClientChooseWindow()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            clientsDataGrid.ItemsSource = dt.AsEnumerable();
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string command = "SELECT * FROM clients";
            connection.Open();
            SqlDataAdapter oda = new SqlDataAdapter(command, connection);
            oda.Fill(dt);
            clientsDataGrid.ItemsSource = dt.AsDataView();
            connection.Close();
        }

        private void selectClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientsDataGrid.SelectedIndex >= 0)
            {
                DataRow selectedClient = ((DataRowView)clientsDataGrid.SelectedItem).Row;
                ChoosenClient = new Client(selectedClient);
                this.Close();
            }
            else
            {
                MessageBox.Show("Оберіть клієнта зі списку");
            }
        }
    }
}