using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using IContract;

namespace LineEntity
{
    public class LinePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var line = shape as LineEntity;
            var element = new Line()
            {
                X1 = line.TopLeft.X,
                Y1 = line.TopLeft.Y,
                X2 = line.BottomRight.X,
                Y2 = line.BottomRight.Y,
                StrokeThickness = shape.StrokeThickness,
                StrokeDashArray = shape.StrokePattern,
                Stroke = new SolidColorBrush(shape.StrokeColor)
            };
            return element;
        }
    }
}