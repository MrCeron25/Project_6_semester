using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Repository
{
    internal abstract class BaseRepository<T>
    {
        public abstract string Data();
    }
}
