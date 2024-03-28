
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
    public partial class Orders_Deliveris : Page
    {
        //описание вспомогательных переменных
        private int DlgMode;
        private string F_buf;
        private string I_buf;
        private string O_buf;
        private string Adress_buf;
        private string Phone_buf;
        //регулирование размера колонки для работы с данными
        public void ClientDlgLoad(bool isOpen)
        {
            //if (isOpen == true)
            //{
            //    ClientsColumnChange.Width = new GridLength(400);
            //   // ClientsColumnChange.IsEnabled = true;
            //}
            //else
            //{
            //    ClientsColumnChange.Width = new GridLength(0);
            //    //ClientsColumnChange.IsEnabled = false;
            //}

        }

        public Orders_Deliveris()
        {
            InitializeComponent();
            DataContext = this;
            List<Base.Deliveries> deliveries = SourceCore.MyBase.Deliveries.ToList();
            deliveries.Remove(deliveries.Find(x => x.Del_ID.Equals(3)));
            DeliveriesGrid.ItemsSource = deliveries;
        }

        private void ClientAdd_Click(object sender, RoutedEventArgs e)
        {
            StaticResouse.mainWindow.TransportFrame.Navigate(new pages.CreateDelivery());
        }

        private void ClientCopy_Click(object sender, RoutedEventArgs e)
        {
            //if (ClientsGrid.SelectedItem != null)
            //{
            //    ClientDlgLoad(true);
            //    DlgMode = 0;
            //    ClientsLabel.Content = "Копировать - добавить клиента на основе выбранного";
            //    ClientAddCommit.Content = "Копировать клиента";
            //    ClientsGrid.IsHitTestVisible = false;
            //    F_buf = ClientF.Text;
            //    I_buf = ClientI.Text;
            //    O_buf = ClientO.Text;
            //    Adress_buf = ClientAdress.Text;
            //    Phone_buf = ClientPhone.Text;
            //    ClientsGrid.SelectedItem = null;
            //    ClientF.Text = F_buf;
            //    ClientI.Text = I_buf;
            //    ClientO.Text = O_buf;
            //    ClientAdress.Text = Adress_buf;
            //    ClientPhone.Text = Phone_buf;
            //}
            //else
            //{
            //    MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            //}
        }

        private void ClientEdit_Click(object sender, RoutedEventArgs e)
        {
            //if (ClientsGrid.SelectedItem != null)
            //{
            //    ClientDlgLoad(true);
            //    ClientsLabel.Content = "Изменить клиента";
            //    ClientAddCommit.Content = "Изменить клиента";
            //    ClientsGrid.IsHitTestVisible = false;
            //    DlgMode = -1;
            //}
            //else
            //{
            //    MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            //}
        }

        private void ClientDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                Base.Deliveries DeletingDelivery = (Base.Deliveries)DeliveriesGrid.SelectedItem;
                if (DeliveriesGrid.SelectedIndex < DeliveriesGrid.Items.Count - 1)
                {
                    DeliveriesGrid.SelectedIndex++;
                }
                else
                {
                    if (DeliveriesGrid.SelectedIndex > 0)
                    {
                        DeliveriesGrid.SelectedIndex--;
                    }
                }
                List<Base.Orders> orders = SourceCore.MyBase.Orders.ToList();
                for (int i = SourceCore.MyBase.Orders.Count() - 1; i >= 0; i--)
                {
                    if (orders[i].Del_ID == DeletingDelivery.Del_ID)
                    {
                        SourceCore.MyBase.Orders.Remove(orders[i]);
                    }
                }
                SourceCore.MyBase.Deliveries.Remove(DeletingDelivery);
                SourceCore.MyBase.SaveChanges();
                UpdateGrid(DeletingDelivery);
            }
        }


        private void ClientAddCommit_Click(object sender, RoutedEventArgs e)
        {
            //var NewClient = new Base.Clients();
            //NewClient.Cl_F = ClientF.Text;
            //NewClient.Cl_I = ClientI.Text;
            //NewClient.Cl_O = ClientO.Text;
            //NewClient.Cl_Adress = ClientAdress.Text;
            //NewClient.Cl_Phone = ClientPhone.Text;
            //if (DlgMode == 0)
            //{
            //    SourceCore.MyBase.Clients.Add(NewClient);
            //}
            //SourceCore.MyBase.SaveChanges();
            //UpdateGrid(null);
            //ClientsGrid.IsHitTestVisible = true;
            //ClientDlgLoad(false);

        }

        private void ClientAddRollback_Click(object sender, RoutedEventArgs e)
        {
            //ClientDlgLoad(false);
            //ClientsGrid.IsHitTestVisible = true;
        }
        public void UpdateGrid(Base.Deliveries Deliveries)
        {
            if ((Deliveries == null) && (DeliveriesGrid.ItemsSource != null))
            {
                Deliveries = (Base.Deliveries)DeliveriesGrid.SelectedItem;
            }
            DeliveriesGrid.ItemsSource = SourceCore.MyBase.Deliveries.ToList();
            DeliveriesGrid.SelectedItem = Deliveries;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> Columns = new List<string>();
            for (int I = 0; I < 3; I++)
            {
                Columns.Add(DeliveriesGrid.Columns[I].Header.ToString());
            }
            ClientFilterComboBox.ItemsSource = Columns;
            ClientFilterComboBox.SelectedIndex = 0;
            // Запрет на управление сортировкой щелчком по заголовку столбца
            foreach (DataGridColumn Column in DeliveriesGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //BooksViewModel.View.Refresh();
            var textbox = sender as TextBox;
            List<Base.Deliveries> deliveries = new List<Base.Deliveries>();
            List<Base.Deliveries> baseDeliveries = SourceCore.MyBase.Deliveries.ToList();
            switch (ClientFilterComboBox.SelectedIndex)
            {

                case 0:
                    deliveries = SourceCore.MyBase.Deliveries.Where(q => q.Del_ID.ToString().Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    List<Base.Drivers> drivers = SourceCore.MyBase.Drivers.Where(q => q.Dr_F.Contains(textbox.Text)).ToList();
                    //List<Base.Deliveries> deliveries = new List<Base.Deliveries>();
                    //List<Base.Deliveries> baseDeliveries = SourceCore.MyBase.Deliveries.ToList();
                    for (int i = 0; i < drivers.Count(); i++)
                    {
                        deliveries.AddRange(baseDeliveries.FindAll(q => q.Dr_ID.Equals(drivers[i].Dr_ID)));

                    }
                    break;
                case 2:
                    List<Base.Models> models = SourceCore.MyBase.Models.Where(q => q.Mdl_Name.Contains(textbox.Text)).ToList();
                    //List<Base.Deliveries> deliveries = new List<Base.Deliveries>();
                    //List<Base.Deliveries> baseDeliveries = SourceCore.MyBase.Deliveries.ToList();
                    List<Base.Cars> cars = new List<Base.Cars>();
                    List<Base.Cars> baseCars = SourceCore.MyBase.Cars.ToList();
                    for (int i = 0; i < models.Count(); i++)
                    {
                        cars.AddRange(baseCars.FindAll(q => q.Mdl_ID.Equals(models[i].Mdl_ID)));
                    }
                    for (int i = 0; i < cars.Count(); i++)
                    {
                        deliveries.AddRange(baseDeliveries.FindAll(q => q.Car_ID.Equals(cars[i].Car_ID)));
                    }                    
                    break;
            }
            deliveries.Remove(deliveries.Find(x => x.Del_ID.Equals(3)));
            DeliveriesGrid.ItemsSource = deliveries;
        }

        private void CloseDelivery(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < SourceCore.MyBase.Orders.Count(); i++)
            {
                Base.Deliveries curDelivery = (Base.Deliveries)DeliveriesGrid.SelectedItem;
                List<Base.Orders> orders = SourceCore.MyBase.Orders.ToList();
                if (orders.ElementAt(i).Del_ID == curDelivery.Del_ID)
                {
                    orders.ElementAt(i).Ord_DateEnd = DateTime.Now;
                    SourceCore.MyBase.SaveChanges();
                }
            }

        }

        private void DeliveriesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void UpdateGrid()
        {
            object curObject = DeliveriesGrid.SelectedItem;
            DeliveriesGrid.SelectedItem = null;
            DeliveriesGrid.SelectedItem = curObject;
        }

        private void CloseDelivery_Click(object sender, RoutedEventArgs e)
        {
            Base.Deliveries curDel = (Base.Deliveries)DeliveriesGrid.SelectedItem;
            List<Base.Orders> orders = SourceCore.MyBase.Orders.ToList();
            for (int i = 0; i < SourceCore.MyBase.Orders.Count(); i++)
            {
                if ((curDel.Del_ID == orders.ElementAt(i).Del_ID) && (orders.ElementAt(i).Ord_DateEnd) == null)
                {
                    orders.ElementAt(i).Ord_DateEnd = DateTime.Now;
                }
            }
            SourceCore.MyBase.SaveChanges();
            UpdateGrid();
        }
    }
}
