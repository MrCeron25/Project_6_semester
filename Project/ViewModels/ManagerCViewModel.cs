using Project.Infrastructure.Commands;
using Project.Models;
using Project.Models.Interfaces;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Project.ViewModels
{
    internal class ManagerCViewModel : ViewModel, IUpdate
    {
        #region OpenMallCommand
        public ICommand OpenMallCommand { get; }
        private void OnOpenMallCommandExecuted(object parameters) => Singleton.Instance.Navigate(new MallViewingPage());
        private bool CanOpenMallCommandExecute(object parameters) => true;
        #endregion

        #region OpenPavilionCommand
        public ICommand OpenPavilionCommand { get; }
        private void OnOpenPavilionCommandExecuted(object parameters) => Singleton.Instance.Navigate(new PavilionPage());
        private bool CanOpenPavilionCommandExecute(object parameters) => true;
        #endregion

        #region UpdateViewModelCommand
        public ICommand UpdateViewModelCommand { get; }
        private void OnUpdateViewModelCommandExecuted(object parameters) => Update();
        private bool CanUpdateViewModelCommandExecute(object parameters) => true;
        #endregion

        #region Malls
        private ObservableCollection<MallItem> _malls = null;
        public ObservableCollection<MallItem> Malls
        {
            get => _malls;
            set => Set(ref _malls, value);
        }
        #endregion

        #region UpdateMalls
        private void UpdateMalls()
        {
            Malls = new ObservableCollection<MallItem>(
                        from mall in Singleton.Instance.Context.Mall
                        join mall_statuses in Singleton.Instance.Context.Mall_statuses on mall.status_id equals mall_statuses.status_id
                        select new MallItem
                        {
                            mall_id = mall.mall_id,
                            mall_name = mall.mall_name,
                            status_name = mall_statuses.status_name,
                            status_id = mall_statuses.status_id,
                            number_of_pavilion = mall.number_of_pavilion,
                            city = mall.city,
                            cost = mall.cost,
                            number_of_storeys = mall.number_of_storeys,
                            value_added_factor = mall.value_added_factor,
                            photo = mall.photo
                        }
                    );
        }
        #endregion

        #region SelectedItemMall
        private MallItem _selectedItemMall = null;
        public MallItem SelectedItemMall
        {
            get => _selectedItemMall;
            set
            {
                IsEnabledButton = value != null;
                Set(ref _selectedItemMall, value);
            }
        }
        #endregion

        #region IsEnabledButton
        private bool _isEnabledButton = false;
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => Set(ref _isEnabledButton, value);
        }
        #endregion

        #region AddMallCommand
        public ICommand AddMallCommand { get; }
        private void OnAddMallExecuted(object parameters)
        {
            MallItem mall = parameters as MallItem;
            Console.WriteLine("AddMallCommand");
            Console.WriteLine(mall);
        }
        private bool CanAddMallExecute(object parameters) => true;
        #endregion

        #region ChangeMallCommand
        public ICommand ChangeMallCommand { get; }
        private void OnChangeMallExecuted(object parameters)
        {
            MallItem mall = parameters as MallItem;
            Console.WriteLine("ChangeMallCommand");
            Console.WriteLine(mall);
        }
        private bool CanChangeMallExecute(object parameters) => true;
        #endregion

        #region DeleteMallCommand
        public ICommand DeleteMallCommand { get; }
        private void OnDeleteMallExecuted(object parameters)
        {
            MallItem mall = parameters as MallItem;
            Console.WriteLine("DeleteMallCommand");
            Console.WriteLine(mall);
        }
        private bool CanDeleteMallExecute(object parameters) => true;
        #endregion

        #region ViewingMallsCommand
        public ICommand ViewingMallsCommand { get; }
        private void OnViewingMallsExecuted(object parameters)
        {
            MallItem mall = parameters as MallItem;
            Console.WriteLine("ViewingMallsCommand");
            Console.WriteLine(mall);
        }
        private bool CanViewingMallsExecute(object parameters) => true;
        #endregion

        #region MallStatuses
        private ObservableCollection<string> _mallStatuses = null;
        public ObservableCollection<string> MallStatuses
        {
            get => _mallStatuses;
            set => Set(ref _mallStatuses, value);
        }
        #endregion

        #region UpdateStatuses
        private void UpdateStatuses()
        {
            MallStatuses = new ObservableCollection<string>(
                    (from mall_statuses in Singleton.Instance.Context.Mall_statuses
                     select mall_statuses.status_name).AsEnumerable()
                );
        }
        #endregion

        #region SelectedItemMallStatuses
        private string _selectedItemMallStatuses = null;
        public string SelectedItemMallStatuses
        {
            get => _selectedItemMallStatuses;
            set => Set(ref _selectedItemMallStatuses, value);
        }
        #endregion

        #region Cities
        private ObservableCollection<string> _cities = null;
        public ObservableCollection<string> Cities
        {
            get => _cities;
            set => Set(ref _cities, value);
        }
        #endregion

        #region UpdateCities
        private void UpdateCities()
        {
            Cities = new ObservableCollection<string>(
                (
                    from m in Singleton.Instance.Context.Mall
                    orderby m.city
                    where m.status_id != (from ms in Singleton.Instance.Context.Mall_statuses
                                              //where ms.status_name == DeleteNameSorting
                                          select ms.status_id).FirstOrDefault()
                    select m.city
                ).Distinct().ToList()
            )
            {
                "Все"
                //AllNameSorting
            };
        }
        #endregion

        #region SelectedItemCity
        private string _selectedItemCity = null;
        public string SelectedItemCity
        {
            get => _selectedItemCity;
            set => Set(ref _selectedItemCity, value);
        }
        #endregion

        #region Конструктор
        public ManagerCViewModel()
        {
            OpenMallCommand = new LambdaCommand(OnOpenMallCommandExecuted, CanOpenMallCommandExecute);
            OpenPavilionCommand = new LambdaCommand(OnOpenPavilionCommandExecuted, CanOpenPavilionCommandExecute);
            AddMallCommand = new LambdaCommand(OnAddMallExecuted, CanAddMallExecute);
            ChangeMallCommand = new LambdaCommand(OnChangeMallExecuted, CanChangeMallExecute);
            DeleteMallCommand = new LambdaCommand(OnDeleteMallExecuted, CanDeleteMallExecute);
            ViewingMallsCommand = new LambdaCommand(OnViewingMallsExecuted, CanViewingMallsExecute);
            UpdateViewModelCommand = new LambdaCommand(OnUpdateViewModelCommandExecuted, CanUpdateViewModelCommandExecute);
            Update();
        }
        #endregion

        public void Update()
        {
            UpdateMalls();
            UpdateStatuses();
            UpdateCities();
        }
    }
}
