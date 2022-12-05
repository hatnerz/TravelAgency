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
    /// Логика взаимодействия для AddSubscription.xaml
    /// </summary>
    public partial class AddSubscription : Window
    {
        TourSubscriptionViewModel tourSubscriptionViewModel;
        public AddSubscription(Client client)
        {
            InitializeComponent();
            tourSubscriptionViewModel = new TourSubscriptionViewModel(new TourSubscription(client));
            subscriptionInfoGrid.DataContext = tourSubscriptionViewModel;
        }

        private void addSubscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            tourSubscriptionViewModel.CurrentTourSubscription.RegistrationDate = DateTime.Now;
            SubscriptionService.InsertSubscription(tourSubscriptionViewModel.CurrentTourSubscription);
            SubscriptionService.SendSubscriptionToClient(tourSubscriptionViewModel.CurrentTourSubscription);
            MessageBox.Show("Підписка успішно оформлена");
            this.Close();
        }
    }
}
