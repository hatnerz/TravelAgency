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
using TravelAgency.model;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для Tours.xaml
    /// </summary>
    public partial class Tours : Page
    {
        SqlDataAdapter oda;
        DataTable toursViewDataTable;
        public Tours()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string command =
                "SELECT tours.tour_id, tours.departure_date, tours.arriving_date, tours.base_cost, tours.flight_cost, tours.food_cost, hotels.name, hotels.country, hotels.city, hotels.hotel_id " +
                "FROM tours LEFT OUTER JOIN hotels ON tours.hotel_id = hotels.hotel_id";
            connection.Open();
            oda = new SqlDataAdapter(command, connection);
            toursViewDataTable = new DataTable();
            oda.Fill(toursViewDataTable);
            toursDataGrid.ItemsSource = toursViewDataTable.AsDataView();
            connection.Close();
        }

        private void addTourButton_Click(object sender, RoutedEventArgs e)
        {
            AddTourWindow addTourWindow = new AddTourWindow();
            addTourWindow.ShowDialog();
            toursViewDataTable.Clear();
            oda.Fill(toursViewDataTable);
        }

        private void editTourButton_Click(object sender, RoutedEventArgs e)
        {
            if(toursDataGrid.SelectedIndex!=-1)
            {
                int selectedTourId = (int)((DataRowView)toursDataGrid.SelectedItem)["tour_id"];
                Tour editableTour = new Tour(selectedTourId);
                EditTourWindow editTourWindow = new EditTourWindow(editableTour);
                editTourWindow.ShowDialog();
                toursViewDataTable.Clear();
                oda.Fill(toursViewDataTable);
            }
            else
            {
                MessageBox.Show("Оберіть тур для редагування");
            }
        }

        private void deleteTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (toursDataGrid.SelectedIndex != -1)
            {
                if(MessageBox.Show("Ви точно хочете видалити обраний тур?", "Підтвердження", MessageBoxButton.YesNo) 
                    == MessageBoxResult.Yes)
                {
                    int selectedTourId = (int)((DataRowView)toursDataGrid.SelectedItem)["tour_id"];
                    SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
                    string commandStr = "DELETE FROM tours WHERE tour_id = " + selectedTourId;
                    connection.Open();
                    SqlCommand command = new SqlCommand(commandStr, connection);
                    command.ExecuteNonQuery();
                    toursViewDataTable.Clear();
                    oda.Fill(toursViewDataTable);
                }
            }
            else
            {
                MessageBox.Show("Оберіть тур для видалення");
            }
        }

        private void toursDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if(toursDataGrid.CurrentCell.Column != null)
            {
                int selectedColumn = toursDataGrid.CurrentCell.Column.DisplayIndex;
                var dataRowView = (DataRowView)toursDataGrid.CurrentCell.Item;
                string propertyName = ((Binding)toursDataGrid.Columns[selectedColumn].ClipboardContentBinding).Path.Path;
                string element = dataRowView[propertyName].ToString();
                currentCell.Text = element;
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string command =
                "SELECT tours.tour_id, tours.departure_date, tours.arriving_date, tours.base_cost, tours.flight_cost, tours.food_cost, hotels.name, hotels.country, hotels.city, hotels.hotel_id " +
                "FROM tours LEFT OUTER JOIN hotels ON tours.hotel_id = hotels.hotel_id ";
            List<string> filters = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();


            if(countryTextBox.Text.Length > 0)
            {
                filters.Add("country=@country");
                parameters.Add(new SqlParameter("@country", countryTextBox.Text));
            }
            if (cityTextBox.Text.Length > 0)
            {
                filters.Add("city=@city");
                parameters.Add(new SqlParameter("@city", cityTextBox.Text));
            }
            if (hotelTextBox.Text.Length > 0)
            {
                filters.Add("name=@name");
                parameters.Add(new SqlParameter("@name", hotelTextBox.Text));
            }


            if (priceMinTextBox.Text.Length > 0)
            {
                filters.Add("base_cost + flight_cost >= @min_price");
                parameters.Add(new SqlParameter("@min_price", priceMinTextBox.Text));
            }
            if (priceMaxTextBox.Text.Length > 0)
            {
                filters.Add("base_cost + flight_cost <= @max_price");
                parameters.Add(new SqlParameter("@max_price", priceMaxTextBox.Text));
            }


            if (departureMinTextBox.Text.Length > 0)
            {
                filters.Add("departure_date >= @min_departure");
                parameters.Add(new SqlParameter("@min_departure", departureMinTextBox.Text));
            }
            if (departureMaxTextBox.Text.Length > 0)
            {
                filters.Add("departure_date <= @max_departure");
                parameters.Add(new SqlParameter("@max_departure", departureMaxTextBox.Text));
            }


            if (arrivingMinTextBox.Text.Length > 0)
            {
                filters.Add("arriving_date >= @min_arrivinge");
                parameters.Add(new SqlParameter("@min_arriving", arrivingMinTextBox.Text));
            }
            if (arrivingMaxTextBox.Text.Length > 0)
            {
                filters.Add("arriving_date <= @max_arriving");
                parameters.Add(new SqlParameter("@max_arriving", arrivingMaxTextBox.Text));
            }
            SqlCommand sqlCommand = new SqlCommand(command, connection);
            if (filters.Count > 0)
            {
                command += "WHERE ";
                command += filters[0];

                for (int i = 1; i < filters.Count; i++)
                {
                    command += " AND ";
                    command += filters[i];
                }
                sqlCommand = new SqlCommand(command, connection);
                for (int i = 0; i < parameters.Count; i++)
                {
                    sqlCommand.Parameters.Add(parameters[i]);
                }
            }
            connection.Open();
            oda = new SqlDataAdapter(sqlCommand);
            toursViewDataTable.Clear();
            oda.Fill(toursViewDataTable);
            connection.Close();
        }

        private void toursDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRow selectedTour = ((DataRowView)toursDataGrid.SelectedItem).Row;
            TourInfo tourInfoWindow = new TourInfo(new Tour(selectedTour));
            tourInfoWindow.ShowDialog();
        }
    }
}
