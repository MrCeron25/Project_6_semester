using Project.Models;
using Project.Views.Pages;
using System.Windows;
using System.Linq;
using System.Threading;

namespace Project
{
    public partial class MainWindow : Window
    {

        #region Тестовое подключение (Для ускорения загрузки данных из EF)
        private void TestConnect()
        {
            var data = from employees in Singleton.Instance.Context.Employees
                       select employees;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Singleton.Instance.MainFrame = MainFrame;
            Singleton.Instance.Navigate(new LoginPage());
            // TestConnect в новом потоке
            Thread thread = new Thread(TestConnect);
            thread.Start();
        }
    }
}
