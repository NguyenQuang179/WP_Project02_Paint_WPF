﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IContract
{
    public interface IShapeEntity : ICloneable
    {
        public string Name { get; }

        BitmapImage Icon { get; }
        public int StrokeThickness { get; set; }
        public Color StrokeColor { get; set; }
        public DoubleCollection StrokePattern { get; set; }

        void HandleStart(Point point);

        void HandleEnd(Point point);
    }
}