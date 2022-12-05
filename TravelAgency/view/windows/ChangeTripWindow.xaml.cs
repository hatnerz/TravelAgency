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
using TravelAgency.view.pages;
using TravelAgency.model;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeTripWindow.xaml
    /// </summary>
    public partial class ChangeTripWindow : Window
    {
        public Trip EditedTrip;
        public ChangeTripWindow(Trip editedTrip)
        {
            InitializeComponent();
            EditedTrip = editedTrip;
            mainFrame.Navigate(new TripSettings(EditedTrip, true,this));
        }
    }
}
