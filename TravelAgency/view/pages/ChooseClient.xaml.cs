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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.model;
using TravelAgency.view.windows;
using TravelAgency.viewmodel;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для ChooseClient.xaml
    /// </summary>
    public partial class ChooseClient : Page
    {
        Trip FormedTrip;
        public ChooseClient()
        {
            InitializeComponent();
        }
        public ChooseClient(Trip formedTrip) : this()
        {
            FormedTrip = formedTrip;
            mainGrid.DataContext = new ClientViewModel(FormedTrip.Client);
        }

        private void clientExists_Click(object sender, RoutedEventArgs e)
        {
            ClientChooseWindow clientChoose = new ClientChooseWindow();
            clientChoose.ShowDialog();
            if (clientChoose.ChoosenClient != null) FormedTrip.Client = clientChoose.ChoosenClient;
        }
    }
}