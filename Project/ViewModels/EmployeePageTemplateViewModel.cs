using Microsoft.Win32;
using Project.Infrastructure.Commands;
using Project.Models;
using Project.Models.Validators;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Project.ViewModels
{
    public enum EmployeePageTemplateAction
    {
        Add,
        Change
    }

    internal class EmployeePageTemplateViewModel : UpdatableViewModel
    {
        #region Consts
        private readonly static string Woman = "Ж";
        private readonly static string Man = "М";
        #endregion

        #region BackCommand
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object parameters) => Singleton.Instance.Navigate(new ViewingEmployeePage());
        private bool CanBackCommandExecute(object parameters) => true;
        #endregion

        #region SelectedEmployee
        private EmployeeItem _selectedEmployee;
        public EmployeeItem SelectedEmployee
        {
            get => _selectedEmployee;
            set => Set(ref _selectedEmployee, value);
        }
        #endregion

        #region SelectedEmployeeRoles
        private string _selectedEmployeeRoles;
        public string SelectedEmployeeRoles
        {
            get => _selectedEmployeeRoles;
            set => Set(ref _selectedEmployeeRoles, value);
        }
        #endregion

        #region CurrentEmployeeActionEntities
        private EmployeePageTemplateAction _currentEmployeeActionEntities;
        public EmployeePageTemplateAction CurrentEmployeeActionEntities
        {
            get => _currentEmployeeActionEntities;
            set
            {
                Set(ref _currentEmployeeActionEntities, value);
                switch (CurrentEmployeeActionEntities)
                {
                    case EmployeePageTemplateAction.Add:
                        EmployeePageTemplateButtonName = "Добавить";
                        CurrentEmployee = new Employees();
                        LoadedImage = null;
                        break;
                    case EmployeePageTemplateAction.Change:
                        EmployeePageTemplateButtonName = "Изменить";
                        CurrentEmployee = (
                            from em in Singleton.Instance.Context.Employees
                            where em.employe_id == SelectedEmployee.employe_id
                            select em
                        ).FirstOrDefault();
                        SelectedEmployeeSex = CurrentEmployee.sex;
                        SelectedEmployeeRoles = (
                            from emr in Singleton.Instance.Context.Employees_roles
                            where emr.role_id == CurrentEmployee.role
                            select emr.role_name
                        ).FirstOrDefault();
                        if (CurrentEmployee.photo != null)
                        {
                            LoadedImage = Tools.BytesToImage(CurrentEmployee.photo);
                        }
                        break;
                }
            }
        }
        #endregion

        #region LoadedImage
        private BitmapImage _loadedImage;
        public BitmapImage LoadedImage
        {
            get => _loadedImage;
            set => Set(ref _loadedImage, value);
        }
        #endregion

        #region Загрузка фотографии
        public ICommand LoadPhotoCommand { get; }
        private bool CanLoadPhotoCommandExecute(object parameters) => true;
        private void OnLoadPhotoCommandExecuted(object parameters)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*png;"
                };
                if ((bool)fileDialog.ShowDialog())
                {
                    if (fileDialog.FileName.EndsWith(".jpg") ||
                        fileDialog.FileName.EndsWith(".png"))
                    {
                        CurrentEmployee.photo = Tools.GetImageBytes(fileDialog.FileName);
                        LoadedImage = new BitmapImage(new Uri(fileDialog.FileName));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region ExecuteCommand
        public ICommand ExecuteCommand { get; }
        private bool CanExecuteCommandExecute(object parameters) => true;
        private void OnExecuteCommandExecuted(object parameters)
        {
            string error = string.Empty;
            EmployeeValidator employeeValidator = new EmployeeValidator();
            if (employeeValidator.IsNotValid(CurrentEmployee, out error))
            {
                MessageBox.Show(error);
            }
            else if (LoadedImage == null)
            {
                MessageBox.Show($"Выберите фото.");
            }
            else if (SelectedEmployeeSex == null)
            {
                MessageBox.Show($"Выберите пол.");
            }
            else if (SelectedEmployeeRoles == null)
            {
                MessageBox.Show($"Выберите роль.");
            }
            else
            {
                try
                {
                    CurrentEmployee.surname = CurrentEmployee.surname.Trim();
                    CurrentEmployee.name = CurrentEmployee.name.Trim();
                    CurrentEmployee.patronymic = CurrentEmployee.patronymic.Trim();
                    CurrentEmployee.login = CurrentEmployee.login.Trim();
                    CurrentEmployee.password = CurrentEmployee.password.Trim();
                    CurrentEmployee.phone = CurrentEmployee.phone.Trim();
                    CurrentEmployee.sex = SelectedEmployeeSex;
                    CurrentEmployee.role = (
                        from em in Singleton.Instance.Context.Employees_roles
                        where em.role_name == SelectedEmployeeRoles
                        select em.role_id
                    ).FirstOrDefault();
                    switch (CurrentEmployeeActionEntities)
                    {
                        case EmployeePageTemplateAction.Add:
                            Singleton.Instance.Context.Employees.Add(CurrentEmployee);
                            break;
                        case EmployeePageTemplateAction.Change:
                            break;
                    }
                    Singleton.Instance.Context.SaveChanges();
                    switch (CurrentEmployeeActionEntities)
                    {
                        case EmployeePageTemplateAction.Add:
                            MessageBox.Show($"Сотрудник добавлен.");
                            break;
                        case EmployeePageTemplateAction.Change:
                            MessageBox.Show($"Сотрудник изменён.");
                            break;
                    }
                    ViewingEmployeeViewModel.Instanse.UpdateViewModel();
                    Singleton.Instance.Navigate(new ViewingEmployeePage());
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e}");
                }
            }
            UpdateViewModel();
        }
        #endregion

        #region EmployeeSex
        private ObservableCollection<string> _employeeSex = new ObservableCollection<string>() { Woman, Man };
        public ObservableCollection<string> EmployeeSex
        {
            get => _employeeSex;
            set => Set(ref _employeeSex, value);
        }
        #endregion

        #region EmployeeSex
        private string _selectedEmployeeSex;
        public string SelectedEmployeeSex
        {
            get => _selectedEmployeeSex;
            set => Set(ref _selectedEmployeeSex, value);
        }
        #endregion

        #region EmployeeRoles
        private ObservableCollection<string> _employeeRoles;
        public ObservableCollection<string> EmployeeRoles
        {
            get => _employeeRoles;
            set => Set(ref _employeeRoles, value);
        }
        #endregion

        #region UpdateEmployeeRoles
        private void UpdateEmployeeRoles()
        {
            EmployeeRoles = new ObservableCollection<string>(
                from em in Singleton.Instance.Context.Employees_roles
                select em.role_name
            );
        }
        #endregion

        #region EmployeePageTemplateButtonName
        private string _employeePageTemplateButtonName;
        public string EmployeePageTemplateButtonName
        {
            get => _employeePageTemplateButtonName;
            set => Set(ref _employeePageTemplateButtonName, value);
        }
        #endregion

        #region CurrentEmployee
        private Employees _сurrentEmployee;
        public Employees CurrentEmployee
        {
            get => _сurrentEmployee;
            set => Set(ref _сurrentEmployee, value);
        }
        #endregion

        #region Instanse
        public static EmployeePageTemplateViewModel Instanse { get; } = new EmployeePageTemplateViewModel();
        #endregion

        #region Конструктор
        private EmployeePageTemplateViewModel()
        {
            BackCommand = new LambdaCommand(OnBackCommandExecuted, CanBackCommandExecute);
            LoadPhotoCommand = new LambdaCommand(OnLoadPhotoCommandExecuted, CanLoadPhotoCommandExecute);
            ExecuteCommand = new LambdaCommand(OnExecuteCommandExecuted, CanExecuteCommandExecute);

            UpdateViewModel();
        }
        #endregion

        #region UpdateViewModel
        public override void UpdateViewModel()
        {
            UpdateEmployeeRoles();
            SelectedEmployeeSex = Woman;
        }
        #endregion
    }
}
