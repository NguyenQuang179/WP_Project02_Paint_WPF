using IContract;
using System;
using System.Windows;
using System.Windows.Media;

namespace TriangleEntity
{
    public class TriangleEntity : IShapeEntity
    {
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }

        public string Name => "Triangle";

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

        public Point[] GetVertices()
        {
            Point[] vertices = new Point[3];
            double width = BottomRight.X - TopLeft.X;
            double height = BottomRight.Y - TopLeft.Y;

            // Define the base of the isosceles triangle
            double baseWidth = 0.8 * width;
            Point baseCenter = new Point(TopLeft.X + width / 2, TopLeft.Y + height);

            // Define the two points at the top of the isosceles triangle
            double sideLength = Math.Sqrt(Math.Pow(baseWidth / 2, 2) + Math.Pow(height, 2));
            Point leftVertex = new Point(baseCenter.X - baseWidth / 2, baseCenter.Y - sideLength);
            Point rightVertex = new Point(baseCenter.X + baseWidth / 2, baseCenter.Y - sideLength);

            vertices[0] = leftVertex;
            vertices[1] = rightVertex;
            vertices[2] = baseCenter;

            return vertices;
        }
    }
}