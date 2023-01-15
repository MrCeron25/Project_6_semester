using Project.ViewModels.Base;
using System.Windows.Input;

namespace Project.Models.ViewModel
{
    internal abstract class ViewModelActionCommand : UpdatableViewModel
    {
        protected ICommand _addCommand { get; set; }
        public ICommand AddCommand => _addCommand;
        protected abstract void OnAddExecuted(object parameters);
        protected virtual bool CanAddExecute(object parameters) => true;

        protected ICommand _changeCommand { get; set; }
        public ICommand ChangeCommand => _changeCommand;
        protected abstract void OnChangeExecuted(object parameters);
        protected virtual bool CanChangeExecute(object parameters) => true;

        protected ICommand _deleteCommand { get; set; }
        public ICommand DeleteCommand => _deleteCommand;
        protected abstract void OnDeleteExecuted(object parameters);
        protected virtual bool CanDeleteExecute(object parameters) => true;

        protected ICommand _updateViewModelCommand { get; set; }
        public ICommand UpdateViewModelCommand => _updateViewModelCommand;
        protected virtual void OnUpdateViewModelCommandExecuted(object parameters) => UpdateViewModel();
        protected virtual bool CanUpdateViewModelCommandExecute(object parameters) => true;
    }
}
