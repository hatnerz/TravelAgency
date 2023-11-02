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
using TravelAgency.model;
using TravelAgency.DbAdapters;
using TravelAgency.viewmodel;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для ServicePickerWindow.xaml
    /// </summary>
    public partial class ServicePickerWindow : Window
    {
        List<Service> services;
        public List<Service> SelectedServices;
        ServicePickerViewModel servicePickerViewModel;
        public ServicePickerWindow()
        {
            InitializeComponent();
            SelectedServices = new List<Service>();
            services = HotelsAdapter.GetServices();
            servicePickerViewModel = new ServicePickerViewModel(services);
            ServicesListBox.ItemsSource = servicePickerViewModel.servicesWithStatus;
        }
        private void ConfirmServices_Click(object sender, RoutedEventArgs e)
        {

            foreach (var item in servicePickerViewModel.servicesWithStatus)
            {
                if(item.status == true)
                {
                    SelectedServices.Add(item.service);
                }
            }
            this.Close();
        }
    }
}
