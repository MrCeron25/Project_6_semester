using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Project.Models;
using Project.ViewModels.Base;
using Project.Infrastructure.Commands;
using System.Data.SqlClient;
using System.Windows;
using Project.Views.Pages;

namespace Project.ViewModels
{
    internal class PavilionRentalViewModel : UpdatableViewModel
    {
        #region Consts
        static private readonly string RentActionName = "Аренда";
        static private readonly string BookActionName = "Бронь";
        #endregion

        #region RentActions
        private ObservableCollection<string> _rentActions;
        public ObservableCollection<string> RentActions
        {
            get => _rentActions;
            set => Set(ref _rentActions, value);
        }
        #endregion

        #region UpdateRentActions
        private void UpdateRentActions()
        {
            RentActions = new ObservableCollection<string>()
            {
                RentActionName,
                BookActionName
            };
        }
        #endregion

        #region PavilionRentalButtonName
        private string _pavilionRentalButtonName = RentActionName;
        public string PavilionRentalButtonName
        {
            get => _pavilionRentalButtonName;
            set => Set(ref _pavilionRentalButtonName, value);
        }
        #endregion

        #region SelectedRentAction
        private string _selectedRentAction;
        public string SelectedRentAction
        {
            get => _selectedRentAction;
            set
            {
                Set(ref _selectedRentAction, value);
                if (_selectedRentAction == RentActionName)
                {
                    PavilionRentalButtonName = "Арендовать";
                }
                else if (_selectedRentAction == BookActionName)
                {
                    PavilionRentalButtonName = "Забронировать";
                }
            }
        }
        #endregion

        #region RentalStartDate
        private DateTime? _rentalStartDate = DateTime.Today;
        public DateTime? RentalStartDate
        {
            get => _rentalStartDate;
            set
            {
                Set(ref _rentalStartDate, value);
                MinimumRentalEndDate = _rentalStartDate;
            }
        }
        #endregion

        #region MinimumRentalStartDate
        private DateTime? _minimumRentalStartDate = DateTime.Today;
        public DateTime? MinimumRentalStartDate
        {
            get => _minimumRentalStartDate;
            set => Set(ref _minimumRentalStartDate, value);
        }
        #endregion

        #region MaximumRentalStartDate
        private DateTime? _maximumRentalStartDate;
        public DateTime? MaximumRentalStartDate
        {
            get => _maximumRentalStartDate;
            set => Set(ref _maximumRentalStartDate, value);
        }
        #endregion

        #region RentalEndDate
        private DateTime? _rentalEndDate;
        public DateTime? RentalEndDate
        {
            get => _rentalEndDate;
            set
            {
                Set(ref _rentalEndDate, value);
                MaximumRentalStartDate = _rentalEndDate;
            }
        }
        #endregion

        #region MinimumRentalEndDate
        private DateTime? _minimumRentalEndDate = DateTime.Today;
        public DateTime? MinimumRentalEndDate
        {
            get => _minimumRentalEndDate;
            set => Set(ref _minimumRentalEndDate, value);
        }
        #endregion

        #region MaximumRentalEndDate
        private DateTime? _maximumRentalEndDate;
        public DateTime? MaximumRentalEndDate
        {
            get => _maximumRentalEndDate;
            set => Set(ref _maximumRentalEndDate, value);
        }
        #endregion

        #region Tenants
        private ObservableCollection<string> _tenants;
        public ObservableCollection<string> Tenants
        {
            get => _tenants;
            set => Set(ref _tenants, value);
        }
        #endregion

        #region SelectedTenant
        private string _selectedTenant;
        public string SelectedTenant
        {
            get => _selectedTenant;
            set => Set(ref _selectedTenant, value);
        }
        #endregion

        #region UpdateTenants
        private void UpdateTenants()
        {
            Tenants = new ObservableCollection<string>(
                from t in Singleton.Instance.Context.Tenants
                select t.company_name
            );
        }
        #endregion

        #region Employees
        private ObservableCollection<string> _employees;
        public ObservableCollection<string> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }
        #endregion

        #region SelectedEmployee
        private string _selectedEmployee;
        public string SelectedEmployee
        {
            get => _selectedEmployee;
            set => Set(ref _selectedEmployee, value);
        }
        #endregion

        #region GetEmployees
        private ObservableCollection<string> GetEmployees()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            foreach (Employees em in from t in Singleton.Instance.Context.Employees
                                     select t)
            {
                result.Add($"{em.employe_id} {em.surname} {em.name} {em.patronymic}");
            }
            return result;
        }
        #endregion

        #region UpdateEmployees
        private void UpdateEmployees()
        {
            Employees = GetEmployees();
        }
        #endregion

        #region SelectedPavilion
        private PavilionItem _selectedPavilion;
        public PavilionItem SelectedPavilion
        {
            get => _selectedPavilion;
            set => Set(ref _selectedPavilion, value);
        }
        #endregion

        #region BackCommand
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object parameters) => Singleton.Instance.Navigate(new ViewingPavilionsPage());
        private bool CanBackCommandExecute(object parameters) => true;
        #endregion

        #region PavilionRentalCommand
        public ICommand PavilionRentalCommand { get; }
        private bool CanPavilionRentalCommandExecute(object parameters) => true;
        private void OnPavilionRentalCommandExecuted(object parameters)
        {
            if (SelectedRentAction != null &&
                RentalStartDate != null &&
                SelectedPavilion != null &&
                RentalEndDate != null &&
                SelectedTenant != null &&
                SelectedEmployee != null)
            {
                try
                {
                    int status_action = (SelectedRentAction == RentActionName) ? 0 : 1;
                    string pavilion_number = SelectedPavilion.pavilion_number;
                    long mall_id = SelectedPavilion.mall_id;
                    long tenant_id = (from t in Singleton.Instance.Context.Tenants
                                      where t.company_name == SelectedTenant
                                      select t.tenant_id).FirstOrDefault();
                    long employee_id = long.Parse(SelectedEmployee.Split(' ')[0]);
                    Singleton.Instance.Context.Database.ExecuteSqlCommand
                        ($@"exec RentOrBookPavilionInMall @status_action, 
                                                      @pavilion_number, 
                                                      @mall_id, 
                                                      @start_date, 
                                                      @end_date, 
                                                      @tenant_id, 
                                                      @employee_id",
                        new SqlParameter("@status_action", status_action),
                        new SqlParameter("@pavilion_number", pavilion_number),
                        new SqlParameter("@mall_id", mall_id),
                        new SqlParameter("@start_date", RentalStartDate),
                        new SqlParameter("@end_date", RentalEndDate),
                        new SqlParameter("@tenant_id", tenant_id),
                        new SqlParameter("@employee_id", employee_id));
                    MessageBox.Show($"Павильон {((SelectedRentAction == RentActionName) ? "арендован" : "забронирован")}.");
                    Singleton.Instance.Navigate(new ViewingPavilionsPage());
                    UpdateViewModel();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e}");
                }
            }
            else
            {
                MessageBox.Show($"Заполните все поля.");
            }
        }
        #endregion

        #region Instanse
        public static PavilionRentalViewModel Instanse { get; } = new PavilionRentalViewModel();
        #endregion

        #region Конструктор
        private PavilionRentalViewModel()
        {
            BackCommand = new LambdaCommand(OnBackCommandExecuted, CanBackCommandExecute);
            PavilionRentalCommand = new LambdaCommand(OnPavilionRentalCommandExecuted, CanPavilionRentalCommandExecute);
            UpdateViewModel();
        }
        #endregion

        public override void UpdateViewModel()
        {
            UpdateRentActions();
            UpdateTenants();
            UpdateEmployees();
        }
    }
}
