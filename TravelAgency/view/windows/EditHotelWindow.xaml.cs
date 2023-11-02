using System;
using System.Collections.Generic;
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
using TravelAgency.DbAdapters;
using TravelAgency.model;
using TravelAgency.viewmodel;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для EditHotelWindow.xaml
    /// </summary>
    public partial class EditHotelWindow : Window
    {
        HotelViewModel hotelViewModel;
        ServicesListViewModel servicesListViewModel;
        public EditHotelWindow(Hotel hotel)
        {
            InitializeComponent();
            hotelViewModel = new HotelViewModel(hotel);
            servicesListViewModel = new ServicesListViewModel(HotelsAdapter.GetServicesByHotel(hotel));
            ServicesTextBox.DataContext = servicesListViewModel;
            hotelInfoGrid.DataContext = hotelViewModel;
        }

        private void changeHotel_Click(object sender, RoutedEventArgs e)
        {
            HotelsAdapter.UpdateHotel(hotelViewModel.CurrentHotel, servicesListViewModel.services);
            MessageBox.Show("Дані про готель успішно змінено");
            this.Close();
        }

        private void servicePicker_Click(object sender, RoutedEventArgs e)
        {
            ServicePickerWindow servicePickerWindow = new ServicePickerWindow();
            servicePickerWindow.ShowDialog();
            servicesListViewModel = new ServicesListViewModel(servicePickerWindow.SelectedServices);
            ServicesTextBox.DataContext = servicesListViewModel;
        }
    }
}
