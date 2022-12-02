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

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        ClientViewModel clientViewModel;
        public AddClientWindow()
        {
            InitializeComponent();
            clientViewModel = new ClientViewModel(new Client());
            clientInfoGrid.DataContext = clientViewModel;
        }

        private void addClient_Click(object sender, RoutedEventArgs e)
        {
            if(managerCheckBox.IsChecked == true)
            {
                clientViewModel.CurrentClient.Manager = (Manager)App.Current.Properties["currentUser"];
                ClientsAdapter.InsertClient(clientViewModel.CurrentClient);
            }
            else
            {
                ClientsAdapter.InsertClientWithoutManager(clientViewModel.CurrentClient);
            }
            MessageBox.Show("Клієнта успішно додано");
            this.Close();
        }
    }
}
