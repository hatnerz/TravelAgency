using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf.qrcode;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TravelAgency.view.pages;
using TravelAgency.viewmodel;
//using static System.Net.Mime.MediaTypeNames;

namespace TravelAgency
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Pages { MainPage, DataBaseAdmin, Tours, Staff, Clients, Hotels }
        static Pages currentPage;
        private void pageNavigate(Pages page)
        {
            void changeButtons()
            {
                foreach (Button button in menuButtons.Children)
                {
                    string pageName = currentPage.ToString();
                    if ((string)button.Tag == pageName)
                        button.Style = (Style)Application.Current.Resources["MaterialDesignRaisedButton"];
                    else
                        button.Style = (Style)Application.Current.Resources["MaterialDesignFlatButton"];
                }
            }
            if (currentPage != page)
            {
                currentPage = page;
                switch (page) {
                    case Pages.MainPage:
                        changeButtons();
                        mainFrame.Navigate(new MainPage());
                        break;
                    case Pages.DataBaseAdmin:
                        changeButtons();
                        mainFrame.Navigate(new DataBaseAdmin());
                        break;
                    case Pages.Tours:
                        changeButtons();
                        mainFrame.Navigate(new Tours());
                        break;
                    case Pages.Staff:
                        changeButtons();
                        mainFrame.Navigate(new Staff());
                        break;
                    case Pages.Clients:
                        changeButtons();
                        mainFrame.Navigate(new Clients());
                        break;
                    case Pages.Hotels:
                        changeButtons();
                        mainFrame.Navigate(new Hotels());
                        break;
                }
                
            }
            
        }

        public Manager currentUser { private set; get; }
        public ManagerViewModel ManagerViewModel;
        private DispatcherTimer _timer;
        public MainWindow(Manager currentUser)
        {
            Directory.CreateDirectory("reports");
            InitializeComponent();
            this.currentUser = currentUser;
            this.ManagerViewModel = new ManagerViewModel(currentUser);
            mainGrid.DataContext = ManagerViewModel;
            Application.Current.Properties.Add("currentUser", currentUser);
            if (!File.Exists("email_check.dat"))
            {
                using (StreamWriter writer = new StreamWriter("email_check.dat", false))
                {
                    writer.WriteLine(DateTime.Now);
                }
            }
            
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                timeBlock.Text = DateTime.Now.ToString("HH:mm:ss\ndd.MM.yyyy");
            }, Dispatcher);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentUser.Admin == true)
            {
                dataBasePageButton.IsEnabled = true;
                employeesPageButton.IsEnabled = true;
            }
            mainFrame.Navigate(new MainPage());
        }

        private void dataBasePageButton_Click(object sender, RoutedEventArgs e)
        {
            pageNavigate(Pages.DataBaseAdmin);
        }

        private void mainPageButton_Click(object sender, RoutedEventArgs e)
        {
            pageNavigate(Pages.MainPage);
        }

        private void toursPageButton_Click(object sender, object e)
        {
            pageNavigate(Pages.Tours);
        }

        private void employeesPageButton_Click(object sender, RoutedEventArgs e)
        {
            pageNavigate(Pages.Staff);
        }

        private void clientsPageButton_Click(object sender, RoutedEventArgs e)
        {
            pageNavigate(Pages.Clients);
        }

        private void hotelsPageButton_Click(object sender, RoutedEventArgs e)
        {
            pageNavigate(Pages.Hotels);
        }
    }
}
