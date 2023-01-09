using Project.Views.Pages;
using System;
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

        public practiceEntities Context { get; set; } = new practiceEntities();

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
        }

        private Employees _loginedEmployee { get; set; }

        internal void LoadEmployeeInterface(Employees employees)
        {
            // TODO: Load state from previously suspended application
            _loginedEmployee = employees;
            Navigate(new MallPage());
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
