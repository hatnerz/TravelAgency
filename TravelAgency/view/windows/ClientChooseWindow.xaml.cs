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
using System.Windows.Shapes;
using TravelAgency.model;
using TravelAgency.DbAdapters;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для ClientChooseWindow.xaml
    /// </summary>
    public partial class ClientChooseWindow : Window
    {
        public Client ChoosenClient;
        DataTable clientsViewDataTable;
        public ClientChooseWindow()
        {
            InitializeComponent();
            clientsViewDataTable = new DataTable();
            clientsDataGrid.ItemsSource = clientsViewDataTable.AsDataView();
            ClientsAdapter.FillClientsByManager((Manager)App.Current.Properties["currentUser"], clientsViewDataTable);
        }

        private void selectClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientsDataGrid.SelectedIndex >= 0)
            {
                DataRow selectedClient = ((DataRowView)clientsDataGrid.SelectedItem).Row;
                ChoosenClient = new Client(selectedClient);
                this.Close();
            }
            else
            {
                MessageBox.Show("Оберіть клієнта зі списку");
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsAdapter.FillClientByManagerWithFilters((Manager)App.Current.Properties["currentUser"], clientsViewDataTable,
                firstName.Text, lastName.Text, patronymicName.Text);
        }
    }
}