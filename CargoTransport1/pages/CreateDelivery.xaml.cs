
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

namespace CargoTransport1.pages
{
    /// <summary>
    /// Логика взаимодействия для CreateDelivery.xaml
    /// </summary>
    public partial class CreateDelivery : Page
    {
        public int driverID;
        public int carID;
        public List<Base.Orders> orders = new List<Base.Orders>();
        int _curPage = 0;
        int curPage
        {
            get
            {
                return _curPage;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _curPage = value;
                switch (_curPage)
                {
                    case 0:
                        FrameForPages.Navigate(new pages.Cars(this));
                        DeliveryAddCommit.Content = "Далее";
                        break;
                    case 1:
                        FrameForPages.Navigate(new pages.Drivers(this));
                        DeliveryAddCommit.Content = "Далее";
                        break;
                    case 2:
                        FrameForPages.Navigate(new pages.Orders(this));
                        DeliveryAddCommit.Content = "Создать доставку";
                        break;
                    case 3:
                        var NewDelivery = new Base.Deliveries();
                        NewDelivery.Car_ID = carID;
                        NewDelivery.Dr_ID = driverID;
                        SourceCore.MyBase.Deliveries.Add(NewDelivery);
                        for (int i = 0; i < orders.Count; i++)
                        {
                            Base.Orders o = SourceCore.MyBase.Orders.Find(orders[i].Ord_ID);
                            o.Del_ID = NewDelivery.Del_ID;
                        }
                        SourceCore.MyBase.SaveChanges();
                        FrameForPages.Navigate(new pages.Orders(this));
                        orders = new List<Base.Orders>();
                        OrdersGrid.ItemsSource = null;
                        _curPage = 0;
                        FrameForPages.Navigate(new pages.Cars(this));
                        DeliveryAddCommit.Content = "Далее";
                        Driver.Text = null;
                        Car.Text = null;
                        break;
                }
            }
        }
        public void UpdateGrid(Base.Orders Orders)
        {

            OrdersGrid.ItemsSource = orders.ToList();
            //OrdersGrid.SelectedItem = Orders;

        }
        public CreateDelivery()
        {
            InitializeComponent();
            DataContext = this;
            FrameForPages.Navigate(new pages.Cars(this));
            OrdersGrid.ItemsSource = orders;

        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            curPage++;
        }

        private void PreviusPage(object sender, RoutedEventArgs e)
        {
            curPage--;
        }

        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            Base.Orders curOrd = (Base.Orders)OrdersGrid.SelectedItem;
            curOrd = orders.Find(x => x.Ord_ID == curOrd.Ord_ID);
            orders.Remove(curOrd);
            UpdateGrid(null);
        }
    }
}
