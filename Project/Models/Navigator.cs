using Project.Views.Pages;
using System.Windows;
using System.Windows.Controls;

namespace Project.Models
{
    public class Singleton
    {
        private static Singleton _instance;
        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Singleton();
                }
                return _instance;
            }
        }

        public Frame MainFrame { get; set; }

        private static practiceEntities _context = new practiceEntities();
        public practiceEntities Context
        {
            get => _context;
            set => _context = value;
        }

        public void Navigate(Page content)
        {
            if (MainFrame != null)
            {
                if (MainFrame.CanGoBack)
                {
                    MainFrame.NavigationService.RemoveBackEntry();
                }
                MainFrame.Navigate(content);
            }
            else
            {
                MessageBox.Show($"MainFrame не существует.");
            }
        }

        private Employees _loginedEmployee { get; set; }

        public Employees LoginedEmployee
        {
            get => _loginedEmployee;
            set => _loginedEmployee = value;
        }

        internal void LoadEmployeePage()
        {
            if (_loginedEmployee == null)
            {
                MessageBox.Show($"LoginedEmployee не существует.");
                return;
            }
            Navigate(new MallViewingPage());
            //switch (_employees.role)
            //{
            //    //case 1:
            //    //    break;
            //    //case 1:
            //    //    break;
            //    //case 1:
            //    //    break;
            //    //case 1:
            //    //    break;
            //    default:
            //        break;
            //}
        }
    }
}
