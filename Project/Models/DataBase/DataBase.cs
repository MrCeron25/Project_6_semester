using Project.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataBase
{
    internal abstract class DataBase : IDataBase<practiceEntities>
    {
        private practiceEntities _context { get; set; }
        public practiceEntities Context
        {
            get => _context;
            set => _context = value;
        }



        public DataBase(practiceEntities context = null)
        {
            _context = context ?? throw new NotImplementedException();
        }

        public practiceEntities GetData()
        {
            return _context;
        }
    }
}
