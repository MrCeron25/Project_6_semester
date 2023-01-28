using Project.Infrastructure.Commands;
using Project.Models;
using Project.Models.ViewModel;
using Project.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModels
{
    internal class ViewingEmployeeViewModel : ViewModelActionCommand
    {
        #region Consts
        private readonly static string DeletedStatus = Application.Current.FindResource("DeletedStatus") as string;
        #endregion

        #region Employees
        private ObservableCollection<EmployeeItem> _employees;
        public ObservableCollection<EmployeeItem> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }
        #endregion

        #region UpdateEmployees
        private void UpdateEmployees()
        {
            Employees = new ObservableCollection<EmployeeItem>(
                from e in Singleton.Instance.Context.Employees
                join er in Singleton.Instance.Context.Employees_roles on e.role equals er.role_id
                orderby e.surname, e.name, e.patronymic
                where er.role_name != DeletedStatus
                select new EmployeeItem
                {
                    employe_id = e.employe_id,
                    name = e.name,
                    surname = e.surname,
                    patronymic = e.patronymic,
                    login = e.login,
                    password = e.password,
                    role = e.role,
                    role_name = er.role_name,
                    phone = e.phone,
                    sex = e.sex,
                    photo = e.photo
                }
            );
        }
        #endregion



        #region OnAddExecuted
        protected override void OnAddExecuted(object parameters)
        {
            Console.WriteLine(EmployeePageTemplateViewModel.Instanse);
            Console.WriteLine("AddEmployeeCommand");

            EmployeePageTemplateViewModel.Instanse.CurrentEmployeeActionEntities = EmployeePageTemplateAction.Add;
            Singleton.Instance.Navigate(new EmployeePageTemplate());
        }
        #endregion

        #region OnChangeExecuted
        protected override void OnChangeExecuted(object parameters)
        {
            Console.WriteLine(EmployeePageTemplateViewModel.Instanse);
            Console.WriteLine("ChangeEmployeeCommand");

            EmployeeItem employee = parameters as EmployeeItem;
            if (employee != null)
            {
                Console.WriteLine(employee);
                EmployeePageTemplateViewModel.Instanse.SelectedEmployee = employee;
                EmployeePageTemplateViewModel.Instanse.CurrentEmployeeActionEntities = EmployeePageTemplateAction.Change;
                Singleton.Instance.Navigate(new EmployeePageTemplate());
            }
        }
        #endregion

        #region OnDeleteExecuted
        protected override void OnDeleteExecuted(object parameters)
        {
            Console.WriteLine("DeleteEmployeeCommand");
            EmployeeItem employeeParameter = parameters as EmployeeItem;

            Console.WriteLine(employeeParameter);
            try
            {
                Employees employee = (
                    from em in Singleton.Instance.Context.Employees
                    where em.employe_id == employeeParameter.employe_id
                    select em
                ).FirstOrDefault();
                employee.role = (
                    from er in Singleton.Instance.Context.Employees_roles
                    where er.role_name == DeletedStatus
                    select er.role_id
                ).FirstOrDefault();
                Singleton.Instance.Context.SaveChanges();
                UpdateViewModel();
                MessageBox.Show($"Сотрудник (id={employee.employe_id}) {employee.surname} {employee.surname} {employee.patronymic}  удалён.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
        #endregion

        #region GoToMallsCommand
        public ICommand GoToMallsCommand { get; }
        private void OnGoToMallsCommandExecuted(object parameters) => Singleton.Instance.Navigate(new ViewingMallsPage());
        private bool CanGoToMallsCommandExecute(object parameters) => true;
        #endregion

        #region Instanse
        public static ViewingEmployeeViewModel Instanse { get; } = new ViewingEmployeeViewModel();
        #endregion

        #region Конструктор
        private ViewingEmployeeViewModel()
        {
            _addCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            _changeCommand = new LambdaCommand(OnChangeExecuted, CanChangeExecute);
            _deleteCommand = new LambdaCommand(OnDeleteExecuted, CanDeleteExecute);
            _updateViewModelCommand = new LambdaCommand(OnUpdateViewModelCommandExecuted, CanUpdateViewModelCommandExecute);

            GoToMallsCommand = new LambdaCommand(OnGoToMallsCommandExecuted, CanGoToMallsCommandExecute);

            UpdateViewModel();
        }
        #endregion

        #region UpdateViewModel
        public override void UpdateViewModel()
        {
            UpdateEmployees();
        }
        #endregion
    }
}
