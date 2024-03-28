
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
    public partial class Drivers : Page
    {
        //описание вспомогательных переменных
        private int DlgMode;
        private string Dr_License;
        private int Ppl_ID;
        private string F_buf;
        private string I_buf;
        private string O_buf;
        private string Adress_buf;
        private string Phone_buf;
        private string DriverLicense_buf;
        //регулирование размера колонки для работы с данными
        public void DriverDlgLoad(bool isOpen)
        {
            if (isOpen == true)
            {
                DriversColumnChange.Width = new GridLength(400);
                // DriversColumnChange.IsEnabled = true;
            }
            else
            {
                DriversColumnChange.Width = new GridLength(0);
                //DriversColumnChange.IsEnabled = false;
            }

        }

        public Drivers()
        {
            InitializeComponent();
            DriversColumnChange.Width = new GridLength(0);
            //DriversColumnChange.
            DataContext = this;
            DriversGrid.ItemsSource = SourceCore.MyBase.Drivers.ToList();
        }
        pages.CreateDelivery _createDelivery;
        public Drivers(pages.CreateDelivery createDelivery)
        {
            InitializeComponent();
            DriversColumnChange.Width = new GridLength(0);
            //DriversColumnChange.
            DataContext = this;
            DriversGrid.ItemsSource = SourceCore.MyBase.Drivers.ToList();
            _createDelivery = createDelivery;
        }

        private void DriverAdd_Click(object sender, RoutedEventArgs e)
        {
            DriverDlgLoad(true);
            DlgMode = 0;
            DriversGrid.IsHitTestVisible = false;
            DriversGrid.SelectedItem = null;
            DriversLabel.Content = "Добавить водителя";
            DriverAddCommit.Content = "Добавить водителя";
            DriverF.Text = "";
            DriverI.Text = "";
            DriverO.Text = "";
            DriverAdress.Text = "";
            DriverPhone.Text = "";
            DriverLicense.Text = "";
        }

        private void DriverCopy_Click(object sender, RoutedEventArgs e)
        {
            if (DriversGrid.SelectedItem != null)
            {
                DriverDlgLoad(true);
                DlgMode = 0;
                DriversLabel.Content = "Копировать - добавить водителя на основе выбранного";
                DriverAddCommit.Content = "Копировать водителя";
                DriversGrid.IsHitTestVisible = false;
                F_buf = DriverF.Text;
                I_buf = DriverI.Text;
                O_buf = DriverO.Text;
                Adress_buf = DriverAdress.Text;
                Phone_buf = DriverPhone.Text;
                DriverLicense_buf = DriverLicense.Text;
                DriversGrid.SelectedItem = null;
                DriverF.Text = F_buf;
                DriverI.Text = I_buf;
                DriverO.Text = O_buf;
                DriverAdress.Text = Adress_buf;
                DriverPhone.Text = Phone_buf;
                DriverLicense.Text = DriverLicense_buf;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void DriverEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DriversGrid.SelectedItem != null)
            {
                DriverDlgLoad(true);
                DriversLabel.Content = "Изменить водителя";
                DriverAddCommit.Content = "Изменить водителя";
                DriversGrid.IsHitTestVisible = false;
                DlgMode = -1;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void DriverDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {

                try
                {
                    // Ссылка на удаляемую книгу
                    Base.Drivers DeletingDriver = (Base.Drivers)DriversGrid.SelectedItem;
                    // Определение ссылки, на которую должен перейти указатель после удаления
                    if (DriversGrid.SelectedIndex < DriversGrid.Items.Count - 1)
                    {
                        DriversGrid.SelectedIndex++;
                    }
                    else
                    {
                        if (DriversGrid.SelectedIndex > 0)
                        {
                            DriversGrid.SelectedIndex--;
                        }
                    }
                    Base.Drivers SelectingDriver = (Base.Drivers)DriversGrid.SelectedItem;
                    SourceCore.MyBase.Drivers.Remove(DeletingDriver);
                    SourceCore.MyBase.SaveChanges();
                    UpdateGrid(SelectingDriver);
                }
                catch
                {

                    MessageBox.Show("Невозможно удалить запись, так как она используется в других справочниках базы данных.",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                }
            }
        }


        private void DriverAddCommit_Click(object sender, RoutedEventArgs e)
        {
            var NewDriver = new Base.Drivers();
            NewDriver.Dr_F = DriverF.Text;
            NewDriver.Dr_I = DriverI.Text;
            NewDriver.Dr_O = DriverO.Text;
            NewDriver.Dr_Adress = DriverAdress.Text;
            NewDriver.Dr_Phone = DriverPhone.Text;
            NewDriver.Dr_License = DriverLicense.Text;
            if (DlgMode == 0)
            {
                SourceCore.MyBase.Drivers.Add(NewDriver);
            }
            SourceCore.MyBase.SaveChanges();
            UpdateGrid(null);
            DriversGrid.IsHitTestVisible = true;
            DriverDlgLoad(false);

        }

        private void DriverAddRollback_Click(object sender, RoutedEventArgs e)
        {
            DriverDlgLoad(false);
            DriversGrid.IsHitTestVisible = true;
        }
        public void UpdateGrid(Base.Drivers driver)
        {
            //if ((driver == null) && (DriversGrid.ItemsSource != null))
            //{
            //    driver = (Base.People)DriversGrid.SelectedItem;
            //}
            DriversGrid.ItemsSource = SourceCore.MyBase.Drivers.ToList();
            //BookDataGrid.SelectedIndex = books;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Первоначальная настройка фильтра данных для быстрого поиска,
            // при этом из фильтрации нужно исключить столбец "Управление"
            // Создание собствнного списка заголовков столбцов
            List<String> Columns = new List<string>();
            // Перебор и добавление в новый список только необходимых заголовков
            // Исключен столбец 4
            for (int I = 0; I < 6; I++)
            {
                Columns.Add(DriversGrid.Columns[I].Header.ToString());
            }
            DriverFilterComboBox.ItemsSource = Columns;
            DriverFilterComboBox.SelectedIndex = 0;
            // Запрет на управление сортировкой щелчком по заголовку столбца
            foreach (DataGridColumn Column in DriversGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //BooksViewModel.View.Refresh();
            var textbox = sender as TextBox;
            switch (DriverFilterComboBox.SelectedIndex)
            {
                case 0:
                    DriversGrid.ItemsSource = SourceCore.MyBase.Drivers.Where(q => q.Dr_F.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    DriversGrid.ItemsSource = SourceCore.MyBase.Drivers.Where(q => q.Dr_I.Contains(textbox.Text)).ToList();
                    break;
                case 2:
                    DriversGrid.ItemsSource = SourceCore.MyBase.Drivers.Where(q => q.Dr_O.Contains(textbox.Text)).ToList();
                    break;
                case 3:
                    DriversGrid.ItemsSource = SourceCore.MyBase.Drivers.Where(q => q.Dr_Adress.Contains(textbox.Text)).ToList();
                    break;
                case 4:
                    DriversGrid.ItemsSource = SourceCore.MyBase.Drivers.Where(q => q.Dr_Phone.Contains(textbox.Text)).ToList();
                    break;
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_createDelivery != null)
            {
                Base.Drivers driver = (Base.Drivers)DriversGrid.SelectedItem;
                _createDelivery.driverID = driver.Dr_ID;
                _createDelivery.Driver.Text = driver.Dr_I;
            }
        }
    }
}
