using System;
using System.Collections.Generic;
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
using TravelAgency.model;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для AddTourWindow.xaml
    /// </summary>
    public partial class AddTourWindow : Window
    {
        public Hotel tourHotel = null;
        public AddTourWindow()
        {
            InitializeComponent();
        }

        private void hotelPicker_Click(object sender, object e)
        {
            HotelChooseWindow hotelChooseWindow = new HotelChooseWindow();
            hotelChooseWindow.ShowDialog();
            tourHotel = hotelChooseWindow.ChoosenHotel;
            hotelPicker.Content = tourHotel.Name;
        }

        private void addTourButton_Click(object sender, RoutedEventArgs e)
        {
            if(departureDateTextBox.Text == "" ||
                arrivingDateTextBox.Text == "" ||
                baseCostTextBox.Text == "" ||
                flightCostTextBox.Text == "" ||
                foodCostTextBox.Text == "" ||
                tourHotel == null)
            {
                MessageBox.Show("Не всі поля заповнені");
            }
            else
            {
                //SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                //string command = "INSERT * FROM hotels";
                //connection.Open();
                //SqlDataAdapter oda = new SqlDataAdapter(command, connection);
            }
        }
    }
}
