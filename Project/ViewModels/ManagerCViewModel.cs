using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System.Windows.Input;

namespace Project.ViewModels
{
    internal class ManagerCViewModel : ViewModel
    {
        #region OpenMallCommand
        public ICommand OpenMallCommand { get; }
        private void OnOpenMallCommandExecuted(object parameters) => Singleton.Instance.Navigate(new ViewingMallsPage());
        private bool CanOpenMallCommandExecute(object parameters) => true;
        #endregion

        #region OpenPavilionCommand
        public ICommand OpenPavilionCommand { get; }
        private void OnOpenPavilionCommandExecuted(object parameters) => Singleton.Instance.Navigate(new ViewingPavilionsPage());
        private bool CanOpenPavilionCommandExecute(object parameters) => true;
        #endregion

        #region Instanse
        public static ManagerCViewModel Instanse { get; } = new ManagerCViewModel();
        #endregion

        #region Конструктор
        private ManagerCViewModel()
        {
            OpenMallCommand = new LambdaCommand(OnOpenMallCommandExecuted, CanOpenMallCommandExecute);
            OpenPavilionCommand = new LambdaCommand(OnOpenPavilionCommandExecuted, CanOpenPavilionCommandExecute);
        }
        #endregion
    }
}
