using IContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace TriangleEntity
{
    public class TrianglePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var triangle = shape as TriangleEntity;
            Point topLeft = triangle.TopLeft;
            Point bottomRight = triangle.BottomRight;

            double minX = Math.Min(triangle.TopLeft.X, triangle.BottomRight.X);
            double minY = Math.Min(triangle.TopLeft.Y, triangle.BottomRight.Y);
            double maxX = Math.Max(triangle.TopLeft.X, triangle.BottomRight.X);
            double maxY = Math.Max(triangle.TopLeft.Y, triangle.BottomRight.Y);
            double width = Math.Abs(triangle.TopLeft.X - triangle.BottomRight.X);
            double height = Math.Abs(triangle.TopLeft.Y - triangle.BottomRight.Y);

            Point topPoint = new Point(minX + (width / 2), minY);
            Point bottomLeftPoint = new Point(minX, maxY);
            Point bottomRightPoint = new Point(maxX, maxY);

            var element = new Polygon()
            {
                Points = new PointCollection()
            {
                topPoint,
                bottomLeftPoint,
                bottomRightPoint
            },
                StrokeThickness = shape.StrokeThickness,
                StrokeDashArray = shape.StrokePattern,
                Stroke = new SolidColorBrush(shape.StrokeColor)
            };
            return element;
        }
    }
}