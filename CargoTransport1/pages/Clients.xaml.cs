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
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Clients : Page
    {
        //описание вспомогательных переменных
        private int DlgMode;
        private string F_buf;
        private string I_buf;
        private string O_buf;
        private string Adress_buf;
        private string Phone_buf;

        public delegate void ClientHendler(Base.Clients client);
        public event ClientHendler Notify;

        public void ClientDlgLoad(bool isOpen)
        {
            if (isOpen == true)
            {
                ClientsColumnChange.Width = new GridLength(400);
                // ClientsColumnChange.IsEnabled = true;
            }
            else
            {
                ClientsColumnChange.Width = new GridLength(0);
                //ClientsColumnChange.IsEnabled = false;
            }

        }

        public Clients()
        {
            InitializeComponent();
            ClientsColumnChange.Width = new GridLength(0);
            //ClientsColumnChange.
            DataContext = this;
            ClientsGrid.ItemsSource = SourceCore.MyBase.Clients.ToList();
        }

        private void ClientAdd_Click(object sender, RoutedEventArgs e)
        {
            ClientDlgLoad(true);
            DlgMode = 0;
            ClientsGrid.IsHitTestVisible = false;
            ClientsGrid.SelectedItem = null;
            ClientsLabel.Content = "Добавить клиента";
            ClientAddCommit.Content = "Добавить клиента";
            ClientF.Text = "";
            ClientI.Text = "";
            ClientO.Text = "";
            ClientAdress.Text = "";
            ClientPhone.Text = "";
        }

        private void ClientCopy_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem != null)
            {
                ClientDlgLoad(true);
                DlgMode = 0;
                ClientsLabel.Content = "Копировать - добавить клиента на основе выбранного";
                ClientAddCommit.Content = "Копировать клиента";
                ClientsGrid.IsHitTestVisible = false;
                F_buf = ClientF.Text;
                I_buf = ClientI.Text;
                O_buf = ClientO.Text;
                Adress_buf = ClientAdress.Text;
                Phone_buf = ClientPhone.Text;
                ClientsGrid.SelectedItem = null;
                ClientF.Text = F_buf;
                ClientI.Text = I_buf;
                ClientO.Text = O_buf;
                ClientAdress.Text = Adress_buf;
                ClientPhone.Text = Phone_buf;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void ClientEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem != null)
            {
                ClientDlgLoad(true);
                ClientsLabel.Content = "Изменить клиента";
                ClientAddCommit.Content = "Изменить клиента";
                ClientsGrid.IsHitTestVisible = false;
                DlgMode = -1;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void ClientDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {

                try
                {
                    // Ссылка на удаляемую книгу
                    Base.Clients DeletingClient = (Base.Clients)ClientsGrid.SelectedItem;
                    // Определение ссылки, на которую должен перейти указатель после удаления
                    if (ClientsGrid.SelectedIndex < ClientsGrid.Items.Count - 1)
                    {
                        ClientsGrid.SelectedIndex++;
                    }
                    else
                    {
                        if (ClientsGrid.SelectedIndex > 0)
                        {
                            ClientsGrid.SelectedIndex--;
                        }
                    }
                    Base.Clients SelectingClient = (Base.Clients)ClientsGrid.SelectedItem;
                    SourceCore.MyBase.Clients.Remove(DeletingClient);
                    SourceCore.MyBase.SaveChanges();
                    UpdateGrid(SelectingClient);
                }
                catch
                {

                    MessageBox.Show("Невозможно удалить запись, так как она используется в других справочниках базы данных.",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                }
            }
        }


        private void ClientAddCommit_Click(object sender, RoutedEventArgs e)
        {
            var NewClient = new Base.Clients();
            NewClient.Cl_F = ClientF.Text;
            NewClient.Cl_I = ClientI.Text;
            NewClient.Cl_O = ClientO.Text;
            NewClient.Cl_Adress = ClientAdress.Text;
            NewClient.Cl_Phone = ClientPhone.Text;
            if (DlgMode == 0)
            {
                SourceCore.MyBase.Clients.Add(NewClient);
            }
            SourceCore.MyBase.SaveChanges();
            UpdateGrid(null);
            ClientsGrid.IsHitTestVisible = true;
            ClientDlgLoad(false);

        }

        private void ClientAddRollback_Click(object sender, RoutedEventArgs e)
        {
            ClientDlgLoad(false);
            ClientsGrid.IsHitTestVisible = true;
        }
        public void UpdateGrid(Base.Clients Clients)
        {
            if ((Clients == null) && (ClientsGrid.ItemsSource != null))
            {
                Clients = (Base.Clients)ClientsGrid.SelectedItem;
            }
            ClientsGrid.ItemsSource = SourceCore.MyBase.Clients.ToList();
            ClientsGrid.SelectedItem = Clients;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Первоначальная настройка фильтра данных для быстрого поиска,
            // при этом из фильтрации нужно исключить столбец "Управление"
            // Создание собствнного списка заголовков столбцов
            List<String> Columns = new List<string>();
            // Перебор и добавление в новый список только необходимых заголовков
            // Исключен столбец 4
            for (int I = 0; I < 5; I++)
            {
                Columns.Add(ClientsGrid.Columns[I].Header.ToString());
            }
            ClientFilterComboBox.ItemsSource = Columns;
            ClientFilterComboBox.SelectedIndex = 0;
            // Запрет на управление сортировкой щелчком по заголовку столбца
            foreach (DataGridColumn Column in ClientsGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //BooksViewModel.View.Refresh();
            var textbox = sender as TextBox;
            switch (ClientFilterComboBox.SelectedIndex)
            {
                case 0:
                    ClientsGrid.ItemsSource = SourceCore.MyBase.Clients.Where(q => q.Cl_F.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    ClientsGrid.ItemsSource = SourceCore.MyBase.Clients.Where(q => q.Cl_I.Contains(textbox.Text)).ToList();
                    break;
                case 2:
                    ClientsGrid.ItemsSource = SourceCore.MyBase.Clients.Where(q => q.Cl_O.Contains(textbox.Text)).ToList();
                    break;
                case 3:
                    ClientsGrid.ItemsSource = SourceCore.MyBase.Clients.Where(q => q.Cl_Adress.Contains(textbox.Text)).ToList();
                    break;
                case 4:
                    ClientsGrid.ItemsSource = SourceCore.MyBase.Clients.Where(q => q.Cl_Phone.Contains(textbox.Text)).ToList();
                    break;
            }
        }

        private void CallEvent(object sender, SelectionChangedEventArgs e)
        {
            Notify?.Invoke((Base.Clients)ClientsGrid.SelectedItem);
        }
    }
}
