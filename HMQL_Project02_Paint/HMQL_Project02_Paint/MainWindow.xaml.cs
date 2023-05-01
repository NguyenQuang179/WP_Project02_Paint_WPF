using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

        bool _isDrawing = false;
        bool _selectItemMode = false;
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
            public Point BottomRight { get; set; }
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
                double width = rectangle.BottomRight.X - rectangle.TopLeft.X;
                double height = rectangle.BottomRight.Y - rectangle.TopLeft.Y;

                var element = new Rectangle()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = shape.StrokeThickness,
                    Stroke = new SolidColorBrush(shape.StrokeColor)
                };
                Canvas.SetLeft(element, rectangle.TopLeft.X);
                Canvas.SetTop(element, rectangle.TopLeft.Y);
                return element;
            }
        }

        class EllipsePainter : IPainter
        {
            public UIElement Draw(IShape shape)
            {
                var ellipse = shape as EllipseEntity;
                double width = ellipse.BottomRight.X - ellipse.TopLeft.X;
                double height = ellipse.BottomRight.Y - ellipse.TopLeft.Y;

                var element = new Ellipse()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = shape.StrokeThickness,
                    Stroke = new SolidColorBrush(shape.StrokeColor)
                };
                Canvas.SetLeft(element, ellipse.TopLeft.X);
                Canvas.SetTop(element, ellipse.TopLeft.Y);
                return element;
            }
        }

        List<IShape> _drawnShapes = new List<IShape>();
        List<IShape> _redoList = new List<IShape>();
        IShape _preview = null;
        string _type = "Unknown"; // 0-LINE, 1-Rectangle, 2-Ellipse

        public bool areCollinear(Point p1, Point p2, Point p3)
        {
            double numerator = Math.Abs((p2.Y - p1.Y) * p3.X - (p2.X - p1.X) * p3.Y + p2.X * p1.Y - p2.Y * p1.X);
            double denominator = Math.Sqrt(Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Y - p1.X, 2));
            return (numerator / denominator) < 10;
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

        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            if (_selectItemMode)
            {
                _start = e.GetPosition(canvas);
                foreach (var item in _drawnShapes)
                {
                    switch (item)
                    {
                        case LineEntity line:
                            if (areCollinear(line.Start, line.End, _start))
                            {

                            }
                            else
                            {

                            }
                            break;
                        case RectangleEntity rectangle:
                            if (isInRectangle(rectangle.TopLeft, rectangle.BottomRight, _start))
                            {


                            }
                            else
                            {

                            }
                            break;
                        case EllipseEntity ellipse:
                            if (isInRectangle(ellipse.TopLeft, ellipse.BottomRight, _start))
                            {

                            }
                            else
                            {

                            }
                            break;
                        default:
                            // code for other types
                            break;
                    }
                }
            }
            else
            {
                _isDrawing = true;
                _start = e.GetPosition(canvas);
                _preview.HandleStart(_start);
                _preview.StrokeThickness = strokeThickness;
                _preview.StrokeColor = strokeColor.color;
            }
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
                foreach (var item in _drawnShapes)
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
            if (_type != "FillColor")
            {
                _isDrawing = false;
                var end = e.GetPosition(canvas);
                _preview.HandleEnd(end);
                _drawnShapes.Add(_preview.Clone() as IShape);
                _redoList.Clear();
            }
        }

        LineEntity _line = new LineEntity();
        RectangleEntity _rectangle = new RectangleEntity();
        EllipseEntity _ellipse = new EllipseEntity();
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
                {_ellipse.Name, new EllipsePainter() }
            };

            _shapePrototypes = new Dictionary<string, IShape>
            {
                {_line.Name, new LineEntity() },
                {_rectangle.Name, new RectangleEntity() },
                {_ellipse.Name, new EllipseEntity() }
            };
            _type = _line.Name;
            _preview = (IShape)_shapePrototypes[_type].Clone();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void lineButton_Click(object sender, RoutedEventArgs e)
        {
            _type = _line.Name;
            _preview = _line.Clone() as IShape;
            _selectItemMode = false;
        }

        private void rectangleButton_Click(object sender, RoutedEventArgs e)
        {
            _type = _rectangle.Name;
            _preview = _rectangle.Clone() as IShape;
            _selectItemMode = false;
        }

        private void ellipseButton_Click(Object sender, RoutedEventArgs e)
        {
            _type = _ellipse.Name;
            _preview = _ellipse.Clone() as IShape;
            _selectItemMode = false;
        }

        private void fillColorButton_Click(object sender, RoutedEventArgs e)
        {
            _selectItemMode = true;
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_drawnShapes.Count <= 0) return;
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
