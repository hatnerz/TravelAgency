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
    /// Логика взаимодействия для AddResponceWindow.xaml
    /// </summary>
    public partial class AddResponceWindow : Window
    {
        ResponceViewModel responceViewModel;
        public AddResponceWindow(Responce responce)
        {
            InitializeComponent();
            responceViewModel = new ResponceViewModel(responce);
            mainGrid.DataContext = responceViewModel;
            scoreComboBox.ItemsSource = new int[] { 1, 2, 3, 4, 5 };
        }

        private void addResponceButton_Click(object sender, RoutedEventArgs e)
        {
            ResponceAdapter.InsertResponce(responceViewModel.CurrentResponce);
            MessageBox.Show("Відгук додано");
            this.Close();
        }
    }
}
