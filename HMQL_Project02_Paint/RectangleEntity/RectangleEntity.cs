using IContract;
using System;
using System.Windows;
using System.Windows.Media;

namespace RectangleEntity
{
    public class RectangleEntity : IShapeEntity
    {
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }
        public string Name => "Rectangle";

        public int StrokeThickness { get; set; }
        public Color StrokeColor { get; set; }
        public DoubleCollection StrokePattern { get; set; }

        public void HandleStart(Point point)
        {
            TopLeft = point;
        }

        public void HandleEnd(Point point)
        {
            BottomRight = point;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}