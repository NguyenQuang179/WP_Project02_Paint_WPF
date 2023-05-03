using IContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace RectangleEntity
{
    internal class RectanglePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var rectangle = shape as RectangleEntity;
            double x = Math.Min(rectangle.TopLeft.X, rectangle.BottomRight.X);
            double y = Math.Min(rectangle.TopLeft.Y, rectangle.BottomRight.Y);
            double width = Math.Abs(rectangle.TopLeft.X - rectangle.BottomRight.X);
            double height = Math.Abs(rectangle.TopLeft.Y - rectangle.BottomRight.Y);

            var element = new Rectangle()
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