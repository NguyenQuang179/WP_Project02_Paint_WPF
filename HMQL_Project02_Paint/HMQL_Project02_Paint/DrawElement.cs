using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HMQL_Project02_Paint.MainWindow;

namespace HMQL_Project02_Paint
{
    class DrawElement
    {
        public IShape shape {  get; set; }
        public IPainter painter { get; set; }
    }
}
