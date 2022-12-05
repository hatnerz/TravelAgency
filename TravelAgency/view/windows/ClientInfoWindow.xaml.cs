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
using MaterialDesignThemes.Wpf;
using TravelAgency.view.pages;
using System.Data.SqlClient;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для ClientInfoWindow.xaml
    /// </summary>
    public partial class ClientInfoWindow : Window
    {
        public void ChangeTextBoxState(UIElementCollection controls)
        {
            foreach (var control in controls)
            {
                if (control is TextBox currentTextBox)
                {
                    currentTextBox.IsReadOnly = !currentTextBox.IsReadOnly;
                }
            }
        }


        ClientViewModel clientViewModel;
        DataTable clientTripsViewDataTable;
        DataTable subscriptionDataTable;
        public ClientInfoWindow(Client client)
        {
            InitializeComponent();
            clientViewModel = new ClientViewModel(client);
            clientInfoGrid.DataContext = clientViewModel;
            clientTripsViewDataTable = new DataTable();
            subscriptionDataTable = new DataTable();
            clientTripsInfo.ItemsSource = clientTripsViewDataTable.AsDataView();
            subscriptionsInfo.ItemsSource = subscriptionDataTable.AsDataView();
            TripsAdapter.FillTripsByClient(client, clientTripsViewDataTable);
            SubscriptionService.FillSubscriptionsByClient(client, subscriptionDataTable);
            if(clientViewModel.CurrentClient.Manager != null)
            {
                changeManagerButton.Tag = "deleteManager";
                changeManagerButton.Content = "Прибрати менеджера";
            }

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

        private void formTicket_Click(object sender, RoutedEventArgs e)
        {
            if (clientTripsInfo.SelectedIndex != -1)
            {
                DataRow selectedTripRow = ((DataRowView)clientTripsInfo.SelectedItem).Row;
                Trip selectedTrip = new Trip(selectedTripRow);
                selectedTrip.CreateTicket();
            }
            else
            {
                MessageBox.Show("Оберіть тур для формування квитку.");
            }
        }

        private void addSubscription_Click(object sender, RoutedEventArgs e)
        {
            AddSubscription addSubscriptionWindow = new AddSubscription(clientViewModel.CurrentClient);
            addSubscriptionWindow.ShowDialog();
            SubscriptionService.FillSubscriptionsByClient(clientViewModel.CurrentClient, subscriptionDataTable);
        }

        private void deleteSubscription_Click(object sender, RoutedEventArgs e)
        {
            if (subscriptionsInfo.SelectedIndex != -1)
            {
                if (MessageBox.Show("Ви точно хочете видалити обрану підписку?", "Підтвердження", MessageBoxButton.YesNo)
                    == MessageBoxResult.Yes)
                {
                    int selectedSubscriptionId = (int)((DataRowView)subscriptionsInfo.SelectedItem)["subscription_id"];
                    SubscriptionService.DeleteSubscription(selectedSubscriptionId);
                    SubscriptionService.FillSubscriptionsByClient(clientViewModel.CurrentClient,subscriptionDataTable);
                }
            }
            else
            {
                MessageBox.Show("Оберіть підписку для видалення");
            }
        }

        private void changeManagerButton_Click(object sender, object e)
        {
            if((string)changeManagerButton.Tag == "setManager")
            {
                clientViewModel.CurrentClient.Manager = (Manager)App.Current.Properties["currentUser"];
                MessageBox.Show("Ви успішно стали менеджером цього клієнта");
                changeManagerButton.Tag = "deleteManager";
                changeManagerButton.Content = "Прибрати менеджера";
            }
            else
            {
                clientViewModel.CurrentClient.Manager = null;
                MessageBox.Show("Ви успішно прибрали менеджера для цього клієнта");
                changeManagerButton.Tag = "setManager";
                changeManagerButton.Content = "Стати менеджером";
            }
            ClientsAdapter.UpdateClient(clientViewModel.CurrentClient);
        }

        private void changeInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if((string)changeInfoButton.Tag == "toChange")
            {
                changeInfoButton.Content = "Підтвердити зміни";
                ChangeTextBoxState(clientInfoGrid.Children);
                changeInfoButton.Tag = "toAccept";
            }
            else
            {
                changeInfoButton.Content = "Редагувати інформацію";
                ChangeTextBoxState(clientInfoGrid.Children);
                changeInfoButton.Tag = "toChange";
                ClientsAdapter.UpdateClient(clientViewModel.CurrentClient);
            }
        }

        private void changeTrip_Click(object sender, RoutedEventArgs e)
        {
            if (clientTripsInfo.SelectedIndex != -1)
            {
                DataRow selectedTripRow = ((DataRowView)clientTripsInfo.SelectedItem).Row;
                Trip selectedTrip = new Trip(selectedTripRow);
                ChangeTripWindow changeTripWindow = new ChangeTripWindow(selectedTrip);
                changeTripWindow.ShowDialog();
                TripsAdapter.FillTripsByClient(clientViewModel.CurrentClient, clientTripsViewDataTable);
            }
            else
            {
                MessageBox.Show("Оберіть тур для формування квитку.");
            }
            
        }
    }
}
