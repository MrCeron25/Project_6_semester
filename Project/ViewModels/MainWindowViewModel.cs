using Project.Models;
using Project.Properties;
using Project.ViewModels.Base;
using System.Windows.Media.Imaging;

namespace Project.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок
        private string _tiile = "KingIT";
        public string Title
        {
            get => _tiile;
            set => Set(ref _tiile, value);
        }
        #endregion

        #region Icon
        private BitmapImage _icon = Tools.BitmapToBitmapImage(Resources.Icon);
        public BitmapImage Icon
        {
            get => _icon;
            set => Set(ref _icon, value);
        }
        #endregion
    }
}
