using Microsoft.Win32;
using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Project.Models.Validators.MallValidators;
using System.Collections.ObjectModel;

namespace Project.ViewModels
{
    public enum MallPageTemplateAction
    {
        Add,
        Change
    }

    internal class MallPageTemplateViewModel : UpdatableViewModel
    {
        #region Consts
        private readonly static string AllNameSorting = Application.Current.FindResource("AllNameSorting") as string;
        private readonly static string DeleteNameSorting = Application.Current.FindResource("DeleteNameSorting") as string;
        #endregion

        #region BackCommand
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object parameters) => Singleton.Instance.Navigate(new ViewingMallsPage());
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

        #region SelectedMallStatus
        private string _selectedMallStatus;
        public string SelectedMallStatus
        {
            get => _selectedMallStatus;
            set => Set(ref _selectedMallStatus, value);
        }
        #endregion

        #region CurrentMallActionEntities
        private MallPageTemplateAction _currentMallActionEntities;
        public MallPageTemplateAction CurrentMallActionEntities
        {
            get => _currentMallActionEntities;
            set
            {
                Set(ref _currentMallActionEntities, value);
                ViewingMallsViewModel.Instanse.UpdateStatuses();
                switch (CurrentMallActionEntities)
                {
                    case MallPageTemplateAction.Add:
                        MallPageTemplateButtonName = "Добавить";
                        LoadedMallPhoto = null;
                        CurrentMall = new Mall();
                        break;
                    case MallPageTemplateAction.Change:
                        MallPageTemplateButtonName = "Изменить";
                        CurrentMall = (
                            from m in Singleton.Instance.Context.Mall
                            where m.mall_id == ViewingMallsViewModel.Instanse.SelectedItemMall.mall_id
                            select m
                        ).FirstOrDefault();
                        SelectedMallStatus = ViewingMallsViewModel.Instanse.SelectedItemMall.status_name;
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

        #region MallPageTemplateButtonName
        private string _MallPageTemplateButtonName;
        public string MallPageTemplateButtonName
        {
            get => _MallPageTemplateButtonName;
            set => Set(ref _MallPageTemplateButtonName, value);
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

        #region MallStatuses
        private ObservableCollection<string> _mallStatuses = new ObservableCollection<string>();
        public ObservableCollection<string> MallStatuses
        {
            get => _mallStatuses;
            set => Set(ref _mallStatuses, value);
        }
        #endregion

        #region UpdateMallStatuses
        public void UpdateMallStatuses()
        {
            MallStatuses = new ObservableCollection<string>(
                    from mall_statuses in Singleton.Instance.Context.Mall_statuses
                    where mall_statuses.status_name != DeleteNameSorting
                    select mall_statuses.status_name);
        }
        #endregion

        #region MallPageTemplateExecuteCommand
        public ICommand MallPageTemplateExecuteCommand { get; }
        private bool CanMallPageTemplateExecuteCommandExecute(object parameters) => true;
        private void OnMallPageTemplateExecuteCommandExecuted(object parameters)
        {
            string error = string.Empty;
            MallValidator mallValidator = new MallValidator();
            if (mallValidator.IsNotValid(CurrentMall, out error))
            {
                MessageBox.Show(error);
            }
            else if (SelectedMallStatus == null)
            {
                MessageBox.Show("Статус не выбран");
            }
            else
            {
                try
                {
                    CurrentMall.status_id = (
                        from ms in Singleton.Instance.Context.Mall_statuses
                        where ms.status_name == SelectedMallStatus
                        select ms.status_id
                    ).FirstOrDefault();
                    CurrentMall.mall_name = CurrentMall.mall_name.Trim();
                    CurrentMall.city = CurrentMall.city.Trim();
                    switch (CurrentMallActionEntities)
                    {
                        case MallPageTemplateAction.Add:
                            Singleton.Instance.Context.Mall.Add(CurrentMall);
                            MessageBox.Show($"Торговый центр добавлен.");
                            break;
                        case MallPageTemplateAction.Change:
                            MessageBox.Show($"Торговый центр изменён.");
                            break;
                    }
                    Singleton.Instance.Context.SaveChanges();
                    Singleton.Instance.Navigate(new ViewingMallsPage());
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e}");
                }
                ViewingMallsViewModel.Instanse.SelectedItemMallStatusesSorting = AllNameSorting;
                ViewingMallsViewModel.Instanse.SelectedItemCitySorting = AllNameSorting;
                ViewingMallsViewModel.Instanse.UpdateViewModel();
            }
        }

        public override void UpdateViewModel()
        {
            UpdateMallStatuses();
        }
        #endregion

        #region Instanse
        public static MallPageTemplateViewModel Instanse { get; } = new MallPageTemplateViewModel();
        #endregion

        #region Конструктор
        private MallPageTemplateViewModel()
        {
            BackCommand = new LambdaCommand(OnBackCommandExecuted, CanBackCommandExecute);
            MallPageTemplateExecuteCommand = new LambdaCommand(OnMallPageTemplateExecuteCommandExecuted, CanMallPageTemplateExecuteCommandExecute);
            LoadMallPhotoCommand = new LambdaCommand(OnLoadMallPhotoCommandExecuted, CanLoadMallPhotoCommandExecute);
            UpdateViewModel();
        }
        #endregion
    }
}
