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
using TravelAgency.viewmodel;
using TravelAgency.model;
using TravelAgency.DbAdapters;
using System.Data;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для ClientInfoWindow.xaml
    /// </summary>
    public partial class ClientInfoWindow : Window
    {
        ClientViewModel clientViewModel;
        DataTable clientTripsViewDataTable;
        public ClientInfoWindow(Client client)
        {
            InitializeComponent();
            clientViewModel = new ClientViewModel(client);
            clientInfoGrid.DataContext = clientViewModel;
            clientTripsViewDataTable = new DataTable();
            clientTripsInfo.ItemsSource = clientTripsViewDataTable.AsDataView();
            TripsAdapter.FillTripsByClient(client, clientTripsViewDataTable);
        }

        private void deleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Ви впевнені, що хочете видалити клієнта? Будуть видалені усі тури, оформлені на цього клієнта","Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    ClientsAdapter.DeleteClient(clientViewModel.CurrentClient);
                    MessageBox.Show("Клієнта успішно видалено.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
