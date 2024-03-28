
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CargoTransport1.pages
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Orders : Page
    {
        //описание вспомогательных переменных
        private int DlgMode;
        private string Package_buf;
        private Decimal Distance_buf;
        private string Ord_PlaceStart_buf;
        private string Ord_PlaceEnd_buf;
        private string Phone_buf;
        //регулирование размера колонки для работы с данными
        public void OrderDlgLoad(bool isOpen)
        {
            if (isOpen == true)
            {
                OrdersColumnChange.Width = new GridLength(400);
                ClientsRowChange.Height = new GridLength(400);
                // OrdersColumnChange.IsEnabled = true;
            }
            else
            {
                OrdersColumnChange.Width = new GridLength(0);
                ClientsRowChange.Height = new GridLength(0);
                //OrdersColumnChange.IsEnabled = false;
            }

        }
        pages.Clients innerPage = new pages.Clients();
        pages.CreateDelivery _createDelivery;
        public Orders()
        {

            InitializeComponent();
            OrdersColumnChange.Width = new GridLength(0);
            DataContext = this;
            OrdersGrid.ItemsSource = SourceCore.MyBase.Orders.ToList();

            FrameForClients.Navigate(innerPage);
            innerPage.Notify += innerPageSelectionChanged;
            ClientsRowChange.Height = new GridLength(0);
        }
        public Orders(pages.CreateDelivery createDelivery)
        {

            InitializeComponent();
            OrdersColumnChange.Width = new GridLength(0);
            DataContext = this;
            List<Base.Orders> legalOrders = SourceCore.MyBase.Orders.ToList();
            for (int i = legalOrders.Count - 1; i >= 0; i--)
            {
                if (legalOrders[i].Del_ID != 3)
                {
                    legalOrders.RemoveAt(i);
                }
            }

            OrdersGrid.ItemsSource = legalOrders;

            FrameForClients.Navigate(innerPage);
            innerPage.Notify += innerPageSelectionChanged;
            ClientsRowChange.Height = new GridLength(0);
            _createDelivery = createDelivery;

        }
        Base.Clients curClient;
        private void innerPageSelectionChanged(Base.Clients client)
        {
            curClient = client;
            ClientName.Text = curClient.Cl_I + " " + curClient.Cl_F + " " + curClient.Cl_O;
        }

        private void OrderAdd_Click(object sender, RoutedEventArgs e)
        {
            OrderDlgLoad(true);
            DlgMode = 0;
            OrdersGrid.IsHitTestVisible = false;
            OrdersGrid.SelectedItem = null;
            OrdersLabel.Content = "Добавить заказ";
            OrderAddCommit.Content = "Добавить заказ";
            ClientName.Text = "";
            Distance.Text = "";
            Package.Text = "";
            PlaceStart.Text = "";
            PlaceEnd.Text = "";
        }
        decimal DecimalParse(string s)
        {
            s = s.Replace('.', ',');
            return decimal.Parse(s);
        }

        private void OrderCopy_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                OrderDlgLoad(true);
                DlgMode = 0;
                OrdersLabel.Content = "Копировать - добавить заказ на основе выбранного";
                OrderAddCommit.Content = "Копировать заказ";
                OrdersGrid.IsHitTestVisible = false;
                Package_buf = Package.Text;
                Distance_buf = DecimalParse(Distance.Text);
                Ord_PlaceStart_buf = PlaceStart.Text;
                Ord_PlaceEnd_buf = PlaceEnd.Text;

                OrdersGrid.SelectedItem = null;
                Package.Text = Package_buf;
                Distance.Text = Distance_buf.ToString();
                PlaceStart.Text = Ord_PlaceStart_buf;
                PlaceEnd.Text = Ord_PlaceEnd_buf;

            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void OrderEdit_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                Base.Orders ord = (Base.Orders)OrdersGrid.SelectedItem;
                curClient = (Base.Clients)innerPage.ClientsGrid.Items.GetItemAt(ord.Cl_ID - 1);
                ClientName.Text = curClient.Cl_I + " " + curClient.Cl_F + " " + curClient.Cl_O;
                OrderDlgLoad(true);
                OrdersLabel.Content = "Изменить заказ";
                OrderAddCommit.Content = "Изменить заказ";
                //OrdersGrid.IsHitTestVisible = false;
                DlgMode = -1;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void OrderDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {

                try
                {
                    Base.Orders DeletingOrder = (Base.Orders)OrdersGrid.SelectedItem;
                    // Определение ссылки, на которую должен перейти указатель после удаления
                    if (OrdersGrid.SelectedIndex < OrdersGrid.Items.Count - 1)
                    {
                        OrdersGrid.SelectedIndex++;
                    }
                    else
                    {
                        if (OrdersGrid.SelectedIndex > 0)
                        {
                            OrdersGrid.SelectedIndex--;
                        }
                    }
                    Base.Orders SelectingOrder = (Base.Orders)OrdersGrid.SelectedItem;
                    SourceCore.MyBase.Orders.Remove(DeletingOrder);
                    SourceCore.MyBase.SaveChanges();
                    UpdateGrid(SelectingOrder);
                }
                catch
                {

                    MessageBox.Show("Невозможно удалить запись, так как она используется в других справочниках базы данных.",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                }
            }
        }


        private void OrderAddCommit_Click(object sender, RoutedEventArgs e)
        {
            var NewOrder = new Base.Orders();

            NewOrder.Cl_ID = curClient.Cl_ID;
            NewOrder.Distance = DecimalParse(Distance.Text);
            NewOrder.Package = Package.Text;
            NewOrder.Ord_PlaceStart = PlaceStart.Text;
            NewOrder.Ord_PlaceEnd = PlaceEnd.Text;
            NewOrder.Ord_Price = 10;
            NewOrder.Ord_DateStart = DateTime.Now;
            NewOrder.Del_ID = 3;
            if (DlgMode == 0)
            {
                SourceCore.MyBase.Orders.Add(NewOrder);
            }
            SourceCore.MyBase.SaveChanges();
            UpdateGrid(null);
            OrdersGrid.IsHitTestVisible = true;
            OrderDlgLoad(false);

        }

        private void OrderAddRollback_Click(object sender, RoutedEventArgs e)
        {
            OrderDlgLoad(false);
            OrdersGrid.IsHitTestVisible = true;
        }
        public void UpdateGrid(Base.Orders Orders)
        {
            if ((Orders == null) && (OrdersGrid.ItemsSource != null))
            {
                Orders = (Base.Orders)OrdersGrid.SelectedItem;
            }
            OrdersGrid.ItemsSource = SourceCore.MyBase.Orders.ToList();
            OrdersGrid.SelectedItem = Orders;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Первоначальная настройка фильтра данных для быстрого поиска,
            // при этом из фильтрации нужно исключить столбец "Управление"
            // Создание собствнного списка заголовков столбцов
            List<String> Columns = new List<string>();
            // Перебор и добавление в новый список только необходимых заголовков
            // Исключен столбец 4
            Columns.Add(OrdersGrid.Columns[4].Header.ToString());
            Columns.Add(OrdersGrid.Columns[5].Header.ToString());
            Columns.Add(OrdersGrid.Columns[8].Header.ToString());
            OrderFilterComboBox.ItemsSource = Columns;
            OrderFilterComboBox.SelectedIndex = 0;
            // Запрет на управление сортировкой щелчком по заголовку столбца
            foreach (DataGridColumn Column in OrdersGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //    BooksViewModel.View.Refresh();
            var textbox = sender as TextBox;
            switch (OrderFilterComboBox.SelectedIndex)
            {
                case 0:
                    OrdersGrid.ItemsSource = SourceCore.MyBase.Orders.Where(q => q.Ord_PlaceStart.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    OrdersGrid.ItemsSource = SourceCore.MyBase.Orders.Where(q => q.Ord_PlaceEnd.Contains(textbox.Text)).ToList();
                    break;
                case 2:
                    OrdersGrid.ItemsSource = SourceCore.MyBase.Orders.Where(q => q.Package.Contains(textbox.Text)).ToList();

                    break;
                    //        case 3:
                    //            OrdersGrid.ItemsSource = SourceCore.MyBase.Orders.Where(q => q.Cl_Adress.Contains(textbox.Text)).ToList();
                    //            break;
                    //        case 4:
                    //            OrdersGrid.ItemsSource = SourceCore.MyBase.Orders.Where(q => q.Cl_Phone.Contains(textbox.Text)).ToList();
                    //            break;
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_createDelivery != null)
            {
                Base.Orders order = (Base.Orders)OrdersGrid.SelectedItem;
                Base.Orders o = new Base.Orders();
                o.Ord_ID = -111;
                _createDelivery.orders.Add(o);
                if (_createDelivery.orders.Exists(x => x.Ord_ID == order.Ord_ID))
                {

                }
                else
                {
                    _createDelivery.orders.Add(order);
                    _createDelivery.UpdateGrid(null);
                }
                _createDelivery.orders.Remove(o);
                _createDelivery.UpdateGrid(null);
            }
        }
    }
}
