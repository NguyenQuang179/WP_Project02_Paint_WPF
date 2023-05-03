using IContract;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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

        private bool _isDrawing = false;
        private bool _selectItemMode = false;
        private bool _isFoundItem = false;
        private int _posOfSelectedItem = -1;
        private Point _start;

        public Double TestX = 0;
        public Double TestY = 0;

        //public interface IShapeEntity : ICloneable
        //{
        //    public string Name { get; }
        //    public int StrokeThickness { get; set; }
        //    public Color StrokeColor { get; set; }
        //    public DoubleCollection StrokePattern { get; set; }

        //    void HandleStart(Point point);

        //    void HandleEnd(Point point);
        //}

        //private class LineEntity : IShapeEntity
        //{
        //    public Point Start { get; set; }
        //    public Point End { get; set; }

        //    public string Name => "Line";

        //    public int StrokeThickness { get; set; }
        //    public Color StrokeColor { get; set; }
        //    public DoubleCollection StrokePattern { get; set; }

        //    public void HandleStart(Point point)
        //    {
        //        Start = point;
        //    }

        //    public void HandleEnd(Point point)
        //    {
        //        End = point;
        //    }

        //    public object Clone()
        //    {
        //        return MemberwiseClone();
        //    }
        //}

        //private class RectangleEntity : IShapeEntity
        //{
        //    public Point TopLeft { get; set; }
        //    public Point BottomRight { get; set; }
        //    public string Name => "Rectangle";

        //    public int StrokeThickness { get; set; }
        //    public Color StrokeColor { get; set; }
        //    public DoubleCollection StrokePattern { get; set; }

        //    public void HandleStart(Point point)
        //    {
        //        TopLeft = point;
        //    }

        //    public void HandleEnd(Point point)
        //    {
        //        BottomRight = point;
        //    }

        //    public object Clone()
        //    {
        //        return MemberwiseClone();
        //    }
        //}

        //private class TriangleEntity : IShapeEntity
        //{
        //    public Point TopLeft { get; set; }
        //    public Point BottomRight { get; set; }

        //    public string Name => "Triangle";

        //    public int StrokeThickness { get; set; }
        //    public Color StrokeColor { get; set; }
        //    public DoubleCollection StrokePattern { get; set; }

        //    public void HandleStart(Point point)
        //    {
        //        TopLeft = point;
        //    }

        //    public void HandleEnd(Point point)
        //    {
        //        BottomRight = point;
        //    }

        //    public object Clone()
        //    {
        //        return MemberwiseClone();
        //    }

        //    public Point[] GetVertices()
        //    {
        //        Point[] vertices = new Point[3];
        //        double width = BottomRight.X - TopLeft.X;
        //        double height = BottomRight.Y - TopLeft.Y;

        //        // Define the base of the isosceles triangle
        //        double baseWidth = 0.8 * width;
        //        Point baseCenter = new Point(TopLeft.X + width / 2, TopLeft.Y + height);

        //        // Define the two points at the top of the isosceles triangle
        //        double sideLength = Math.Sqrt(Math.Pow(baseWidth / 2, 2) + Math.Pow(height, 2));
        //        Point leftVertex = new Point(baseCenter.X - baseWidth / 2, baseCenter.Y - sideLength);
        //        Point rightVertex = new Point(baseCenter.X + baseWidth / 2, baseCenter.Y - sideLength);

        //        vertices[0] = leftVertex;
        //        vertices[1] = rightVertex;
        //        vertices[2] = baseCenter;

        //        return vertices;
        //    }
        //}

        //private class EllipseEntity : IShapeEntity
        //{
        //    public Point TopLeft { get; set; }
        //    public Point BottomRight { get; set; }
        //    public string Name => "Ellipse";

        //    public int StrokeThickness { get; set; }
        //    public Color StrokeColor { get; set; }
        //    public DoubleCollection StrokePattern { get; set; }

        //    public void HandleStart(Point point)
        //    {
        //        TopLeft = point;
        //    }

        //    public void HandleEnd(Point point)
        //    {
        //        BottomRight = point;
        //    }

        //    public object Clone()
        //    {
        //        return MemberwiseClone();
        //    }
        //}

        public List<int> thicknessValues = new List<int> { 1, 3, 5, 7, 9 };

        public List<DoubleCollection> listOfPatterns = new List<DoubleCollection>
        {
            new DoubleCollection { 1, 0 },
            new DoubleCollection { 1, 3 },
            new DoubleCollection { 3, 3 },
            new DoubleCollection { 5, 3 },
            new DoubleCollection { 1, 5, 5, 5 },
            new DoubleCollection { 5, 3, 1, 3, 1, 3}
        };

        public DoubleCollection strokePattern = new DoubleCollection { 1, 0 };
        public int strokeThickness = 1;
        private ColorAutoChange strokeColor = new ColorAutoChange();

        //public interface IPaintBusiness
        //{
        //    UIElement Draw(IShapeEntity shape);
        //}

        //private class LinePainter : IPaintBusiness
        //{
        //    public UIElement Draw(IShapeEntity shape)
        //    {
        //        var line = shape as LineEntity;
        //        var element = new Line()
        //        {
        //            X1 = line.Start.X,
        //            Y1 = line.Start.Y,
        //            X2 = line.End.X,
        //            Y2 = line.End.Y,
        //            StrokeThickness = shape.StrokeThickness,
        //            StrokeDashArray = shape.StrokePattern,
        //            Stroke = new SolidColorBrush(shape.StrokeColor)
        //        };
        //        return element;
        //    }
        //}

        //private class RectanglePainter : IPaintBusiness
        //{
        //    public UIElement Draw(IShapeEntity shape)
        //    {
        //        var rectangle = shape as RectangleEntity;
        //        double x = Math.Min(rectangle.TopLeft.X, rectangle.BottomRight.X);
        //        double y = Math.Min(rectangle.TopLeft.Y, rectangle.BottomRight.Y);
        //        double width = Math.Abs(rectangle.TopLeft.X - rectangle.BottomRight.X);
        //        double height = Math.Abs(rectangle.TopLeft.Y - rectangle.BottomRight.Y);

        //        var element = new Rectangle()
        //        {
        //            Width = width,
        //            Height = height,
        //            StrokeThickness = shape.StrokeThickness,
        //            StrokeDashArray = shape.StrokePattern,
        //            Stroke = new SolidColorBrush(shape.StrokeColor)
        //        };
        //        Canvas.SetLeft(element, x);
        //        Canvas.SetTop(element, y);
        //        return element;
        //    }
        //}

        //private class EllipsePainter : IPaintBusiness
        //{
        //    public UIElement Draw(IShapeEntity shape)
        //    {
        //        var ellipse = shape as EllipseEntity;
        //        double x = Math.Min(ellipse.TopLeft.X, ellipse.BottomRight.X);
        //        double y = Math.Min(ellipse.TopLeft.Y, ellipse.BottomRight.Y);
        //        double width = Math.Abs(ellipse.TopLeft.X - ellipse.BottomRight.X);
        //        double height = Math.Abs(ellipse.TopLeft.Y - ellipse.BottomRight.Y);

        //        var element = new Ellipse()
        //        {
        //            Width = width,
        //            Height = height,
        //            StrokeThickness = shape.StrokeThickness,
        //            StrokeDashArray = shape.StrokePattern,
        //            Stroke = new SolidColorBrush(shape.StrokeColor)
        //        };
        //        Canvas.SetLeft(element, x);
        //        Canvas.SetTop(element, y);
        //        return element;
        //    }
        //}

        //private class TrianglePainter : IPaintBusiness
        //{
        //    public UIElement Draw(IShapeEntity shape)
        //    {
        //        var triangle = shape as TriangleEntity;
        //        Point topLeft = triangle.TopLeft;
        //        Point bottomRight = triangle.BottomRight;

        //        double minX = Math.Min(triangle.TopLeft.X, triangle.BottomRight.X);
        //        double minY = Math.Min(triangle.TopLeft.Y, triangle.BottomRight.Y);
        //        double maxX = Math.Max(triangle.TopLeft.X, triangle.BottomRight.X);
        //        double maxY = Math.Max(triangle.TopLeft.Y, triangle.BottomRight.Y);
        //        double width = Math.Abs(triangle.TopLeft.X - triangle.BottomRight.X);
        //        double height = Math.Abs(triangle.TopLeft.Y - triangle.BottomRight.Y);

        //        Point topPoint = new Point(minX + (width / 2), minY);
        //        Point bottomLeftPoint = new Point(minX, maxY);
        //        Point bottomRightPoint = new Point(maxX, maxY);

        //        var element = new Polygon()
        //        {
        //            Points = new PointCollection()
        //    {
        //        topPoint,
        //        bottomLeftPoint,
        //        bottomRightPoint
        //    },
        //            StrokeThickness = shape.StrokeThickness,
        //            StrokeDashArray = shape.StrokePattern,
        //            Stroke = new SolidColorBrush(shape.StrokeColor)
        //        };
        //        return element;
        //    }
        //}

        private List<IShapeEntity> _drawnShapes = new List<IShapeEntity>();
        private List<IShapeEntity> _redoList = new List<IShapeEntity>();
        private IShapeEntity _preview = null;
        private IShapeEntity _selectedBorder = null;
        private string _currentType = ""; // 0-LINE, 1-Rectangle, 2-Ellipse

        private Dictionary<string, IPaintBusiness> _painterPrototypes = new Dictionary<string, IPaintBusiness>();
        private Dictionary<string, IShapeEntity> _shapePrototypes = new Dictionary<string, IShapeEntity>();

        public static double Distance(Point point1, Point point2)
        {
            double xDiff = point2.X - point1.X;
            double yDiff = point2.Y - point1.Y;
            double distance = Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
            return distance;
        }

        public static bool IsBetween(Point start, Point end, Point point)
        {
            double distance = Distance(start, end);
            double distance1 = Distance(start, point);
            double distance2 = Distance(point, end);
            return (distance > distance1) && (distance > distance2);
        }

        public bool areCollinear(Point p1, Point p2, Point p3)
        {
            double numerator = Math.Abs((p2.Y - p1.Y) * p3.X - (p2.X - p1.X) * p3.Y + p2.X * p1.Y - p2.Y * p1.X);
            double denominator = Math.Sqrt(Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Y - p1.X, 2));
            return ((numerator / denominator) < 10) && IsBetween(p1, p2, p3);
        }

        public bool isInRectangle(Point p1, Point p2, Point p3)
        {
            double minX = Math.Min(p1.X, p2.X);
            double maxX = Math.Max(p1.X, p2.X);
            double minY = Math.Min(p1.Y, p2.Y);
            double maxY = Math.Max(p1.Y, p2.Y);

            if (p3.X >= minX && p3.X <= maxX && p3.Y >= minY && p3.Y <= maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public bool isInShape()
        //{
        //    _isFoundItem = false;
        //    _posOfSelectedItem = -1;
        //    for (int i = _drawnShapes.Count - 1; i >= 0; i--)
        //    {
        //        var item = _drawnShapes[i];
        //        //var line = _shapePrototypes["Line"];
        //        //var rectangle = _shapePrototypes["Rectangle"];
        //        //var ellipse = _shapePrototypes["Ellipse"];
        //        //var triangle = _shapePrototypes["Triangle"];

        //        if (_isFoundItem) break;

        //        switch (item)
        //        {
        //            case LineEntity line:
        //                if (areCollinear(line.Start, line.End, _start))
        //                {
        //                    _posOfSelectedItem = i;
        //                    _isFoundItem = true;
        //                    _selectedBorder = _line.Clone() as IShapeEntity;
        //                    _selectedBorder.HandleStart(line.Start);
        //                    _selectedBorder.HandleEnd(line.End);
        //                    _selectedBorder.StrokeThickness = 2;
        //                    _selectedBorder.StrokeColor = Colors.Red;
        //                    IPaintBusiness painter = _painterPrototypes["Line"];
        //                    UIElement shape = painter.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
        //                    canvas.Children.Add(shape);
        //                }
        //                else
        //                {
        //                }
        //                break;

        //            case RectangleEntity rectangle:
        //                if (isInRectangle(rectangle.TopLeft, rectangle.BottomRight, _start))
        //                {
        //                    _posOfSelectedItem = i;
        //                    _isFoundItem = true;
        //                    _selectedBorder = _rectangle.Clone() as IShapeEntity;
        //                    _selectedBorder.HandleStart(rectangle.TopLeft);
        //                    _selectedBorder.HandleEnd(rectangle.BottomRight);
        //                    _selectedBorder.StrokeThickness = 2;
        //                    _selectedBorder.StrokeColor = Colors.Red;
        //                    IPaintBusiness painter = _painterPrototypes["Rectangle"];
        //                    UIElement shape = painter.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
        //                    canvas.Children.Add(shape);
        //                }
        //                else
        //                {
        //                }
        //                break;

        //            case EllipseEntity ellipse:
        //                if (isInRectangle(ellipse.TopLeft, ellipse.BottomRight, _start))
        //                {
        //                    _posOfSelectedItem = i;
        //                    _isFoundItem = true;
        //                    _selectedBorder = _rectangle.Clone() as IShapeEntity;
        //                    _selectedBorder.HandleStart(ellipse.TopLeft);
        //                    _selectedBorder.HandleEnd(ellipse.BottomRight);
        //                    _selectedBorder.StrokeThickness = 2;
        //                    _selectedBorder.StrokeColor = Colors.Red;
        //                    IPaintBusiness painter = _painterPrototypes["Rectangle"];
        //                    UIElement shape = painter.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
        //                    canvas.Children.Add(shape);
        //                }
        //                else
        //                {
        //                }
        //                break;

        //            case TriangleEntity triangle:
        //                if (isInRectangle(triangle.TopLeft, triangle.BottomRight, _start))
        //                {
        //                    _posOfSelectedItem = i;
        //                    _isFoundItem = true;
        //                    _selectedBorder = _rectangle.Clone() as IShapeEntity;
        //                    _selectedBorder.HandleStart(triangle.TopLeft);
        //                    _selectedBorder.HandleEnd(triangle.BottomRight);
        //                    _selectedBorder.StrokeThickness = 2;
        //                    _selectedBorder.StrokeColor = Colors.Red;
        //                    IPaintBusiness painter = _painterPrototypes["Rectangle"];
        //                    UIElement shape = painter.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
        //                    canvas.Children.Add(shape);
        //                }
        //                else
        //                {
        //                }
        //                break;

        //            default:
        //                // code for other types
        //                break;
        //        }
        //    }
        //    if (_isFoundItem)
        //    {
        //        _drawnShapes.Add(_selectedBorder);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            if (_selectItemMode && !_isFoundItem)
            {
                _start = e.GetPosition(canvas);
                //if (isInShape())
                //{
                //    _isFoundItem = true;
                //}
            }
            else if (_selectItemMode && _isFoundItem)
            {
                _start = e.GetPosition(canvas);

                _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                //if (isInShape())
                //{
                //    _isFoundItem = true;
                //}
            }
            else
            {
                _isDrawing = true;
                _start = e.GetPosition(canvas);
                _preview.HandleStart(_start);
                _preview.StrokeThickness = strokeThickness;
                _preview.StrokeColor = strokeColor.color;
                _preview.StrokePattern = strokePattern;
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                var end = e.GetPosition(canvas);
                _preview.HandleEnd(end);
                var previewPainter = _painterPrototypes[_currentType];
                var previewElement = previewPainter.Draw(_preview);
                canvas.Children.Add(previewElement);
                if (canvas.Children.Count > 1) canvas.Children.RemoveAt(canvas.Children.Count - 2);
            }
        }

        private void Border_MouseUp(object sender, MouseEventArgs e)
        {
            if (_selectItemMode && _isFoundItem)
            {
                var end = e.GetPosition(canvas);
                var item = _drawnShapes[_posOfSelectedItem];

                //switch (item)
                //{
                //    case LineEntity line:
                //        {
                //            _drawnShapes.RemoveAt(_posOfSelectedItem);
                //            canvas.Children.RemoveAt(_posOfSelectedItem);
                //            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                //            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                //            Point newStart = new Point(line.Start.X - _start.X + end.X, line.Start.Y - _start.Y + end.Y);
                //            Point newEnd = new Point(line.End.X - _start.X + end.X, line.End.Y - _start.Y + end.Y);

                //            _preview = _line.Clone() as IShapeEntity;
                //            _preview.HandleStart(newStart);
                //            _preview.HandleEnd(newEnd);
                //            _preview.StrokeThickness = line.StrokeThickness;
                //            _preview.StrokeColor = line.StrokeColor;
                //            IPaintBusiness painterLine = _painterPrototypes["Line"];
                //            UIElement realShape = painterLine.Draw(_preview);
                //            canvas.Children.Add(realShape);
                //            _drawnShapes.Add(_preview);
                //            _posOfSelectedItem = _drawnShapes.Count - 1;

                //            _selectedBorder = _line.Clone() as IShapeEntity;
                //            _selectedBorder.HandleStart(newStart);
                //            _selectedBorder.HandleEnd(newEnd);
                //            _selectedBorder.StrokeThickness = 2;
                //            _selectedBorder.StrokeColor = Colors.Red;
                //            UIElement selectedShape = painterLine.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                //            canvas.Children.Add(selectedShape);
                //            _drawnShapes.Add(_selectedBorder);

                //            break;
                //        }
                //    case RectangleEntity rectangle:
                //        {
                //            _drawnShapes.RemoveAt(_posOfSelectedItem);
                //            canvas.Children.RemoveAt(_posOfSelectedItem);
                //            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                //            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                //            Point newStart = new Point(rectangle.TopLeft.X - _start.X + end.X, rectangle.TopLeft.Y - _start.Y + end.Y);
                //            Point newEnd = new Point(rectangle.BottomRight.X - _start.X + end.X, rectangle.BottomRight.Y - _start.Y + end.Y);

                //            _preview = _rectangle.Clone() as IShapeEntity;
                //            _preview.HandleStart(newStart);
                //            _preview.HandleEnd(newEnd);
                //            _preview.StrokeThickness = rectangle.StrokeThickness;
                //            _preview.StrokeColor = rectangle.StrokeColor;
                //            IPaintBusiness painterRectangle = _painterPrototypes["Rectangle"];
                //            UIElement realShape = painterRectangle.Draw(_preview);
                //            canvas.Children.Add(realShape);
                //            _drawnShapes.Add(_preview);
                //            _posOfSelectedItem = _drawnShapes.Count - 1;

                //            _selectedBorder = _rectangle.Clone() as IShapeEntity;
                //            _selectedBorder.HandleStart(newStart);
                //            _selectedBorder.HandleEnd(newEnd);
                //            _selectedBorder.StrokeThickness = 2;
                //            _selectedBorder.StrokeColor = Colors.Red;
                //            UIElement selectedShape = painterRectangle.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                //            canvas.Children.Add(selectedShape);
                //            _drawnShapes.Add(_selectedBorder);

                //            break;
                //        }
                //    case EllipseEntity ellipse:
                //        {
                //            _drawnShapes.RemoveAt(_posOfSelectedItem);
                //            canvas.Children.RemoveAt(_posOfSelectedItem);
                //            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                //            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                //            Point newStart = new Point(ellipse.TopLeft.X - _start.X + end.X, ellipse.TopLeft.Y - _start.Y + end.Y);
                //            Point newEnd = new Point(ellipse.BottomRight.X - _start.X + end.X, ellipse.BottomRight.Y - _start.Y + end.Y);

                //            _preview = _ellipse.Clone() as IShapeEntity;
                //            _preview.HandleStart(newStart);
                //            _preview.HandleEnd(newEnd);
                //            _preview.StrokeThickness = ellipse.StrokeThickness;
                //            _preview.StrokeColor = ellipse.StrokeColor;
                //            IPaintBusiness painterEllipse = _painterPrototypes["Ellipse"];
                //            UIElement realShape = painterEllipse.Draw(_preview);
                //            canvas.Children.Add(realShape);
                //            _drawnShapes.Add(_preview);
                //            _posOfSelectedItem = _drawnShapes.Count - 1;

                //            _selectedBorder = _rectangle.Clone() as IShapeEntity;
                //            _selectedBorder.HandleStart(newStart);
                //            _selectedBorder.HandleEnd(newEnd);
                //            _selectedBorder.StrokeThickness = 2;
                //            _selectedBorder.StrokeColor = Colors.Red;
                //            IPaintBusiness painterRectangle = _painterPrototypes["Rectangle"];
                //            UIElement selectedShape = painterRectangle.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                //            canvas.Children.Add(selectedShape);
                //            _drawnShapes.Add(_selectedBorder);

                //            break;
                //        }
                //    case TriangleEntity triangle:
                //        {
                //            _drawnShapes.RemoveAt(_posOfSelectedItem);
                //            canvas.Children.RemoveAt(_posOfSelectedItem);
                //            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                //            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                //            Point newStart = new Point(triangle.TopLeft.X - _start.X + end.X, triangle.TopLeft.Y - _start.Y + end.Y);
                //            Point newEnd = new Point(triangle.BottomRight.X - _start.X + end.X, triangle.BottomRight.Y - _start.Y + end.Y);

                //            _preview = _triangle.Clone() as IShapeEntity;
                //            _preview.HandleStart(newStart);
                //            _preview.HandleEnd(newEnd);
                //            _preview.StrokeThickness = triangle.StrokeThickness;
                //            _preview.StrokeColor = triangle.StrokeColor;
                //            IPaintBusiness painterTriangle = _painterPrototypes["Triangle"];
                //            UIElement realShape = painterTriangle.Draw(_preview);
                //            canvas.Children.Add(realShape);
                //            _drawnShapes.Add(_preview);
                //            _posOfSelectedItem = _drawnShapes.Count - 1;

                //            _selectedBorder = _line.Clone() as IShapeEntity;
                //            _selectedBorder.HandleStart(newStart);
                //            _selectedBorder.HandleEnd(newEnd);
                //            _selectedBorder.StrokeThickness = 2;
                //            _selectedBorder.StrokeColor = Colors.Red;
                //            IPaintBusiness painterRectangle = _painterPrototypes["Rectangle"];
                //            UIElement selectedShape = painterRectangle.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                //            canvas.Children.Add(selectedShape);
                //            _drawnShapes.Add(_selectedBorder);

                //            break;
                //        }
                //    default:
                //        // code for other types
                //        break;
                //}
            }
            if (!_selectItemMode)
            {
                _isDrawing = false;
                var end = e.GetPosition(canvas);
                _preview.HandleEnd(end);
                _drawnShapes.Add(_preview.Clone() as IShapeEntity);
                IPaintBusiness painter = _painterPrototypes[_preview.Name];
                UIElement shape = painter.Draw(_preview);
                canvas.Children.Add(shape);
                _redoList.Clear();
            }
        }

        //private LineEntity _line = new LineEntity();

        //private RectangleEntity _rectangle = new RectangleEntity();
        //private EllipseEntity _ellipse = new EllipseEntity();
        //private TriangleEntity _triangle = new TriangleEntity();

        //private Dictionary<string, IPaintBusiness> _painterPrototypes;
        //private Dictionary<string, IShapeEntity> _shapePrototypes;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /* Nạp tất cả dll và tìm kiếm entity và business*/
            string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var folderInfo = new DirectoryInfo(exeFolder);
            var dllFiles = folderInfo.GetFiles("*.dll");

            //Quét dll để tìm khả năng mới
            var list = new List<IShapeEntity>();

            foreach (var dll in dllFiles)
            {
                //var domain = AppDomain.CurrentDomain;
                //Assembly assembly = domain.Load(
                //    AssemblyName.GetAssemblyName(dll.FullName));
                Assembly assembly = Assembly.LoadFrom(dll.FullName);

                Type[] types = assembly.GetTypes();

                //  Giả định dll chi có một entity và business tương ứng
                IShapeEntity? entity = null;
                IPaintBusiness? business = null;

                foreach (Type type in types)
                {
                    //Console.WriteLine(type.FullName);
                    //if (type.Name == "LineEntity")
                    //{
                    //    MessageBox.Show(type.Name);
                    //}

                    if (type.IsClass)
                    {
                        if (typeof(IShapeEntity).IsAssignableFrom(type))
                        {
                            entity = (Activator.CreateInstance(type) as IShapeEntity)!;
                        }

                        if (typeof(IPaintBusiness).IsAssignableFrom(type))
                        {
                            business = (Activator.CreateInstance(type) as IPaintBusiness)!;
                        }
                    }
                }

                if (entity != null)
                {
                    //MessageBox.Show(entity.Name);
                    _shapePrototypes.Add(entity!.Name, entity);
                    _painterPrototypes.Add(entity!.Name, business!);
                }
            }

            Title = $"Tìm thấy {_shapePrototypes.Count} hình ";

            //Tạo ra các nút bấm tương ứng
            foreach (var (name, entity) in _shapePrototypes)
            {
                var button = new Button();
                button.Content = name;
                button.Tag = entity;
                //button.icon = entity.Icon
                button.Width = 56;
                button.Margin = new Thickness(0, 0, 16, 0);
                button.Click += Button_Click;

                //TODO: Thêm nút bấm vào giao diện
                actionStackPanel.Children.Add(button);
            }

            if (_shapePrototypes.Count > 0)
            {
                //Lựa chọn nút bấm đầu tiên
                var (key, shape) = _shapePrototypes.ElementAt(0);
                _currentType = key;
                _preview = (shape.Clone() as IShapeEntity)!;
            }

            StrokeSizeCombobox.ItemsSource = thicknessValues;
            StrokeSizeCombobox.SelectedIndex = 0;

            StrokePatternCombobox.ItemsSource = listOfPatterns;
            StrokePatternCombobox.SelectedIndex = 0;

            ColorPicker.Color = Colors.Black;

            //_painterPrototypes = new Dictionary<string, IPaintBusiness>
            //{
            //    {_line.Name, new LinePainter() },
            //    {_rectangle.Name, new RectanglePainter() },
            //    {_ellipse.Name, new EllipsePainter() },
            //     {_triangle.Name, new TrianglePainter() }
            //};

            //_shapePrototypes = new Dictionary<string, IShapeEntity>
            //{
            //    {_line.Name, new LineEntity() },
            //    {_rectangle.Name, new RectangleEntity() },
            //    {_ellipse.Name, new EllipseEntity() },
            //    {_triangle.Name, new TriangleEntity() }
            //};
            //_currentType = _line.Name;
            //_preview = (IShapeEntity)_shapePrototypes[_type].Clone();

            //Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var entity = button!.Tag as IShapeEntity;

            _currentType = entity.Name;
            _preview = (_shapePrototypes[entity.Name].Clone() as IShapeEntity)!;
        }

        //private void lineButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _type = _line.Name;
        //    _preview = _line.Clone() as IShapeEntity;
        //    _selectItemMode = false;
        //}

        //private void rectangleButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _type = _rectangle.Name;
        //    _preview = _rectangle.Clone() as IShapeEntity;
        //    _selectItemMode = false;
        //}

        //private void ellipseButton_Click(Object sender, RoutedEventArgs e)
        //{
        //    _type = _ellipse.Name;
        //    _preview = _ellipse.Clone() as IShapeEntity;
        //    _selectItemMode = false;
        //}

        //private void triangleButton_Click(Object sender, RoutedEventArgs e)
        //{
        //    _type = _triangle.Name;
        //    _preview = _triangle.Clone() as IShapeEntity;
        //}

        private void chooseShapeButton_Click(object sender, RoutedEventArgs e)
        {
            _selectItemMode = true;
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_drawnShapes.Count <= 0) return;
            IShapeEntity element = _drawnShapes[_drawnShapes.Count - 1];
            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
            _redoList.Add(element);
            //Xóa đi tất cả bản vẽ củ
            canvas.Children.Clear();

            //Vẽ lại các điểm đã lưu (convert nó thành list chứa UI element và loại
            foreach (var item in _drawnShapes)
            {
                IPaintBusiness painter = _painterPrototypes[item.Name];
                UIElement shape = painter.Draw(item); // vẽ ra tương ứng với loại entity
                canvas.Children.Add(shape);
            }
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_redoList.Count <= 0) return;
            IShapeEntity element = _redoList[_redoList.Count - 1];
            _redoList.RemoveAt(_redoList.Count - 1);
            _drawnShapes.Add(element);
            //Xóa đi tất cả bản vẽ củ
            canvas.Children.Clear();

            //Vẽ lại các điểm đã lưu (convert nó thành list chứa UI element và loại
            foreach (var item in _drawnShapes)
            {
                IPaintBusiness painter = _painterPrototypes[item.Name];
                UIElement shape = painter.Draw(item); // vẽ ra tương ứng với loại entity
                canvas.Children.Add(shape);
            }
        }

        private void StrokeSizeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSize = StrokeSizeCombobox.SelectedIndex;
            strokeThickness = thicknessValues[selectedSize];
        }

        private void StrokePatternSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoubleCollection selectedPattern = StrokePatternCombobox.SelectedItem as DoubleCollection;
            strokePattern = selectedPattern;
        }

        private void ColorPicker_SelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorAutoChange newColor = new ColorAutoChange
            {
                color = ColorPicker.Color
            };
            strokeColor = newColor;
        }

        private void deleteTest_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.RemoveAt(_posOfSelectedItem);
        }
    }
}