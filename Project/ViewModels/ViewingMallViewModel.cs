using Microsoft.Win32;
using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Project.Models.Validators.MallValidators;

namespace Project.ViewModels
{
    public enum ViewingMallAction
    {
        None,
        Add,
        Change
    }

    //public static class LINQExtensions
    //{
    //    public static IQueryable<T> If<T>(
    //        this IQueryable<T> query,
    //        bool should,
    //        params Func<IQueryable<T>, IQueryable<T>>[] transforms)
    //    {
    //        return should
    //            ? transforms.Aggregate(query,
    //                (current, transform) => transform.Invoke(current))
    //            : query;
    //    }

    //    public static IEnumerable<T> If<T>(
    //        this IEnumerable<T> query,
    //        bool should,
    //        params Func<IEnumerable<T>, IEnumerable<T>>[] transforms)
    //    {
    //        return should
    //            ? transforms.Aggregate(query,
    //                (current, transform) => transform.Invoke(current))
    //            : query;
    //    }
    //}

    internal class ViewingMallViewModel : UpdatableViewModel
    {

        #region MallViewingPage

        #region Consts
        private readonly static string DeleteNameSorting = "Удалён";
        private readonly static string AllNameSorting = "Всё";
        #endregion

        #region UpdateViewModelCommand
        public ICommand UpdateViewModelCommand { get; }
        private void OnUpdateViewModelCommandExecuted(object parameters) => UpdateViewModel();
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
            //Expression<Func<MallItem, bool>> whereClause = _ => SelectedItemCity == AllNameSorting;
            //var query = Singleton.Instance.Context.Mall.AsEnumerable()
            //           .Join(
            //                Singleton.Instance.Context.Mall_statuses.AsEnumerable(),
            //                mall => mall.status_id,
            //                ms => ms.status_id,
            //                (mall, ms) => new MallItem
            //                {
            //                    mall_id = mall.mall_id,
            //                    mall_name = mall.mall_name,
            //                    status_name = ms.status_name,
            //                    status_id = ms.status_id,
            //                    number_of_pavilion = mall.number_of_pavilion,
            //                    city = mall.city,
            //                    cost = mall.cost,
            //                    number_of_storeys = mall.number_of_storeys,
            //                    value_added_factor = mall.value_added_factor,
            //                    photo = mall.photo
            //                }
            //            ).OrderBy(
            //                m => new { m.city, m.status_name }
            //            ).If(
            //                SelectedItemMallStatusesSorting == AllNameSorting,
            //                m => (m.) ? false  : true
            //            )
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

        #region AddMallCommand
        public ICommand AddMallCommand { get; }
        private void OnAddMallExecuted(object parameters)
        {
            Console.WriteLine("AddMallCommand");
            CurrentMallActionEntities = ViewingMallAction.Add;
            Singleton.Instance.Navigate(new MallPage());
        }
        private bool CanAddMallExecute(object parameters) => true;
        #endregion

        #region ChangeMallCommand
        public ICommand ChangeMallCommand { get; }
        private void OnChangeMallExecuted(object parameters)
        {
            Console.WriteLine("ChangeMallCommand");
            MallItem mall = parameters as MallItem;
            if (mall != null)
            {
                Console.WriteLine(mall);
                SelectedItemMall = mall;
            }
            CurrentMallActionEntities = ViewingMallAction.Change;
            Singleton.Instance.Navigate(new MallPage());
        }
        private bool CanChangeMallExecute(object parameters) => true;
        #endregion

        #region DeleteMallCommand
        public ICommand DeleteMallCommand { get; }
        private void OnDeleteMallExecuted(object parameters)
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
            set
            {
                Set(ref _mallStatuses, value);
                UpdateMalls();
            }
        }
        #endregion

        #region UpdateStatuses
        private void UpdateStatuses()
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

        #endregion

        #region MallPage

        #region BackCommand
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object parameters) => Singleton.Instance.Navigate(new MallViewingPage());
        private bool CanBackCommandExecute(object parameters) => true;
        #endregion

        #region CurrentMall
        private Mall _currentMall = new Mall();
        public Mall CurrentMall
        {
            get => _currentMall;
            set => Set(ref _currentMall, value);
        }
        #endregion

        #region MallPageSelectedMallStatus
        private string _mallPageSelectedMallStatus;
        public string MallPageSelectedMallStatus
        {
            get => _mallPageSelectedMallStatus;
            set => Set(ref _mallPageSelectedMallStatus, value);
        }
        #endregion


        #region CurrentMallActionEntities
        private ViewingMallAction _currentMallActionEntities = ViewingMallAction.None;
        public ViewingMallAction CurrentMallActionEntities
        {
            get => _currentMallActionEntities;
            set
            {
                Set(ref _currentMallActionEntities, value);
                UpdateStatuses();
                switch (CurrentMallActionEntities)
                {
                    case ViewingMallAction.Add:
                        MallPageButtonName = "Добавить";
                        LoadedMallPhoto = null;
                        CurrentMall = new Mall();
                        break;
                    case ViewingMallAction.Change:
                        MallPageButtonName = "Изменить";
                        CurrentMall = (
                            from m in Singleton.Instance.Context.Mall
                            where m.mall_id == SelectedItemMall.mall_id
                            select m
                        ).FirstOrDefault();
                        MallPageSelectedMallStatus = SelectedItemMall.status_name;
                        if (CurrentMall.photo != null)
                        {
                            LoadedMallPhoto = Tools.BytesToImage(CurrentMall.photo);
                        }
                        break;
                }
            }
        }
        #endregion

        #region LoadedMallPhoto
        private BitmapImage _loadedMallPhoto;
        public BitmapImage LoadedMallPhoto
        {
            get => _loadedMallPhoto;
            set => Set(ref _loadedMallPhoto, value);
        }
        #endregion

        #region MallPageButtonName
        private string _mallPageButtonName;
        public string MallPageButtonName
        {
            get => _mallPageButtonName;
            set => Set(ref _mallPageButtonName, value);
        }
        #endregion

        #region LoadMallPhotoCommand
        public ICommand LoadMallPhotoCommand { get; }
        private bool CanLoadMallPhotoCommandExecute(object parameters) => true;
        private void OnLoadMallPhotoCommandExecuted(object parameters)
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
                        CurrentMall.photo = Tools.GetImageBytes(fileDialog.FileName);
                        LoadedMallPhoto = new BitmapImage(new Uri(fileDialog.FileName));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region MallPageExecuteCommand
        public ICommand MallPageExecuteCommand { get; }
        private bool CanMallPageExecuteCommandExecute(object parameters) => true;
        private void OnMallPageExecuteCommandExecuted(object parameters)
        {
            string error = string.Empty;
            MallValidator mallValidator = new MallValidator();
            if (mallValidator.IsNotValid(CurrentMall, out error))
            {
                MessageBox.Show(error);
            }
            else if (MallPageSelectedMallStatus == null)
            {
                MessageBox.Show("Статус не выбран");
            }
            else
            {
                try
                {
                    CurrentMall.status_id = (
                        from ms in Singleton.Instance.Context.Mall_statuses
                        where ms.status_name == MallPageSelectedMallStatus
                        select ms.status_id
                    ).FirstOrDefault();
                    CurrentMall.mall_name = CurrentMall.mall_name.Trim();
                    CurrentMall.city = CurrentMall.city.Trim();
                    switch (CurrentMallActionEntities)
                    {
                        case ViewingMallAction.Add:
                            Singleton.Instance.Context.Mall.Add(CurrentMall);
                            MessageBox.Show($"Торговый центр добавлен.");
                            break;
                        case ViewingMallAction.Change:
                            MessageBox.Show($"Торговый центр изменён.");
                            break;
                    }
                    Singleton.Instance.Context.SaveChanges();
                    Singleton.Instance.Navigate(new MallViewingPage());
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                SelectedItemMallStatusesSorting = AllNameSorting;
                SelectedItemCitySorting = AllNameSorting;
                UpdateViewModel();
            }
        }
        #endregion

        #endregion

        #region Instanse
        public static ViewingMallViewModel Instanse { get; } = new ViewingMallViewModel();
        #endregion

        #region Конструктор
        private ViewingMallViewModel()
        {
            #region MallViewingPage
            AddMallCommand = new LambdaCommand(OnAddMallExecuted, CanAddMallExecute);
            ChangeMallCommand = new LambdaCommand(OnChangeMallExecuted, CanChangeMallExecute);
            DeleteMallCommand = new LambdaCommand(OnDeleteMallExecuted, CanDeleteMallExecute);
            ViewingMallsCommand = new LambdaCommand(OnViewingMallsExecuted, CanViewingMallsExecute);
            UpdateViewModelCommand = new LambdaCommand(OnUpdateViewModelCommandExecuted, CanUpdateViewModelCommandExecute);
            #endregion

            #region MallPage
            BackCommand = new LambdaCommand(OnBackCommandExecuted, CanBackCommandExecute);
            MallPageExecuteCommand = new LambdaCommand(OnMallPageExecuteCommandExecuted, CanMallPageExecuteCommandExecute);
            LoadMallPhotoCommand = new LambdaCommand(OnLoadMallPhotoCommandExecuted, CanLoadMallPhotoCommandExecute);
            #endregion

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
