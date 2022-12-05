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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.DbAdapters;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для DataBaseAdmin.xaml
    /// </summary>
    public partial class DataBaseAdmin : Page
    {
        public DataBaseAdmin()
        {
            InitializeComponent();
        }

        private void doSqlButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                connection.Open();
                SqlDataAdapter oda = new SqlDataAdapter(querryTextBox.Text, connection);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                resultDataGrid.ItemsSource = dt.AsDataView();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: " + ex.Message);
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            querryTextBox.Text = "SELECT" ;
        }

        private void checkSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionService.CheckSubsctiptionAccepts();
        }
    }
}
