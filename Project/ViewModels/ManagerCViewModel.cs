using Project.Models;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System.Windows.Input;

namespace Project.ViewModels
{
    internal class ManagerCViewModel : UpdatableViewModel
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

        //public ViewingMallViewModel ViewingMallViewModel { get; set; } = new ViewingMallViewModel();
        ////public ViewingMallViewModel ViewingMallViewModel { get; set; } = new ViewingMallViewModel();


        //public List<UpdatableViewModel> UpdatableViewModels { get; set; } = new List<UpdatableViewModel>();


        #region Конструктор
        public ManagerCViewModel()
        {
            //UpdatableViewModels.Add(ViewingMallViewModel);
            //UpdateViewModel();
        }
        #endregion

        //#region UpdateViewModel
        //public override void UpdateViewModel()
        //{
        //    foreach (UpdatableViewModel viewModel in UpdatableViewModels)
        //    {
        //        viewModel.UpdateViewModel();
        //    }
        //}
        //#endregion
    }
}
