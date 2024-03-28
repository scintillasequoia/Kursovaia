
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
using System.Windows.Shapes;

namespace CargoTransport1
{
    public partial class Authorisation : Window
    {
        private Base.CargoTransport12Entities DataBase;
        public Authorisation()
        {
            InitializeComponent();
            LoginText.Text = "Admin";
            PasswordText.Text = "Admin";
            try
            {
                DataBase = new Base.CargoTransport12Entities();
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к базе данных. Проверьте настройки подключения приложения.",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
            }
        }

        private void AuthorizationRollBack_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти из программы?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                Close();
            }

        }

        private void AuthorizationCommit_Click(object sender, RoutedEventArgs e)
        {
            Base.Accounts User = DataBase.Accounts.SingleOrDefault(U => U.Acc_Login == LoginText.Text && U.Acc_Password == PasswordText.Text);
            if (User != null)
            {
                MainWindow window = new MainWindow();
                window.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверно указан логин и/или пароль!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            };

        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow window = new RegistrationWindow(DataBase);
            window.ShowDialog();

        }
    }
}
