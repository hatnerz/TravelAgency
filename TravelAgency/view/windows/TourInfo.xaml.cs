using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
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
using TravelAgency.viewmodel;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для TourInfo.xaml
    /// </summary>
    public partial class TourInfo : Window
    {
        public TourInfoViewModel SelectedTourViewModel;
        public TourInfo(Tour selectedTour)
        {
            InitializeComponent();
            SelectedTourViewModel = new TourInfoViewModel(selectedTour);
            InfoDataGrid.DataContext = SelectedTourViewModel;
        }

        private void goBookingButton_Click(object sender, RoutedEventArgs e)
        {
            TourBooking tourBooking = new TourBooking(SelectedTourViewModel.CurrentTour);
            tourBooking.ShowDialog();
        }

        private void formTourMembers_Click(object sender, RoutedEventArgs e)
        {
            SelectedTourViewModel.CurrentTour.FormTourMembers();
        }
    }
}
