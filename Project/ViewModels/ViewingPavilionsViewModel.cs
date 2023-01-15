using Project.Models;
using Project.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    internal class ViewingPavilionsViewModel : ViewModelActionCommand
    {
        #region SelectedItemMall
        private MallItem _selectedItemMall;
        public MallItem SelectedItemMall
        {
            get => _selectedItemMall;
            set => Set(ref _selectedItemMall, value);
        }
        #endregion


        public override void UpdateViewModel()
        {
            
            throw new NotImplementedException();
        }

        protected override void OnAddExecuted(object parameters)
        {
            throw new NotImplementedException();
        }

        protected override void OnChangeExecuted(object parameters)
        {
            throw new NotImplementedException();
        }

        protected override void OnDeleteExecuted(object parameters)
        {
            throw new NotImplementedException();
        }

        #region Instanse
        public static ViewingPavilionsViewModel Instanse { get; } = new ViewingPavilionsViewModel();
        #endregion

        #region Конструктор
        private ViewingPavilionsViewModel()
        {

        }
        #endregion
    }
}
