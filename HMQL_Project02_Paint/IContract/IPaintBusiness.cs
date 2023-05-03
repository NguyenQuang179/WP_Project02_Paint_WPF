﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IContract
{
    public interface IPaintBusiness
    {
        public UIElement Draw(IShapeEntity entity);
    }
}