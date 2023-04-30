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
        interface IShape : ICloneable
        {
            public string Name { get; }
            void HandleStart(Point point);
            void HandleEnd(Point point);

        }

        class LineEntity : IShape
        {
            public Point Start { get; set; }
            public Point End { get; set; }

            public string Name => "Line";

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
        interface IPainter
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
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Black)
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
               
                //if (width >= 0 && height >= 0)
                //{
                    var element = new Rectangle()
                    {
                        Width = width,
                        Height = height,
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Colors.Black)
                    };
                    Canvas.SetLeft(element, rectangle.TopLeft.X);
                    Canvas.SetTop(element, rectangle.TopLeft.Y);
                    return element;
                //}
                //Add Handling negative value as currently it can only draw from top left to bottom right
                //else if (width <= 0 && height >= 0)
                //{
                //    return element;
                //}
                //else if (width >= 0 && height <= 0)
                //{
                //    return element;
                //}
                //else if (width <= 0 && height <= 0)
                //{
                //    return element;
                //}
            }

            
        }
            
        List<IShape> _drawnShapes = new List<IShape>();
        IShape _preview = null;
        string _type = "Unknown"; // 0-LINE, 1-Rectangle
        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            _isDrawing = true;
            _start = e.GetPosition(canvas);

            _preview.HandleStart(_start);
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
        }
        LineEntity _line = new LineEntity();
        RectangleEntity _rectangle = new RectangleEntity();
        Dictionary<string, IPainter> _painterPrototypes;
        Dictionary<string, IShape> _shapePrototypes;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            _painterPrototypes = new Dictionary<string, IPainter>
            {
                {_line.Name, new LinePainter() },
                {_rectangle.Name, new RectanglePainter() }
            };

            _shapePrototypes = new Dictionary<string, IShape>
            {
                {_line.Name, new LineEntity() },
                {_rectangle.Name, new RectangleEntity() }
            };

            _type = _line.Name;
            _preview = (IShape)_shapePrototypes[_type].Clone();
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
    }
}
