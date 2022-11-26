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
    /// Логика взаимодействия для EditTourWindow.xaml
    /// </summary>
    public partial class EditTourWindow : Window
    {
        Tour EditableTour;
        public EditTourWindow(Tour editableTour)
        {
            this.EditableTour = editableTour;
            InitializeComponent();
            departureDateTextBox.Text = EditableTour.DepartureDate.ToString();
            arrivingDateTextBox.Text = EditableTour.ArrivingDate.ToString();
            baseCostTextBox.Text = EditableTour.BaseCost.ToString();
            foodCostTextBox.Text = EditableTour.FoodCost.ToString();
            flightCostTextBox.Text = EditableTour.FlightCost.ToString();
            hotelPicker.Content = editableTour.Hotel.Name;
        }

        private void hotelPicker_Click(object sender, RoutedEventArgs e)
        {
            HotelChooseWindow hotelChooseWindow = new HotelChooseWindow();
            hotelChooseWindow.ShowDialog();
            if (hotelChooseWindow.ChoosenHotel != null)
            {
                EditableTour.Hotel = hotelChooseWindow.ChoosenHotel;
                hotelPicker.Content = EditableTour.Hotel.Name;
            }
        }

        private void changeTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (departureDateTextBox.Text == "" ||
                arrivingDateTextBox.Text == "" ||
                baseCostTextBox.Text == "" ||
                flightCostTextBox.Text == "" ||
                foodCostTextBox.Text == "" ||
                EditableTour.Hotel == null)
            {
                MessageBox.Show("Не всі поля заповнені");
            }
            else
            {
                try
                {
                   string sqlExpression =
                   "UPDATE tours SET departure_date = @departureDate, arriving_date = @arrivingDate, base_cost = @baseCost, " +
                   "flight_cost = @flightCost, food_cost = @foodCost, hotel_id = @hotelId WHERE tour_id = @tour_id";
                    using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                    {
                        connection.Open();
                        DateTime departureDate = DateTime.Parse(departureDateTextBox.Text);
                        DateTime arrivingDate = DateTime.Parse(arrivingDateTextBox.Text);
                        decimal baseCost = Convert.ToDecimal(baseCostTextBox.Text);
                        decimal flightCost = Convert.ToDecimal(flightCostTextBox.Text);
                        decimal foodCost = Convert.ToDecimal(foodCostTextBox.Text);
                        int hotelId = EditableTour.Hotel.id;
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.Parameters.Add(new SqlParameter("@departureDate", departureDate));
                        command.Parameters.Add(new SqlParameter("@arrivingdate", arrivingDate));
                        command.Parameters.Add(new SqlParameter("@baseCost", baseCost));
                        command.Parameters.Add(new SqlParameter("@flightCost", flightCost));
                        command.Parameters.Add(new SqlParameter("@foodCost", foodCost));
                        command.Parameters.Add(new SqlParameter("@hotelId", hotelId));
                        command.Parameters.Add(new SqlParameter("@tour_id", EditableTour.Id));
                        command.ExecuteNonQuery();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
