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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.DbAdapters;
using TravelAgency.model;
using TravelAgency.view.windows;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для Hotels.xaml
    /// </summary>
    public partial class Hotels : Page
    {
        DataTable hotelsDataTable;
        public Hotels()
        {
            InitializeComponent();
            hotelsDataTable = new DataTable();
            HotelsAdapter.FillHotels(hotelsDataTable);
            hotelsDataGrid.ItemsSource = hotelsDataTable.AsDataView();
            countryTextBox.ItemsSource = HotelsAdapter.GetCountries();
            starsTextBox.ItemsSource = new List<string> {"", "1", "2", "3", "4", "5" };
        }

        private void addHotelButton_Click(object sender, RoutedEventArgs e)
        {
            AddHotelWindow addHotelWindow = new AddHotelWindow();
            addHotelWindow.ShowDialog();
            HotelsAdapter.FillHotels(hotelsDataTable);
        }

        private void editHotelButton_Click(object sender, RoutedEventArgs e)
        {
            if (hotelsDataGrid.SelectedIndex != -1)
            {
                int selectedHotelId = (int)((DataRowView)hotelsDataGrid.SelectedItem)["hotel_id"];
                Hotel editableHotel = HotelsAdapter.GetHotel(selectedHotelId);
                EditHotelWindow editTourWindow = new EditHotelWindow(editableHotel);
                editTourWindow.ShowDialog();
                HotelsAdapter.FillHotels(hotelsDataTable);
            }
            else
            {
                MessageBox.Show("Оберіть готель для редагування");
            }
        }

        private void deleteHotelButton_Click(object sender, RoutedEventArgs e)
        {
            if (hotelsDataGrid.SelectedIndex != -1)
            {
                if (MessageBox.Show("Ви точно хочете видалити обраний готель?", "Підтвердження", MessageBoxButton.YesNo)
                    == MessageBoxResult.Yes)
                {
                    int selectedHotelId = (int)((DataRowView)hotelsDataGrid.SelectedItem)["hotel_id"];
                    HotelsAdapter.DeleteHotel(selectedHotelId);
                    HotelsAdapter.FillHotels(hotelsDataTable);
                }
            }
            else
            {
                MessageBox.Show("Оберіть тур для видалення");
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            int minRooms, maxRooms, stars;
            int.TryParse(roomsMinTextBox.Text, out minRooms);
            int.TryParse(roomsMaxTextBox.Text, out maxRooms);
            int.TryParse(starsTextBox.Text, out stars);
            HotelsAdapter.FillHotelsWithFilters(hotelsDataTable, countryTextBox.Text, cityTextBox.Text, nameTextBox.Text,
                minRooms, maxRooms, stars);
        }
    }
}
