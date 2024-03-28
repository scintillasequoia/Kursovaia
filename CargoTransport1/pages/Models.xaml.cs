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
    public partial class Models : Page
    {
        //описание вспомогательных переменных
        private int DlgMode;
        private string ModelName_buf;
        private string ModelCarrindCapacity_buf;
        private string ModelFuelConsumption_buf;
        //регулирование размера колонки для работы с данными
        public void ModelDlgLoad(bool isOpen)
        {
            if (isOpen == true)
            {
                ModelsColumnChange.Width = new GridLength(400);
                // ModelsColumnChange.IsEnabled = true;
            }
            else
            {
                ModelsColumnChange.Width = new GridLength(0);
                //ModelsColumnChange.IsEnabled = false;
            }

        }

        public Models()
        {
            InitializeComponent();
            ModelsColumnChange.Width = new GridLength(0);
            //ModelsColumnChange.
            DataContext = this;
            ModelsGrid.ItemsSource = SourceCore.MyBase.Models.ToList();
        }

        private void ModelAdd_Click(object sender, RoutedEventArgs e)
        {
            ModelDlgLoad(true);
            DlgMode = 0;
            ModelsGrid.IsHitTestVisible = false;
            ModelsGrid.SelectedItem = null;
            ModelsLabel.Content = "Добавить модель";
            ModelAddCommit.Content = "Добавить модель";
            ModelName.Text = "";
            ModelCarrindCapacity.Text = "";
            ModelFuelConsumption.Text = "";
        }

        private void ModelCopy_Click(object sender, RoutedEventArgs e)
        {
            if (ModelsGrid.SelectedItem != null)
            {
                ModelDlgLoad(true);
                DlgMode = 0;
                ModelsLabel.Content = "Копировать - добавить модель на основе выбранной";
                ModelAddCommit.Content = "Копировать модель";
                ModelsGrid.IsHitTestVisible = false;
                ModelName_buf = ModelName.Text;
                ModelCarrindCapacity_buf = ModelCarrindCapacity.Text;
                ModelFuelConsumption_buf = ModelFuelConsumption.Text;
                ModelsGrid.SelectedItem = null;
                ModelName.Text = ModelName_buf;
                ModelCarrindCapacity.Text = ModelCarrindCapacity_buf;
                ModelFuelConsumption.Text = ModelFuelConsumption_buf;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void ModelEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ModelsGrid.SelectedItem != null)
            {
                ModelDlgLoad(true);
                ModelsLabel.Content = "Изменить модель";
                ModelAddCommit.Content = "Изменить модель";
                ModelsGrid.IsHitTestVisible = false;
                DlgMode = -1;
            }
            else
            {
                MessageBox.Show("Не выбрано ниодной строки!", "Сообщение", MessageBoxButton.OK);
            }
        }

        private void ModelDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {

                try
                {
                    // Ссылка на удаляемую книгу
                    Base.Models DeletingModel = (Base.Models)ModelsGrid.SelectedItem;
                    // Определение ссылки, на которую должен перейти указатель после удаления
                    if (ModelsGrid.SelectedIndex < ModelsGrid.Items.Count - 1)
                    {
                        ModelsGrid.SelectedIndex++;
                    }
                    else
                    {
                        if (ModelsGrid.SelectedIndex > 0)
                        {
                            ModelsGrid.SelectedIndex--;
                        }
                    }
                    Base.Models SelectingModel = (Base.Models)ModelsGrid.SelectedItem;
                    SourceCore.MyBase.Models.Remove(DeletingModel);
                    SourceCore.MyBase.SaveChanges();
                    UpdateGrid(SelectingModel);
                }
                catch
                {

                    MessageBox.Show("Невозможно удалить запись, так как она используется в других справочниках базы данных.",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                }
            }
        }


        private void ModelAddCommit_Click(object sender, RoutedEventArgs e)
        {
            var NewModel = new Base.Models();
            NewModel.Mdl_Name = ModelName.Text;
            NewModel.Mdl_FuelConsumption = int.Parse(ModelFuelConsumption.Text);
            NewModel.Mdl_CarrindCapacity = int.Parse(ModelCarrindCapacity.Text);
            if (DlgMode == 0)
            {
                SourceCore.MyBase.Models.Add(NewModel);
            }
            SourceCore.MyBase.SaveChanges();
            UpdateGrid(null);
            ModelsGrid.IsHitTestVisible = true;
            ModelDlgLoad(false);

        }

        private void ModelAddRollback_Click(object sender, RoutedEventArgs e)
        {
            ModelDlgLoad(false);
            ModelsGrid.IsHitTestVisible = true;
        }
        public void UpdateGrid(Base.Models Models)
        {
            if ((Models == null) && (ModelsGrid.ItemsSource != null))
            {
                Models = (Base.Models)ModelsGrid.SelectedItem;
            }
            ModelsGrid.ItemsSource = SourceCore.MyBase.Models.ToList();
            ModelsGrid.SelectedItem = Models;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Первоначальная настройка фильтра данных для быстрого поиска,
            // при этом из фильтрации нужно исключить столбец "Управление"
            // Создание собствнного списка заголовков столбцов
            List<String> Columns = new List<string>();
            // Перебор и добавление в новый список только необходимых заголовков
            // Исключен столбец 4
            for (int I = 0; I < 3; I++)
            {
                Columns.Add(ModelsGrid.Columns[I].Header.ToString());
            }
            ModelFilterComboBox.ItemsSource = Columns;
            ModelFilterComboBox.SelectedIndex = 0;
            // Запрет на управление сортировкой щелчком по заголовку столбца
            foreach (DataGridColumn Column in ModelsGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //BooksViewModel.View.Refresh();
            var textbox = sender as TextBox;
            switch (ModelFilterComboBox.SelectedIndex)
            {
                case 0:
                    ModelsGrid.ItemsSource = SourceCore.MyBase.Models.Where(q => q.Mdl_Name.Contains(textbox.Text)).ToList();
                    break;
                    //case 1:
                    //    ModelsGrid.ItemsSource = SourceCore.MyBase.Models.Where(q => q.Mdl_FuelConsumption.Contains(textbox.Text)).ToList();
                    //    break;
                    //case 2:
                    //    ModelsGrid.ItemsSource = SourceCore.MyBase.Models.Where(q => q.Cl_O.Contains(textbox.Text)).ToList();
                    //    break;
            }
        }


    }
}
