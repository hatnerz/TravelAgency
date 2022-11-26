using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using TravelAgency.view.windows;

namespace TravelAgency.view.pages
{
    /// <summary>
    /// Логика взаимодействия для Staff.xaml
    /// </summary>
    public partial class Staff : Page
    {
        SqlDataAdapter oda;
        DataTable staffViewDataTable;
        public void UpdataDataTable()
        {
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string command = "SELECT * FROM managers";
            connection.Open();
            oda = new SqlDataAdapter(command, connection);
            staffViewDataTable.Clear();
            oda.Fill(staffViewDataTable);
            connection.Close();
        }
        public Staff()
        {
            InitializeComponent();
            staffViewDataTable = new DataTable();
            staffDataGrid.ItemsSource = staffViewDataTable.AsDataView();
            UpdataDataTable();
        }

        private void staffDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataRowView selectedManager = (DataRowView)staffDataGrid.SelectedItem;
            EmployeeInfoWindow employeeInfoWindow = new EmployeeInfoWindow(new Manager(selectedManager));
            employeeInfoWindow.ShowDialog();
            UpdataDataTable();
        }
    }
}
