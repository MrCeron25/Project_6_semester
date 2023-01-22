using Project.Infrastructure.Commands;
using Project.Models;
using Project.Models.Validators.PavilionValidators;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModels
{
    public enum PavilionPageTemplateAction
    {
        Add,
        Change
    }

    internal class PavilionPageTemplateViewModel : UpdatableViewModel
    {
        #region Consts
        private readonly static string DeleteNameSorting = Application.Current.FindResource("DeleteNameSorting") as string;
        #endregion

        private readonly static Pavilion DefaultPavilion = new Pavilion()
        {
            square = PavilionSquareValidator.MinSquare,
            cost_per_square_meter = PavilionCostPerSquareMeterValidator.MinCostPerSquareMeter,
            value_added_factor = PavilionValueAddedFactorValidator.MinValueAddedFactor
        };

        #region BackCommand
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object parameters) => Singleton.Instance.Navigate(new ViewingPavilionsPage());
        private bool CanBackCommandExecute(object parameters) => true;
        #endregion

        #region CurrentPavilion
        private Pavilion _currentPavilion;
        public Pavilion CurrentPavilion
        {
            get => _currentPavilion;
            set => Set(ref _currentPavilion, value);
        }
        #endregion

        #region CurrentPavilionNumber
        private string _currentPavilionNumber;
        public string CurrentPavilionNumber
        {
            get => _currentPavilionNumber;
            set => Set(ref _currentPavilionNumber, value);
        }
        #endregion

        #region PavilionStatus
        private string _selectedItemPavilionStatusSorting;
        public string SelectedItemPavilionStatusSorting
        {
            get => _selectedItemPavilionStatusSorting;
            set => Set(ref _selectedItemPavilionStatusSorting, value);
        }
        #endregion

        #region PavilionStatusesSorting
        private ObservableCollection<string> _pavilionStatusesSorting;
        public ObservableCollection<string> PavilionStatusesSorting
        {
            get => _pavilionStatusesSorting;
            set => Set(ref _pavilionStatusesSorting, value);
        }
        #endregion

        #region UpdatePavilionStatusesSorting
        public void UpdatePavilionStatusesSorting()
        {
            PavilionStatusesSorting = new ObservableCollection<string>(
                from p in Singleton.Instance.Context.Pavilion_statuses
                where p.status_name != DeleteNameSorting
                orderby p.status_name
                select p.status_name
            );
        }
        #endregion

        #region CurrentPavilionActionEntities
        private PavilionPageTemplateAction _currentPavilionActionEntities;
        public PavilionPageTemplateAction CurrentPavilionActionEntities
        {
            get => _currentPavilionActionEntities;
            set
            {
                Set(ref _currentPavilionActionEntities, value);
                switch (CurrentPavilionActionEntities)
                {
                    case PavilionPageTemplateAction.Add:
                        PavilionPageTemplateButtonName = "Добавить";
                        CurrentPavilion = DefaultPavilion;
                        break;
                    case PavilionPageTemplateAction.Change:
                        PavilionPageTemplateButtonName = "Изменить";
                        CurrentPavilion = (
                            from p in Singleton.Instance.Context.Pavilion
                            where p.mall_id == ViewingPavilionsViewModel.Instanse.SelectedItemPavilion.mall_id &&
                                  p.pavilion_number == ViewingPavilionsViewModel.Instanse.SelectedItemPavilion.pavilion_number
                            select p
                        ).FirstOrDefault();
                        CurrentPavilionNumber = CurrentPavilion.pavilion_number;
                        SelectedItemPavilionStatusSorting = ViewingPavilionsViewModel.Instanse.SelectedItemPavilion.pavilion_status_name;
                        break;
                }
            }
        }
        #endregion

        #region PavilionPageTemplateButtonName
        private string _pavilionPageTemplateButtonName;
        public string PavilionPageTemplateButtonName
        {
            get => _pavilionPageTemplateButtonName;
            set => Set(ref _pavilionPageTemplateButtonName, value);
        }
        #endregion

        #region PavilionPageTemplateExecuteCommand
        public ICommand PavilionPageTemplateExecuteCommand { get; }
        private bool CanPavilionPageTemplateExecuteCommandExecute(object parameters) => true;
        private void OnPavilionPageTemplateExecuteCommandExecuted(object parameters)
        {
            string error = string.Empty;
            PavilionValidator pavilionValidator = new PavilionValidator();
            if (pavilionValidator.IsNotValid(CurrentPavilion, out error))
            {
                MessageBox.Show(error);
            }
            else if (SelectedItemPavilionStatusSorting == null)
            {
                MessageBox.Show("Статус не выбран.");
            }
            else
            {
                try
                {
                    CurrentPavilion.status_id = (
                        from ps in Singleton.Instance.Context.Pavilion_statuses
                        where ps.status_name == SelectedItemPavilionStatusSorting
                        select ps.status_id
                    ).FirstOrDefault();
                    CurrentPavilion.pavilion_number = CurrentPavilion.pavilion_number.Trim();
                    switch (CurrentPavilionActionEntities)
                    {
                        case PavilionPageTemplateAction.Add:
                            CurrentPavilion.mall_id = ViewingPavilionsViewModel.Instanse.SelectedItemMall.mall_id;
                            Singleton.Instance.Context.Pavilion.Add(CurrentPavilion);
                            break;
                        case PavilionPageTemplateAction.Change:
                            // нельзя поменять pavilion_number т.к. есть записи в таблице Rent
                            //if (CurrentPavilionNumber != CurrentPavilion.pavilion_number)
                            //{
                            //    string bufPavilion_number = CurrentPavilion.pavilion_number;
                            //    CurrentPavilion.pavilion_number = CurrentPavilionNumber;
                            //    Singleton.Instance.Context.Pavilion.Remove(CurrentPavilion);
                            //    CurrentPavilion.pavilion_number = bufPavilion_number;
                            //    Singleton.Instance.Context.Pavilion.Add(CurrentPavilion);
                            //}
                            break;
                    }
                    Singleton.Instance.Context.SaveChanges();
                    switch (CurrentPavilionActionEntities)
                    {
                        case PavilionPageTemplateAction.Add:
                            MessageBox.Show($"Павильон добавлен.");
                            break;
                        case PavilionPageTemplateAction.Change:
                            MessageBox.Show($"Павильон изменён.");
                            break;
                    }
                    ViewingPavilionsViewModel.Instanse.UpdateViewModel();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e}");
                }
                Singleton.Instance.Navigate(new ViewingPavilionsPage());
            }
        }
        #endregion

        #region Instanse
        public static PavilionPageTemplateViewModel Instanse { get; } = new PavilionPageTemplateViewModel();
        #endregion

        #region Конструктор
        private PavilionPageTemplateViewModel()
        {
            BackCommand = new LambdaCommand(OnBackCommandExecuted, CanBackCommandExecute);
            PavilionPageTemplateExecuteCommand = new LambdaCommand(OnPavilionPageTemplateExecuteCommandExecuted, CanPavilionPageTemplateExecuteCommandExecute);

            UpdateViewModel();
        }
        #endregion

        #region UpdateViewModel
        public override void UpdateViewModel()
        {
            UpdatePavilionStatusesSorting();
        }
        #endregion
    }
}