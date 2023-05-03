using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using IContract;

namespace EllipseEntity
{
    public class EllipsePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var ellipse = shape as EllipseEntity;
            double x = Math.Min(ellipse.TopLeft.X, ellipse.BottomRight.X);
            double y = Math.Min(ellipse.TopLeft.Y, ellipse.BottomRight.Y);
            double width = Math.Abs(ellipse.TopLeft.X - ellipse.BottomRight.X);
            double height = Math.Abs(ellipse.TopLeft.Y - ellipse.BottomRight.Y);

            var element = new Ellipse()
            {
                Width = width,
                Height = height,
                StrokeThickness = shape.StrokeThickness,
                StrokeDashArray = shape.StrokePattern,
                Stroke = new SolidColorBrush(shape.StrokeColor)
            };
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
            return element;
        }
    }
}