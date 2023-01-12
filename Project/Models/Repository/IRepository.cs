using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Repository
{
    internal interface IRepository<T>
    {
        ObservableCollection<T> Data();
    }
}
