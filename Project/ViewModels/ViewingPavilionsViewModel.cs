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
    internal class ViewingPavilionsViewModel : ViewModelActionCommand
    {
        #region Consts
        private readonly static string DeleteNameSorting = Application.Current.FindResource("DeleteNameSorting") as string;
        private readonly static string AllNameSorting = Application.Current.FindResource("AllNameSorting") as string;
        private readonly static int DefaultFloor = 4;
        private readonly static decimal DefaultMinSquare = 0;
        private readonly static decimal DefaultMaxSquare = 1000;
        #endregion

        #region SelectedItemMall 
        private MallItem _selectedItemMall;
        public MallItem SelectedItemMall
        {
            get => _selectedItemMall;
            set
            {
                Set(ref _selectedItemMall, value);
                UpdatePavilions();
            }
        }
        #endregion

        #region SelectedItemMall 
        private PavilionItem _selectedItemPavilion;
        public PavilionItem SelectedItemPavilion
        {
            get => _selectedItemPavilion;
            set => Set(ref _selectedItemPavilion, value);
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

        #region Pavilions
        private ObservableCollection<PavilionItem> _pavilions = null;
        public ObservableCollection<PavilionItem> Pavilions
        {
            get => _pavilions;
            set => Set(ref _pavilions, value);
        }
        #endregion

        #region MinSquare
        private decimal _minSquare = DefaultMinSquare;
        public decimal MinSquare
        {
            get => _minSquare;
            set
            {
                if (value >= 0)
                {
                    if (value <= MaxSquare)
                    {
                        Set(ref _minSquare, value);
                    }
                    else
                    {
                        Set(ref _minSquare, MaxSquare);
                    }
                    UpdatePavilions();
                }
            }
        }
        #endregion

        #region MaxSquare
        private decimal _maxSquare = DefaultMaxSquare;
        public decimal MaxSquare
        {
            get => _maxSquare;
            set
            {
                if (value >= 0)
                {
                    if (value >= MinSquare)
                    {
                        Set(ref _maxSquare, value);
                    }
                    else
                    {
                        Set(ref _maxSquare, MinSquare);
                    }
                    UpdatePavilions();
                }
            }
        }
        #endregion

        #region Floor
        private int _floor = DefaultFloor;
        public int Floor
        {
            get => _floor;
            set
            {
                if (value >= 0)
                {
                    Set(ref _floor, value);
                    UpdatePavilions();
                }
            }
        }
        #endregion

        #region SelectedPavilionItemStatusSorting
        private string _selectedPavilionItemStatusSorting;
        public string SelectedPavilionItemStatusSorting
        {
            get => _selectedPavilionItemStatusSorting;
            set
            {
                Set(ref _selectedPavilionItemStatusSorting, value);
                UpdatePavilions();
            }
        }
        #endregion

        #region UpdatePavilions
        private void UpdatePavilions()
        {
            ObservableCollection<PavilionItem> data = new ObservableCollection<PavilionItem>();
            if (SelectedItemMall == null)
            {
                Pavilions = null;
            }
            else if (SelectedPavilionItemStatusSorting == AllNameSorting)
            {
                data = new ObservableCollection<PavilionItem>(
                from p in Singleton.Instance.Context.Pavilion
                join ps in Singleton.Instance.Context.Pavilion_statuses on p.status_id equals ps.status_id
                join m in Singleton.Instance.Context.Mall on p.mall_id equals m.mall_id
                join ms in Singleton.Instance.Context.Mall_statuses on m.status_id equals ms.status_id
                where ps.status_name != DeleteNameSorting &&
                      p.floor == Floor &&
                      p.square >= MinSquare &&
                      p.square <= MaxSquare &&
                      m.mall_name == SelectedItemMall.mall_name
                select new PavilionItem
                {
                    mall_id = m.mall_id,
                    mall_status_id = m.status_id,
                    mall_status_name = ms.status_name,
                    mall_name = m.mall_name,
                    floor = p.floor,
                    pavilion_number = p.pavilion_number,
                    square = p.square,
                    pavilion_status_id = p.status_id,
                    pavilion_status_name = ps.status_name,
                    cost_per_square_meter = p.cost_per_square_meter,
                    value_added_factor = p.value_added_factor
                });
            }
            else
            {
                data = new ObservableCollection<PavilionItem>(
                    from p in Singleton.Instance.Context.Pavilion
                    join ps in Singleton.Instance.Context.Pavilion_statuses on p.status_id equals ps.status_id
                    join m in Singleton.Instance.Context.Mall on p.mall_id equals m.mall_id
                    join ms in Singleton.Instance.Context.Mall_statuses on m.status_id equals ms.status_id
                    where ps.status_name != DeleteNameSorting &&
                          ps.status_name == SelectedPavilionItemStatusSorting &&
                          p.floor == Floor &&
                          p.square >= MinSquare &&
                          p.square <= MaxSquare &&
                          m.mall_name == SelectedItemMall.mall_name
                    select new PavilionItem
                    {
                        mall_id = m.mall_id,
                        mall_status_id = m.status_id,
                        mall_status_name = ms.status_name,
                        mall_name = m.mall_name,
                        floor = p.floor,
                        pavilion_number = p.pavilion_number,
                        square = p.square,
                        pavilion_status_id = p.status_id,
                        pavilion_status_name = ps.status_name,
                        cost_per_square_meter = p.cost_per_square_meter,
                        value_added_factor = p.value_added_factor
                    });
            }
            Pavilions = data;
        }
        #endregion

        #region UpdatePavilions
        public void UpdatePavilionStatusesSorting()
        {
            PavilionStatusesSorting = new ObservableCollection<string>(
                from p in Singleton.Instance.Context.Pavilion_statuses
                where p.status_name != DeleteNameSorting
                orderby p.status_name
                select p.status_name
            )
            { AllNameSorting };
            SelectedPavilionItemStatusSorting = AllNameSorting;
        }
        #endregion

        #region UpdateUIFields
        private void UpdateUIFields()
        {
            _floor = DefaultFloor;
            _minSquare = DefaultMinSquare;
            _maxSquare = DefaultMaxSquare;
        }
        #endregion

        #region UpdateViewModel
        public override void UpdateViewModel()
        {
            UpdatePavilions();
            UpdatePavilionStatusesSorting();
            UpdateUIFields();
        }
        #endregion

        protected override void OnAddExecuted(object parameters)
        {
            Console.WriteLine("AddPavilionCommand");

            PavilionPageTemplateViewModel.Instanse.CurrentPavilionActionEntities = PavilionPageTemplateAction.Add;
            Singleton.Instance.Navigate(new PavilionPageTemplate());
        }

        protected override void OnChangeExecuted(object parameters)
        {
            Console.WriteLine(PavilionPageTemplateViewModel.Instanse.CurrentPavilionActionEntities);

            Console.WriteLine("ChangePavilionCommand");
            PavilionItem pavilion = parameters as PavilionItem;
            if (pavilion != null)
            {
                Console.WriteLine(pavilion);
                SelectedItemPavilion = pavilion;
            }
            PavilionPageTemplateViewModel.Instanse.CurrentPavilionActionEntities = PavilionPageTemplateAction.Change;
            Singleton.Instance.Navigate(new PavilionPageTemplate());
        }

        protected override void OnDeleteExecuted(object parameters)
        {
            Console.WriteLine("DeletePavilionCommand");
            PavilionItem pavilionParameter = parameters as PavilionItem;
            Console.WriteLine(pavilionParameter);
            try
            {
                Pavilion pavilion = (
                    from p in Singleton.Instance.Context.Pavilion
                    where p.mall_id == pavilionParameter.mall_id &&
                          p.pavilion_number == pavilionParameter.pavilion_number
                    select p
                ).FirstOrDefault();
                pavilion.status_id = (
                    from m in Singleton.Instance.Context.Pavilion_statuses
                    where m.status_name == DeleteNameSorting
                    select m.status_id
                ).FirstOrDefault();
                Singleton.Instance.Context.SaveChanges();
                UpdatePavilions();
                MessageBox.Show($"Павильон '{pavilion.pavilion_number}' торгового центра {pavilionParameter.mall_name} удалён.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }

        #region RentPavilionCommand
        public ICommand RentPavilionCommand { get; }
        private void OnRentPavilionExecuted(object parameters)
        {
            Console.WriteLine("RentPavilionCommand");
            PavilionItem pavilion = parameters as PavilionItem;
            Console.WriteLine(pavilion);

        }
        private bool CanRentPavilionExecute(object parameters) => true;
        #endregion

        #region Instanse
        public static ViewingPavilionsViewModel Instanse { get; } = new ViewingPavilionsViewModel();
        #endregion

        #region Конструктор
        private ViewingPavilionsViewModel()
        {
            _addCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            _changeCommand = new LambdaCommand(OnChangeExecuted, CanChangeExecute);
            _deleteCommand = new LambdaCommand(OnDeleteExecuted, CanDeleteExecute);
            _updateViewModelCommand = new LambdaCommand(OnUpdateViewModelCommandExecuted, CanUpdateViewModelCommandExecute);

            RentPavilionCommand = new LambdaCommand(OnRentPavilionExecuted, CanRentPavilionExecute);

            UpdateViewModel();
        }
        #endregion
    }
}
