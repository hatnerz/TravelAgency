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
using TravelAgency.view.pages;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для TourBooking.xaml
    /// </summary>
    public partial class TourBooking : Window
    {
        public Client SelectedClient;
        public Trip BookedTrip;
        public Tour TESTTOUR;
        public TripSettings CurrentTripSettings;
        public ChooseClient CurrentClientChoose;
        public TourBooking()
        {
            InitializeComponent();
        }
        public TourBooking(Tour tour) : this()
        {
            TESTTOUR = tour;
            BookedTrip = new Trip(0, DateTime.Now, "AI", "B", tour, null);
            CurrentClientChoose = new ChooseClient(BookedTrip);
            CurrentTripSettings = new TripSettings(BookedTrip, tourBookingFrame, CurrentClientChoose);
            tourBookingFrame.Navigate(CurrentTripSettings);
        }
    }
}
