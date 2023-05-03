using IContract;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LineEntity
{
    public class LineEntity : IShapeEntity
    {
        public BitmapImage Icon => new BitmapImage(new Uri("pack://application:,,,/LineEntity;component/line-icon.png", UriKind.Relative));

        public Point Start { get; set; }
        public Point End { get; set; }

        public string Name => "Line";

        public int StrokeThickness { get; set; }
        public Color StrokeColor { get; set; }
        public DoubleCollection StrokePattern { get; set; }

        public void HandleStart(Point point)
        {
            Start = point;
        }

        public void HandleEnd(Point point)
        {
            End = point;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}