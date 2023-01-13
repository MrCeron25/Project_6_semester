using Project.Models.Interfaces;

namespace Project.ViewModels.Base
{
    internal abstract class UpdatableViewModel : ViewModel, IUpdate
    {
        public virtual void UpdateViewModel() { }
    }
}
