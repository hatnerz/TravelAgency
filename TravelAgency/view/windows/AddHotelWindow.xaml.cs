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
using TravelAgency.viewmodel;
using TravelAgency.model;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для AddHotelWindow.xaml
    /// </summary>
    public partial class AddHotelWindow : Window
    {
        HotelViewModel hotelViewModel;
        ServicesListViewModel servicesListViewModel;
        public AddHotelWindow()
        {
            InitializeComponent();
            hotelViewModel = new HotelViewModel(new Hotel());
            hotelInfoGrid.DataContext = hotelViewModel;
        }

        private void addHotel_Click(object sender, RoutedEventArgs e)
        {
            int id = HotelsAdapter.InsertHotel(hotelViewModel.CurrentHotel, servicesListViewModel.services);
            if(id!=-1)
                MessageBox.Show("Готель успішно додано");
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
