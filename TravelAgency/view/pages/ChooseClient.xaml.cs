using System;
using System.Collections.Generic;
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
using TravelAgency.model;
using TravelAgency.view.windows;
using TravelAgency.viewmodel;
using TravelAgency.DbAdapters;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для ChooseClient.xaml
    /// </summary>
    public partial class ChooseClient : Page
    {
        Trip FormedTrip;
        string[] buttonStates = { "Клієнт існує", "Оформити нового клієнта" };
        public void ChangeTextBoxState(UIElementCollection controls)
        {
            foreach (var control in controls)
            {
                if(control is TextBox currentTextBox)
                {
                    currentTextBox.IsReadOnly = !currentTextBox.IsReadOnly;
                }
            }    
        }
        public void ClearTextBox(UIElementCollection controls)
        {
            foreach (var control in controls)
            {
                if (control is TextBox currentTextBox)
                {
                    currentTextBox.Text = string.Empty;
                }
            }
        }
        public ChooseClient()
        {
            InitializeComponent();
        }
        public ChooseClient(Trip formedTrip) : this()
        {
            FormedTrip = formedTrip;
            FormedTrip.Client = new Client();
            mainGrid.DataContext = new ClientViewModel(FormedTrip.Client);
        }

        private void clientExists_Click(object sender, RoutedEventArgs e)
        {
            if((string)clientExists.Content == buttonStates[0])
            {
                ClientChooseWindow clientChoose = new ClientChooseWindow();
                clientChoose.ShowDialog();
                if (clientChoose.ChoosenClient != null)
                {
                    FormedTrip.Client = clientChoose.ChoosenClient;
                    mainGrid.DataContext = new ClientViewModel(FormedTrip.Client);
                    ChangeTextBoxState(clientInfoGrid.Children);
                    clientExists.Content = buttonStates[1];
                }
            }
            else
            {
                FormedTrip.Client = new Client();
                mainGrid.DataContext = new ClientViewModel(FormedTrip.Client);
                ChangeTextBoxState(clientInfoGrid.Children);
                ClearTextBox(clientInfoGrid.Children);
                clientExists.Content = buttonStates[0];
            }
        }

        private void bookTour_Click(object sender, RoutedEventArgs e)
        {
            FormedTrip.RegistrationDate = DateTime.Now;
            if ((string)clientExists.Content == buttonStates[0])
            {
                FormedTrip.Client.Manager = (Manager)Application.Current.Properties["currentUser"];
                FormedTrip.Client.Id = ClientsAdapter.InsertClient(FormedTrip.Client);
            }
            TripsAdapter.InsertTrip(FormedTrip);
        }
    }
}