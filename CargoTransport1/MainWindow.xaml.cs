

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

namespace CargoTransport1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StaticResouse.mainWindow = this;
        }
        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            TransportFrame.Navigate(new pages.Clients());
        }

        private void DriversButton_Click(object sender, RoutedEventArgs e)
        {
            TransportFrame.Navigate(new pages.Drivers());
        }

        private void ModelsButton_Click(object sender, RoutedEventArgs e)
        {
            TransportFrame.Navigate(new pages.Models());
        }

        private void CarsButton_Click(object sender, RoutedEventArgs e)
        {
            TransportFrame.Navigate(new pages.Cars());
        }

        private void Orders_DeliverisButton_Click(object sender, RoutedEventArgs e)
        {
            TransportFrame.Navigate(new pages.Orders_Deliveris());
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            TransportFrame.Navigate(new pages.Orders());
        }

        private void CreateDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            TransportFrame.Navigate(new pages.CreateDelivery());
        }
    }
}

