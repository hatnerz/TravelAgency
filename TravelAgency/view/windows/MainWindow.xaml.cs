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
using System.Windows.Threading;
using TravelAgency.view.pages;

namespace TravelAgency
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Pages { MainPage, DataBaseAdmin, Tours, Staff }
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
                }
                
            }
            
        }

        public Manager currentUser { private set; get; }
        private DispatcherTimer _timer;
        public MainWindow(Manager currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                timeBlock.Text = DateTime.Now.ToString("HH:mm:ss\ndd.MM.yyyy");
            }, Dispatcher);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string status;
            if (currentUser.Admin == true)
            {
                dataBasePageButton.IsEnabled = true;
                employeesPageButton.IsEnabled = true;
                status = "адміністратор";
            }
                
            else
                status = "менеджер";
            infoBlock.Text = "Вітаємо, " + currentUser.GetFullName + "!\nВи авторизувалися як " + status;
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
    }
}
