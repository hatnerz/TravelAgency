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
using TravelAgency.view.windows;
using System.Threading;
using System.Windows.Controls.Primitives;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для Tours.xaml
    /// </summary>
    public partial class Tours : Page
    {
        public Tours()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string command =
                "SELECT tours.departure_date, tours.arriving_date, tours.base_cost, tours.flight_cost, tours.food_cost, hotels.name " +
                "FROM tours LEFT OUTER JOIN hotels ON tours.hotel_id = hotels.hotel_id";
            connection.Open();
            SqlDataAdapter oda = new SqlDataAdapter(command, connection);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            toursDataGrid.ItemsSource = dt.AsDataView();
            connection.Close();
        }

        private void addTourButton_Click(object sender, RoutedEventArgs e)
        {
            AddTourWindow addTourWindow = new AddTourWindow();
            addTourWindow.ShowDialog();
        }
    }
}
