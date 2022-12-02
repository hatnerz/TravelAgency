using System;
using System.Collections.Generic;
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
using TravelAgency.DbAdapters;
using TravelAgency.model;
using TravelAgency.view.windows;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Page
    {
        DataTable clientsViewDataTable;
        public Clients()
        {
            InitializeComponent();
            clientsViewDataTable = new DataTable();
            ClientsAdapter.FillClientsByManager((Manager)Application.Current.Properties["currentUser"], clientsViewDataTable);
            clientsDataGrid.ItemsSource = clientsViewDataTable.AsDataView();
        }

        private void clientsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Client selectedClient = new Client(((DataRowView)clientsDataGrid.SelectedItem).Row);
            ClientInfoWindow clientInfoWindow = new ClientInfoWindow(selectedClient);
            clientInfoWindow.ShowDialog();
            ClientsAdapter.FillClientsByManager((Manager)Application.Current.Properties["currentUser"], clientsViewDataTable);
        }

        private void addClientButton_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWindow = new AddClientWindow();
            addClientWindow.ShowDialog();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsAdapter.FillClientByManagerWithFilters((Manager)App.Current.Properties["currentUser"], clientsViewDataTable,
                firstName.Text, lastName.Text, patronymicName.Text);
        }
    }
}
