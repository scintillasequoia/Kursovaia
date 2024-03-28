
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
    public partial class Cars : Page
    {
        //описание вспомогательных переменных
        private int DlgMode;
        private string RegNumber_buf;
        //регулирование размера колонки для работы с данными
        public void CarDlgLoad(bool isOpen)
        {
            if (isOpen == true)
            {
                CarsColumnChange.Width = new GridLength(400);
                // CarsColumnChange.IsEnabled = true;
            }
            else
            {
                CarsColumnChange.Width = new GridLength(0);
                //CarsColumnChange.IsEnabled = false;
            }

        }

        public Cars()
        {
            InitializeComponent();
            List<String> models = new List<string>();
            Models.ItemsSource = SourceCore.MyBase.Models.ToList();
            Models.SelectedIndex = 0;
            CarsColumnChange.Width = new GridLength(0);
            //CarsColumnChange.
            DataContext = this;
            CarsGrid.ItemsSource = SourceCore.MyBase.Cars.ToList();
        }
        private pages.CreateDelivery _createDelivery;
        public Cars(pages.CreateDelivery createDelivery)
        {
            InitializeComponent();
            List<String> models = new List<string>();
            Models.ItemsSource = SourceCore.MyBase.Models.ToList();
            Models.SelectedIndex = 0;
            CarsColumnChange.Width = new GridLength(0);
            //CarsColumnChange.
            DataContext = this;
            CarsGrid.ItemsSource = SourceCore.MyBase.Cars.ToList();
            _createDelivery = createDelivery;
        }

        private void CarAdd_Click(object sender, RoutedEventArgs e)
        {
            CarDlgLoad(true);
            DlgMode = 0;
            CarsGrid.IsHitTestVisible = false;
            CarsGrid.SelectedItem = null;
            CarsLabel.Content = "Добавить машину";
            CarAddCommit.Content = "Добавить машину";
            RegNumber.Text = "";
        }

        private void CarCopy_Click(object sender, RoutedEventArgs e)
        {
            if (CarsGrid.SelectedItem != null)
            {
                CarDlgLoad(true);
                DlgMode = 0;
                CarsLabel.Content = "Копировать - добавить машину на основе выбранной";
                CarAddCommit.Content = "Копировать машину";
                CarsGrid.IsHitTestVisible = false;
                RegNumber_buf = RegNumber.Text;
                CarsGrid.SelectedItem = null;
                RegNumber.Text = RegNumber_buf;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void CarEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CarsGrid.SelectedItem != null)
            {
                CarDlgLoad(true);
                CarsLabel.Content = "Изменить машину";
                CarAddCommit.Content = "Изменить машину";
                CarsGrid.IsHitTestVisible = false;
                DlgMode = -1;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void CarDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {

                try
                {
                    // Ссылка на удаляемую книгу
                    Base.Cars DeletingCar = (Base.Cars)CarsGrid.SelectedItem;
                    // Определение ссылки, на которую должен перейти указатель после удаления
                    if (CarsGrid.SelectedIndex < CarsGrid.Items.Count - 1)
                    {
                        CarsGrid.SelectedIndex++;
                    }
                    else
                    {
                        if (CarsGrid.SelectedIndex > 0)
                        {
                            CarsGrid.SelectedIndex--;
                        }
                    }
                    Base.Cars SelectingCar = (Base.Cars)CarsGrid.SelectedItem;
                    SourceCore.MyBase.Cars.Remove(DeletingCar);
                    SourceCore.MyBase.SaveChanges();
                    UpdateGrid(SelectingCar);
                }
                catch
                {

                    MessageBox.Show("Невозможно удалить запись, так как она используется в других справочниках базы данных.",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                }
            }
        }


        private void CarAddCommit_Click(object sender, RoutedEventArgs e)
        {
            var NewCar = new Base.Cars();
            //NewCar.Cl_F = CarF.Text;
            //NewCar.Cl_I = CarI.Text;
            //NewCar.Cl_O = CarO.Text;
            //NewCar.Cl_Adress = CarAdress.Text;
            //NewCar.Cl_Phone = CarPhone.Text;
            if (DlgMode == 0)
            {
                SourceCore.MyBase.Cars.Add(NewCar);
            }
            SourceCore.MyBase.SaveChanges();
            UpdateGrid(null);
            CarsGrid.IsHitTestVisible = true;
            CarDlgLoad(false);

        }

        private void CarAddRollback_Click(object sender, RoutedEventArgs e)
        {
            CarDlgLoad(false);
            CarsGrid.IsHitTestVisible = true;
        }
        public void UpdateGrid(Base.Cars Cars)
        {
            if ((Cars == null) && (CarsGrid.ItemsSource != null))
            {
                Cars = (Base.Cars)CarsGrid.SelectedItem;
            }
            CarsGrid.ItemsSource = SourceCore.MyBase.Cars.ToList();
            CarsGrid.SelectedItem = Cars;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


            List<String> Columns = new List<string>();
            for (int I = 0; I < 2; I++)
            {
                Columns.Add(CarsGrid.Columns[I].Header.ToString());
            }
            CarFilterComboBox.ItemsSource = Columns;
            CarFilterComboBox.SelectedIndex = 0;
            // Запрет на управление сортировкой щелчком по заголовку столбца
            foreach (DataGridColumn Column in CarsGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //BooksViewModel.View.Refresh();
            var textbox = sender as TextBox;
            switch (CarFilterComboBox.SelectedIndex)
            {
                //case 0:
                //    CarsGrid.ItemsSource = SourceCore.MyBase.Cars.Where(q => q.Cl_F.Contains(textbox.Text)).ToList();
                //    break;
                //case 1:
                //    CarsGrid.ItemsSource = SourceCore.MyBase.Cars.Where(q => q.Cl_I.Contains(textbox.Text)).ToList();
                //    break;
                //case 2:
                //    CarsGrid.ItemsSource = SourceCore.MyBase.Cars.Where(q => q.Cl_O.Contains(textbox.Text)).ToList();
                //    break;
                //case 3:
                //    CarsGrid.ItemsSource = SourceCore.MyBase.Cars.Where(q => q.Cl_Adress.Contains(textbox.Text)).ToList();
                //    break;
                //case 4:
                //    CarsGrid.ItemsSource = SourceCore.MyBase.Cars.Where(q => q.Cl_Phone.Contains(textbox.Text)).ToList();
                //    break;
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_createDelivery != null)
            {
                Base.Cars car = (Base.Cars)CarsGrid.SelectedItem;
                _createDelivery.carID = car.Car_ID;
                _createDelivery.Car.Text = car.Car_RegNumber;
            }
        }
    }
}
