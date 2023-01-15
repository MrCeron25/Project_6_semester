using Project.Infrastructure.Commands;
using Project.Models;
using Project.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Project.Models.ViewModel;

namespace Project.ViewModels
{
    internal class ViewingMallsViewModel : ViewModelActionCommand
    {
        #region Consts
        private readonly static string DeleteNameSorting = "Удалён";
        public readonly static string AllNameSorting = "Всё";
        #endregion

        //#region UpdateViewModelCommand
        //public ICommand UpdateViewModelCommand { get; }
        //private void OnUpdateViewModelCommandExecuted(object parameters) => UpdateViewModel();
        //private bool CanUpdateViewModelCommandExecute(object parameters) => true;
        //#endregion

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
            ObservableCollection<MallItem> data = new ObservableCollection<MallItem>();
            if (SelectedItemMallStatusesSorting == AllNameSorting)
            {
                data = (SelectedItemCitySorting == AllNameSorting)
                    ? new ObservableCollection<MallItem>(
                    from mall in Singleton.Instance.Context.Mall
                    join ms in Singleton.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                    orderby mall.city, ms.status_name
                    where ms.status_name != DeleteNameSorting
                    select new MallItem
                    {
                        mall_id = mall.mall_id,
                        mall_name = mall.mall_name,
                        status_name = ms.status_name,
                        status_id = ms.status_id,
                        number_of_pavilion = mall.number_of_pavilion,
                        city = mall.city,
                        cost = mall.cost,
                        number_of_storeys = mall.number_of_storeys,
                        value_added_factor = mall.value_added_factor,
                        photo = mall.photo
                    })
                    :
                    new ObservableCollection<MallItem>(
                    from mall in Singleton.Instance.Context.Mall
                    join ms in Singleton.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                    orderby mall.city, ms.status_name
                    where ms.status_name != DeleteNameSorting &&
                          mall.city == SelectedItemCitySorting
                    select new MallItem
                    {
                        mall_id = mall.mall_id,
                        mall_name = mall.mall_name,
                        status_name = ms.status_name,
                        status_id = ms.status_id,
                        number_of_pavilion = mall.number_of_pavilion,
                        city = mall.city,
                        cost = mall.cost,
                        number_of_storeys = mall.number_of_storeys,
                        value_added_factor = mall.value_added_factor,
                        photo = mall.photo
                    });
            }
            else
            {
                data = (SelectedItemCitySorting == AllNameSorting)
                    ? new ObservableCollection<MallItem>(
                        from mall in Singleton.Instance.Context.Mall
                        join ms in Singleton.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                        orderby mall.city, ms.status_name
                        where ms.status_name == SelectedItemMallStatusesSorting
                        select new MallItem
                        {
                            mall_id = mall.mall_id,
                            mall_name = mall.mall_name,
                            status_name = ms.status_name,
                            status_id = ms.status_id,
                            number_of_pavilion = mall.number_of_pavilion,
                            city = mall.city,
                            cost = mall.cost,
                            number_of_storeys = mall.number_of_storeys,
                            value_added_factor = mall.value_added_factor,
                            photo = mall.photo
                        }
                    )
                    :
                    new ObservableCollection<MallItem>(
                        from mall in Singleton.Instance.Context.Mall
                        join ms in Singleton.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                        orderby mall.city, ms.status_name
                        where ms.status_name == SelectedItemMallStatusesSorting &&
                              mall.city == SelectedItemCitySorting
                        select new MallItem
                        {
                            mall_id = mall.mall_id,
                            mall_name = mall.mall_name,
                            status_name = ms.status_name,
                            status_id = ms.status_id,
                            number_of_pavilion = mall.number_of_pavilion,
                            city = mall.city,
                            cost = mall.cost,
                            number_of_storeys = mall.number_of_storeys,
                            value_added_factor = mall.value_added_factor,
                            photo = mall.photo
                        }
                    );
            }
            Malls = data;
        }
        #endregion

        #region SelectedItemMall
        private MallItem _selectedItemMall = null;
        public MallItem SelectedItemMall
        {
            get => _selectedItemMall;
            set => Set(ref _selectedItemMall, value);
        }
        #endregion

        #region ViewingPavilionsCommand
        public ICommand ViewingPavilionsCommand { get; }
        private void OnViewingPavilionsExecuted(object parameters)
        {
            Console.WriteLine("ViewingPavilionsCommand");
            MallItem mall = parameters as MallItem;
            Console.WriteLine(mall);
            ViewingPavilionsViewModel.Instanse.SelectedItemMall = mall;
            Singleton.Instance.Navigate(new ViewingPavilionsPage());
        }
        private bool CanViewingPavilionsExecute(object parameters) => true;
        #endregion

        #region MallStatuses
        private ObservableCollection<string> _mallStatuses = new ObservableCollection<string>();
        public ObservableCollection<string> MallStatuses
        {
            get => _mallStatuses;
            set
            {
                Set(ref _mallStatuses, value);
                UpdateMalls();
            }
        }
        #endregion

        #region UpdateStatuses
        public void UpdateStatuses()
        {
            MallStatuses = new ObservableCollection<string>((
                    from mall_statuses in Singleton.Instance.Context.Mall_statuses
                    where mall_statuses.status_name != DeleteNameSorting
                    select mall_statuses.status_name).AsEnumerable())
            { AllNameSorting };
        }
        #endregion

        #region SelectedItemMallStatusesSorting
        private string _selectedItemMallStatusesSorting = AllNameSorting;
        public string SelectedItemMallStatusesSorting
        {
            get => _selectedItemMallStatusesSorting;
            set
            {
                Set(ref _selectedItemMallStatusesSorting, value);
                UpdateMalls();
            }
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
            Cities = new ObservableCollection<string>((
                    from m in Singleton.Instance.Context.Mall
                    orderby m.city
                    where m.status_id != (from ms in Singleton.Instance.Context.Mall_statuses
                                          where ms.status_name == DeleteNameSorting
                                          select ms.status_id).FirstOrDefault()
                    select m.city
                ).Distinct().ToList()
            )
            {
                AllNameSorting
            };
        }
        #endregion

        #region SelectedItemCitySorting
        private string _selectedItemCitySorting = AllNameSorting;
        public string SelectedItemCitySorting
        {
            get => _selectedItemCitySorting;
            set
            {
                Set(ref _selectedItemCitySorting, value);
                UpdateMalls();
            }
        }
        #endregion

        #region OnAddExecuted
        protected override void OnAddExecuted(object parameters)
        {
            Console.WriteLine("AddMallCommand");
            MallPageTemplateViewModel.Instanse.CurrentMallActionEntities = MallPageTemplateAction.Add;
            Singleton.Instance.Navigate(new MallPageTemplate());
        }
        #endregion

        #region OnChangeExecuted
        protected override void OnChangeExecuted(object parameters)
        {
            Console.WriteLine("ChangeMallCommand");
            MallItem mall = parameters as MallItem;
            if (mall != null)
            {
                Console.WriteLine(mall);
                SelectedItemMall = mall;
            }
            MallPageTemplateViewModel.Instanse.CurrentMallActionEntities = MallPageTemplateAction.Change;
            Singleton.Instance.Navigate(new MallPageTemplate());
        }
        #endregion

        #region OnDeleteExecuted
        protected override void OnDeleteExecuted(object parameters)
        {
            Console.WriteLine("DeleteMallCommand");
            MallItem mallParameter = parameters as MallItem;
            Console.WriteLine(mallParameter);
            try
            {
                Mall mall = (
                    from m in Singleton.Instance.Context.Mall
                    where m.mall_id == mallParameter.mall_id
                    select m
                ).FirstOrDefault();
                mall.status_id = (
                    from m in Singleton.Instance.Context.Mall_statuses
                    where m.status_name == DeleteNameSorting
                    select m.status_id
                ).FirstOrDefault();
                Singleton.Instance.Context.SaveChanges();
                UpdateMalls();
                MessageBox.Show($"Торговый центр '{mall.mall_name}' удалён.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion

        #region Instanse
        public static ViewingMallsViewModel Instanse { get; } = new ViewingMallsViewModel();
        #endregion

        #region Конструктор
        private ViewingMallsViewModel()
        {
            _addCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            _changeCommand = new LambdaCommand(OnChangeExecuted, CanChangeExecute);
            _deleteCommand = new LambdaCommand(OnDeleteExecuted, CanDeleteExecute);
            ViewingPavilionsCommand = new LambdaCommand(OnViewingPavilionsExecuted, CanViewingPavilionsExecute);
            _updateViewModelCommand = new LambdaCommand(OnUpdateViewModelCommandExecuted, CanUpdateViewModelCommandExecute);
            UpdateViewModel();
        }
        #endregion

        #region UpdateViewModel
        public override void UpdateViewModel()
        {
            UpdateMalls();
            UpdateStatuses();
            UpdateCities();
        }
        #endregion
    }
}
