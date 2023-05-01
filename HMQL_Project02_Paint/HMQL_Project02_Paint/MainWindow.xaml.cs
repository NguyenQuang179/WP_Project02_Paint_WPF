using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace HMQL_Project02_Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool _isDrawing = false;
        Point _start;
        //
        public interface IShape : ICloneable
        {
            public string Name { get; }
            public int StrokeThickness { get; set; }
            public Color StrokeColor { get; set; }
            void HandleStart(Point point);
            void HandleEnd(Point point);
        }

        class LineEntity : IShape
        {
            public Point Start { get; set; }
            public Point End { get; set; }

            public string Name => "Line";

            public int StrokeThickness { get; set; }
            public Color StrokeColor { get; set; }

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

        class RectangleEntity : IShape
        {
            public Point TopLeft { get; set; }
            public Point BottomRight{ get; set; }
            public string Name => "Rectangle";

            public int StrokeThickness { get; set; }
            public Color StrokeColor { get; set; }

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

        class TriangleEntity : IShape
        {
            public Point TopLeft { get; set; }
            public Point BottomRight { get; set; }

            public string Name => "Triangle";

            public int StrokeThickness { get; set; }
            public Color StrokeColor { get; set; }

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


        class EllipseEntity : IShape
        {
            public Point TopLeft { get; set; }
            public Point BottomRight { get; set; }
            public string Name => "Ellipse";

            public int StrokeThickness { get; set; }
            public Color StrokeColor { get; set; }

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

        //
        public List<int> thicknessValues = new List<int> { 1, 3, 5, 7, 9 };
        public int strokeThickness = 1;
        ColorAutoChange strokeColor = new ColorAutoChange();
        public interface IPainter
        {
            UIElement Draw(IShape shape);
        }

        class LinePainter : IPainter
        {
            public UIElement Draw(IShape shape)
            {
                var line = shape as LineEntity;
                var element = new Line()
                {
                    X1 = line.Start.X,
                    Y1 = line.Start.Y,
                    X2 = line.End.X,
                    Y2 = line.End.Y,
                    StrokeThickness = shape.StrokeThickness,
                    Stroke = new SolidColorBrush(shape.StrokeColor)
                };
                return element;
            }

        }

        class RectanglePainter : IPainter
        {
            public UIElement Draw(IShape shape)
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
                    Stroke = new SolidColorBrush(shape.StrokeColor)
                };
                Canvas.SetLeft(element, x);
                Canvas.SetTop(element, y);
                return element;
            }
        }

        class EllipsePainter : IPainter
        {
            public UIElement Draw(IShape shape)
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
                    Stroke = new SolidColorBrush(shape.StrokeColor)
                };
                Canvas.SetLeft(element, x);
                Canvas.SetTop(element, y);
                return element;
            }
        }

        class TrianglePainter : IPainter
        {
            public UIElement Draw(IShape shape)
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
                    Stroke = new SolidColorBrush(shape.StrokeColor)
                };
                return element;
            }
        }


        List<IShape> _drawnShapes = new List<IShape>();
        List<IShape> _redoList = new List<IShape>();
        IShape _preview = null;
        string _type = "Unknown"; // 0-LINE, 1-Rectangle, 2-Ellipse
        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            _isDrawing = true;
            _start = e.GetPosition(canvas);
            _preview.HandleStart(_start);
            _preview.StrokeThickness = strokeThickness;
            _preview.StrokeColor = strokeColor.color;
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                var end = e.GetPosition(canvas);
                _preview.HandleEnd(end);

                //Xóa đi tất cả bản vẽ củ
                canvas.Children.Clear();

                //Vẽ lại các điểm đã lưu (convert nó thành list chứa UI element và loại
                foreach( var item in _drawnShapes)
                {
                    IPainter painter = _painterPrototypes[item.Name];
                    UIElement shape = painter.Draw(item); // vẽ ra tương ứng với loại entity
                    canvas.Children.Add(shape);
                }

                var previewPainter = _painterPrototypes[_type];
                var previewElement = previewPainter.Draw(_preview);
                canvas.Children.Add(previewElement);
            }

        }

        private void Border_MouseUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
            var end = e.GetPosition(canvas);
            _preview.HandleEnd(end);
            _drawnShapes.Add(_preview.Clone() as IShape);
            _redoList.Clear();
        }

        LineEntity _line = new LineEntity();
        RectangleEntity _rectangle = new RectangleEntity();
        EllipseEntity _ellipse = new EllipseEntity();
        TriangleEntity _triangle = new TriangleEntity();
        Dictionary<string, IPainter> _painterPrototypes;
        Dictionary<string, IShape> _shapePrototypes;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StrokeSizeCombobox.SelectedIndex = 0;
            StrokeSizeCombobox.ItemsSource = thicknessValues;
            ColorPicker.Color = Colors.Black;
            _painterPrototypes = new Dictionary<string, IPainter>
            {
                {_line.Name, new LinePainter() },
                {_rectangle.Name, new RectanglePainter() },
                {_ellipse.Name, new EllipsePainter() },
                 {_triangle.Name, new TrianglePainter() }
            };

            _shapePrototypes = new Dictionary<string, IShape>
            {
                {_line.Name, new LineEntity() },
                {_rectangle.Name, new RectangleEntity() },
                {_ellipse.Name, new EllipseEntity() },
                {_triangle.Name, new TriangleEntity() }
            };
            _type = _line.Name;
            _preview = (IShape)_shapePrototypes[_type].Clone();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void lineButton_Click(object sender, RoutedEventArgs e)
        {
            _type = _line.Name;
            _preview = _line.Clone() as IShape;
        }

        private void rectangleButton_Click(object sender, RoutedEventArgs e)
        {
            _type = _rectangle.Name;
            _preview = _rectangle.Clone() as IShape;
        }

        private void ellipseButton_Click(Object sender, RoutedEventArgs e)
        {
            _type = _ellipse.Name;
            _preview = _ellipse.Clone() as IShape;
        }

        private void triangleButton_Click(Object sender, RoutedEventArgs e)
        {
            _type = _triangle.Name;
            _preview = _triangle.Clone() as IShape;
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            if(_drawnShapes.Count <= 0) return;
            IShape element = _drawnShapes[_drawnShapes.Count - 1];
            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
            _redoList.Add(element);
            //Xóa đi tất cả bản vẽ củ
            canvas.Children.Clear();

            //Vẽ lại các điểm đã lưu (convert nó thành list chứa UI element và loại
            foreach (var item in _drawnShapes)
            {
                IPainter painter = _painterPrototypes[item.Name];
                UIElement shape = painter.Draw(item); // vẽ ra tương ứng với loại entity
                canvas.Children.Add(shape);
            }
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_redoList.Count <= 0) return;
            IShape element = _redoList[_redoList.Count - 1];
            _redoList.RemoveAt(_redoList.Count - 1);
            _drawnShapes.Add(element);
            //Xóa đi tất cả bản vẽ củ
            canvas.Children.Clear();

            //Vẽ lại các điểm đã lưu (convert nó thành list chứa UI element và loại
            foreach (var item in _drawnShapes)
            {
                IPainter painter = _painterPrototypes[item.Name];
                UIElement shape = painter.Draw(item); // vẽ ra tương ứng với loại entity
                canvas.Children.Add(shape);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSize = StrokeSizeCombobox.SelectedIndex;
            strokeThickness = thicknessValues[selectedSize];
        }

        private void ColorPicker_SelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorAutoChange newColor = new ColorAutoChange
            {
                color = ColorPicker.Color
            };
            strokeColor = newColor;
        }
    }
}
