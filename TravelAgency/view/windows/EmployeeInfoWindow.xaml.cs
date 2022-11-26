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
using System.Windows.Shapes;

namespace TravelAgency.view.windows
{
    /// <summary>
    /// Логика взаимодействия для EmployeeInfoWindow.xaml
    /// </summary>
    public partial class EmployeeInfoWindow : Window
    {
        Manager currentManager;
        public EmployeeInfoWindow()
        {
            InitializeComponent();
            currentManager = new Manager(1,"34","34","352","4526","6547",true,"436");
            currentManager.LastName = "keks";
            infoGrid.DataContext = currentManager;
        }

        public EmployeeInfoWindow(Manager currentManager)
        {
            InitializeComponent();
            this.currentManager = currentManager;
            infoGrid.DataContext = currentManager;
        }

        private void changeInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if((string)changeInfoButton.Tag == "change")
            {
                changeInfoButton.Content = "Зберегти зміни";
                firstNameTextBox.IsEnabled = true;
                lastNameTextBox.IsEnabled = true;
                patronymicNameTextBox.IsEnabled = true;
                loginTextBox.IsEnabled = true;
                passwordTextBox.IsEnabled = true;
                officePhoneTextBox.IsEnabled = true;
                adminCheckBox.IsEnabled = true;
                changeInfoButton.Tag = "confirmChanges";
                changeInfoButton.Content = "Зберегти зміни";
            }

            else
            {
                changeInfoButton.Content = "Змінити дані";
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
                patronymicNameTextBox.IsEnabled = false;
                loginTextBox.IsEnabled = false;
                passwordTextBox.IsEnabled = false;
                officePhoneTextBox.IsEnabled = false;
                adminCheckBox.IsEnabled = false;
                changeInfoButton.Tag = "change";
                currentManager.UpdateTable();
            }
        }
    }
}
