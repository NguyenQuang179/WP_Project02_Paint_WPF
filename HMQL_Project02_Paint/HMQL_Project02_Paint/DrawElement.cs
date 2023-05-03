using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IContract;

using static HMQL_Project02_Paint.MainWindow;

namespace HMQL_Project02_Paint
{
    internal class DrawElement
    {
        public IShapeEntity shape { get; set; }
        public IPaintBusiness painter { get; set; }
    }
}