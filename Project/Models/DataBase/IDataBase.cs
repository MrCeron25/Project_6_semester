﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Interfaces
{
    internal interface IDataBase <T>
    {
        T GetData();
    }
}