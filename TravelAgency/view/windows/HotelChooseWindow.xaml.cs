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
    /// Логика взаимодействия для HotelChooseWindow.xaml
    /// </summary>
    public partial class HotelChooseWindow : Window
    {
        public Hotel ChoosenHotel;
        public HotelChooseWindow()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            hotelDataGrid.ItemsSource = dt.AsEnumerable();
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string command = "SELECT * FROM hotels";
            connection.Open();
            SqlDataAdapter oda = new SqlDataAdapter(command, connection);
            oda.Fill(dt);
            hotelDataGrid.ItemsSource = dt.AsDataView();
            connection.Close();
        }

        private void addHotelButton_Click(object sender, object e)
        {
            if(hotelDataGrid.SelectedIndex >=0)
            {
                DataRowView selectedHotel = (DataRowView)hotelDataGrid.SelectedItem;
                ChoosenHotel = new Hotel(selectedHotel);
                this.Close();
            }
            else
            {
                MessageBox.Show("Оберіть готель зі списку");
            }
        }
    }
}
