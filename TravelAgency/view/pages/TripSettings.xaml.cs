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
using TravelAgency.DbAdapters;
using TravelAgency.model;
using TravelAgency.view.windows;
using TravelAgency.viewmodel;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для TripSettings.xaml
    /// </summary>
    public partial class TripSettings : Page
    {
        bool change = false;
        public Trip FormedTrip;
        public Frame TourBookingFrame;
        public ChooseClient CurrentClientChoose;
        public ChangeTripWindow CurrentClientChangeTripWindow;
        TripSettingsViewModel tripSettingsViewModel;
        public TripSettings()
        {
            InitializeComponent();
        }
        public TripSettings(Trip formedTrip) : this()
        {
            FormedTrip = formedTrip;
            tripSettingsViewModel = new TripSettingsViewModel(formedTrip);
            mainGrid.DataContext = tripSettingsViewModel;
            planeClassComboBox.ItemsSource = tripSettingsViewModel.PlaneClassesFull.Values;
            foodTypeComboBox.ItemsSource = tripSettingsViewModel.FoodTypeFull.Values;
        }
        public TripSettings(Trip formedTrip, Frame tourBookingFrame, ChooseClient currentClientChoose) : this(formedTrip)
        {
            TourBookingFrame = tourBookingFrame;
            CurrentClientChoose = currentClientChoose;
        }
        public TripSettings(Trip formedTrip, bool change) : this(formedTrip)
        {
            this.change = change;
        }
        public TripSettings(Trip formedTrip, bool change, ChangeTripWindow changeTripWindow) : this(formedTrip,change)
        {
            CurrentClientChangeTripWindow = changeTripWindow;
        }

        private void setTripSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!change)
                TourBookingFrame.Navigate(CurrentClientChoose);
            else
            {
                TripsAdapter.UpdateTrip(tripSettingsViewModel.FormedTrip);
                CurrentClientChangeTripWindow.Close();
            }
                
        }
    }
}
