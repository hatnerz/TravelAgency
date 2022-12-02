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
using System.Xml.Linq;
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
            if(hotelChooseWindow.ChoosenHotel != null)
            {
                tourHotel = hotelChooseWindow.ChoosenHotel;
                hotelPicker.Content = tourHotel.Name;
            }
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
                try
                {
                    string sqlExpression =
                   "INSERT INTO tours (departure_date, arriving_date, base_cost, flight_cost, food_cost, hotel_id) " +
                   "VALUES (@departureDate, @arrivingDate, @baseCost, @flightCost, @foodCost, @hotelId)";
                    using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                    {
                        connection.Open();
                        DateTime departureDate = DateTime.Parse(departureDateTextBox.Text);
                        DateTime arrivingDate = DateTime.Parse(arrivingDateTextBox.Text);
                        decimal baseCost = Convert.ToDecimal(baseCostTextBox.Text);
                        decimal flightCost = Convert.ToDecimal(flightCostTextBox.Text);
                        decimal foodCost = Convert.ToDecimal(foodCostTextBox.Text);
                        int hotelId = tourHotel.id;
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.Parameters.Add(new SqlParameter("@departureDate", departureDate));
                        command.Parameters.Add(new SqlParameter("@arrivingdate", arrivingDate));
                        command.Parameters.Add(new SqlParameter("@baseCost", baseCost));
                        command.Parameters.Add(new SqlParameter("@flightCost", flightCost));
                        command.Parameters.Add(new SqlParameter("@foodCost", foodCost));
                        command.Parameters.Add(new SqlParameter("@hotelId", hotelId));
                        command.ExecuteNonQuery();
                        this.Close();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
