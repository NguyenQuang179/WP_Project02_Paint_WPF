using Microsoft.Win32;
﻿using IContract;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HMQL_Project02_Paint
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool _isDrawing = false;
        private bool _selectItemMode = false;
        private bool _isFoundItem = false;
        private bool _canBeMoved = false;
        private int _posOfSelectedItem = -1;
        private Point _start;

        public List<int> thicknessValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public List<DoubleCollection> listOfPatterns = new List<DoubleCollection>
        {
            new DoubleCollection { 1, 0 },
            new DoubleCollection { 1, 3 },
            new DoubleCollection { 3, 3 },
            new DoubleCollection { 5, 3 },
            new DoubleCollection { 1, 5, 5, 5 },
            new DoubleCollection { 5, 3, 1, 3, 1, 3}
        };

        public List<string> listOfPatternNames = new List<string>
        {
            "Line",
            "Dot",
            "Short Dash",
            "Long Dash",
            "Dot Dash",
            "Dash Dot Dot"
        };

        public DoubleCollection strokePattern = new DoubleCollection { 1, 0 };
        public int strokeThickness = 1;
        private ColorAutoChange strokeColor = new ColorAutoChange();

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

        public bool isInShape()
        {
            _isFoundItem = false;
            _posOfSelectedItem = -1;
            for (int i = _drawnShapes.Count - 1; i >= 0; i--)
            {
                var item = _drawnShapes[i];
                var item_name = _drawnShapes[i].Name;
                var line = _shapePrototypes["Line"];
                var rectangle = _shapePrototypes["Rectangle"];

                if (_isFoundItem) break;

                switch (item_name)
                {
                    case "Line":
                        if (areCollinear(item.TopLeft, item.BottomRight, _start))
                        {
                            _posOfSelectedItem = i;
                            _isFoundItem = true;
                            _selectedBorder = line.Clone() as IShapeEntity;
                            _selectedBorder.HandleStart(item.TopLeft);
                            _selectedBorder.HandleEnd(item.BottomRight);
                            _selectedBorder.StrokeThickness = 2;
                            _selectedBorder.StrokeColor = Colors.Red;
                            _selectedBorder.StrokePattern = new DoubleCollection() { 3, 3 };
                            IPaintBusiness painter = _painterPrototypes["Line"];
                            UIElement shape = painter.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                            canvas.Children.Add(shape);
                        }
                        else
                        {
                        }
                        break;

                    case "Rectangle":
                        if (isInRectangle(item.TopLeft, item.BottomRight, _start))
                        {
                            _posOfSelectedItem = i;
                            _isFoundItem = true;
                            _selectedBorder = rectangle.Clone() as IShapeEntity;
                            _selectedBorder.HandleStart(item.TopLeft);
                            _selectedBorder.HandleEnd(item.BottomRight);
                            _selectedBorder.StrokeThickness = 2;
                            _selectedBorder.StrokeColor = Colors.Red;
                            _selectedBorder.StrokePattern = new DoubleCollection() { 3, 3 };
                            IPaintBusiness painter = _painterPrototypes["Rectangle"];
                            UIElement shape = painter.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                            canvas.Children.Add(shape);
                        }
                        else
                        {
                        }
                        break;

                    case "Ellipse":
                        if (isInRectangle(item.TopLeft, item.BottomRight, _start))
                        {
                            _posOfSelectedItem = i;
                            _isFoundItem = true;
                            _selectedBorder = rectangle.Clone() as IShapeEntity;
                            _selectedBorder.HandleStart(item.TopLeft);
                            _selectedBorder.HandleEnd(item.BottomRight);
                            _selectedBorder.StrokeThickness = 2;
                            _selectedBorder.StrokeColor = Colors.Red;
                            _selectedBorder.StrokePattern = new DoubleCollection() { 3, 3 };
                            IPaintBusiness painter = _painterPrototypes["Rectangle"];
                            UIElement shape = painter.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                            canvas.Children.Add(shape);
                        }
                        else
                        {
                        }
                        break;

                    case "Triangle":
                        if (isInRectangle(item.TopLeft, item.BottomRight, _start))
                        {
                            _posOfSelectedItem = i;
                            _isFoundItem = true;
                            _selectedBorder = rectangle.Clone() as IShapeEntity;
                            _selectedBorder.HandleStart(item.TopLeft);
                            _selectedBorder.HandleEnd(item.BottomRight);
                            _selectedBorder.StrokeThickness = 2;
                            _selectedBorder.StrokeColor = Colors.Red;
                            _selectedBorder.StrokePattern = new DoubleCollection() { 3, 3 };
                            IPaintBusiness painter = _painterPrototypes["Rectangle"];
                            UIElement shape = painter.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                            canvas.Children.Add(shape);
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
            if (_isFoundItem)
            {
                _drawnShapes.Add(_selectedBorder);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            if (_selectItemMode && !_isFoundItem)
            {
                _start = e.GetPosition(canvas);
                _canBeMoved = false;
                if (isInShape())
                {
                    _isFoundItem = true;
                    _redoList.Clear();
                }
            }
            else if (_selectItemMode && _isFoundItem)
            {
                _start = e.GetPosition(canvas);
                int temp = _posOfSelectedItem;
                _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                if (isInShape())
                {
                    _isFoundItem = true;
                }
                if (temp != _posOfSelectedItem)
                {
                    _canBeMoved = false;
                    _redoList.Clear();
                }
            }
            else
            {
                if (_currentType == "") return;
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
            if (_isDrawing && _currentType != "")
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
            if (_selectItemMode && _isFoundItem && _canBeMoved)
            {
                var end = e.GetPosition(canvas);
                var item = _drawnShapes[_posOfSelectedItem];

                var rectangle = _shapePrototypes["Rectangle"];
                

                switch (item.Name)
                {
                    case "Line":
                        {
                            var line = _shapePrototypes["Line"];
                            _redoList.Add(_drawnShapes[_posOfSelectedItem]);
                            _drawnShapes.RemoveAt(_posOfSelectedItem);
                            canvas.Children.RemoveAt(_posOfSelectedItem);
                            _redoList.Add(_drawnShapes[_drawnShapes.Count - 1]);
                            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                            Point newStart = new Point(item.TopLeft.X - _start.X + end.X, item.TopLeft.Y - _start.Y + end.Y);
                            Point newEnd = new Point(item.BottomRight.X - _start.X + end.X, item.BottomRight.Y - _start.Y + end.Y);

                            _preview = line.Clone() as IShapeEntity;
                            _preview.HandleStart(newStart);
                            _preview.HandleEnd(newEnd);
                            _preview.StrokeThickness = item.StrokeThickness;
                            _preview.StrokeColor = item.StrokeColor;
                            _preview.StrokePattern = item.StrokePattern;
                            IPaintBusiness painterLine = _painterPrototypes["Line"];
                            UIElement realShape = painterLine.Draw(_preview);
                            canvas.Children.Add(realShape);
                            _drawnShapes.Add(_preview);
                            _posOfSelectedItem = _drawnShapes.Count - 1;

                            _selectedBorder = line.Clone() as IShapeEntity;
                            _selectedBorder.HandleStart(newStart);
                            _selectedBorder.HandleEnd(newEnd);
                            _selectedBorder.StrokeThickness = 2;
                            _selectedBorder.StrokeColor = Colors.Red;
                            _selectedBorder.StrokePattern = new DoubleCollection() { 3, 3 };
                            UIElement selectedShape = painterLine.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                            canvas.Children.Add(selectedShape);
                            _drawnShapes.Add(_selectedBorder);

                            break;
                        }
                    case "Rectangle":
                        {
                            
                            _redoList.Add(_drawnShapes[_posOfSelectedItem]);
                            _drawnShapes.RemoveAt(_posOfSelectedItem);
                            canvas.Children.RemoveAt(_posOfSelectedItem);
                            _redoList.Add(_drawnShapes[_drawnShapes.Count - 1]);
                            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                            Point newStart = new Point(item.TopLeft.X - _start.X + end.X, item.TopLeft.Y - _start.Y + end.Y);
                            Point newEnd = new Point(item.BottomRight.X - _start.X + end.X, item.BottomRight.Y - _start.Y + end.Y);

                            _preview = rectangle.Clone() as IShapeEntity;
                            _preview.HandleStart(newStart);
                            _preview.HandleEnd(newEnd);
                            _preview.StrokeThickness = item.StrokeThickness;
                            _preview.StrokeColor = item.StrokeColor;
                            _preview.StrokePattern = item.StrokePattern;
                            IPaintBusiness painterRectangle = _painterPrototypes["Rectangle"];
                            UIElement realShape = painterRectangle.Draw(_preview);
                            canvas.Children.Add(realShape);
                            _drawnShapes.Add(_preview);
                            _posOfSelectedItem = _drawnShapes.Count - 1;

                            _selectedBorder = rectangle.Clone() as IShapeEntity;
                            _selectedBorder.HandleStart(newStart);
                            _selectedBorder.HandleEnd(newEnd);
                            _selectedBorder.StrokeThickness = 2;
                            _selectedBorder.StrokeColor = Colors.Red;
                            _selectedBorder.StrokePattern = new DoubleCollection() { 3, 3 };
                            UIElement selectedShape = painterRectangle.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                            canvas.Children.Add(selectedShape);
                            _drawnShapes.Add(_selectedBorder);

                            break;
                        }
                    case "Ellipse":
                        {
                            var ellipse = _shapePrototypes["Ellipse"];
                            _redoList.Add(_drawnShapes[_posOfSelectedItem]);
                            _drawnShapes.RemoveAt(_posOfSelectedItem);
                            canvas.Children.RemoveAt(_posOfSelectedItem);
                            _redoList.Add(_drawnShapes[_drawnShapes.Count - 1]);
                            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                            Point newStart = new Point(item.TopLeft.X - _start.X + end.X, item.TopLeft.Y - _start.Y + end.Y);
                            Point newEnd = new Point(item.BottomRight.X - _start.X + end.X, item.BottomRight.Y - _start.Y + end.Y);

                            _preview = ellipse.Clone() as IShapeEntity;
                            _preview.HandleStart(newStart);
                            _preview.HandleEnd(newEnd);
                            _preview.StrokeThickness = item.StrokeThickness;
                            _preview.StrokeColor = item.StrokeColor;
                            _preview.StrokePattern = item.StrokePattern;
                            IPaintBusiness painterEllipse = _painterPrototypes["Ellipse"];
                            UIElement realShape = painterEllipse.Draw(_preview);
                            canvas.Children.Add(realShape);
                            _drawnShapes.Add(_preview);
                            _posOfSelectedItem = _drawnShapes.Count - 1;

                            _selectedBorder = rectangle.Clone() as IShapeEntity;
                            _selectedBorder.HandleStart(newStart);
                            _selectedBorder.HandleEnd(newEnd);
                            _selectedBorder.StrokeThickness = 2;
                            _selectedBorder.StrokeColor = Colors.Red;
                            _selectedBorder.StrokePattern = new DoubleCollection() { 3, 3 };
                            IPaintBusiness painterRectangle = _painterPrototypes["Rectangle"];
                            UIElement selectedShape = painterRectangle.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                            canvas.Children.Add(selectedShape);
                            _drawnShapes.Add(_selectedBorder);

                            break;
                        }
                    case "Triangle":
                        {
                            var triangle = _shapePrototypes["Triangle"];
                            _redoList.Add(_drawnShapes[_posOfSelectedItem]);
                            _drawnShapes.RemoveAt(_posOfSelectedItem);
                            canvas.Children.RemoveAt(_posOfSelectedItem);
                            _redoList.Add(_drawnShapes[_drawnShapes.Count - 1]);
                            _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                            Point newStart = new Point(item.TopLeft.X - _start.X + end.X, item.TopLeft.Y - _start.Y + end.Y);
                            Point newEnd = new Point(item.BottomRight.X - _start.X + end.X, item.BottomRight.Y - _start.Y + end.Y);

                            _preview = triangle.Clone() as IShapeEntity;
                            _preview.HandleStart(newStart);
                            _preview.HandleEnd(newEnd);
                            _preview.StrokeThickness = item.StrokeThickness;
                            _preview.StrokeColor = item.StrokeColor;
                            _preview.StrokePattern = item.StrokePattern;
                            IPaintBusiness painterTriangle = _painterPrototypes["Triangle"];
                            UIElement realShape = painterTriangle.Draw(_preview);
                            canvas.Children.Add(realShape);
                            _drawnShapes.Add(_preview);
                            _posOfSelectedItem = _drawnShapes.Count - 1;

                            _selectedBorder = rectangle.Clone() as IShapeEntity;
                            _selectedBorder.HandleStart(newStart);
                            _selectedBorder.HandleEnd(newEnd);
                            _selectedBorder.StrokeThickness = 2;
                            _selectedBorder.StrokeColor = Colors.Red;
                            _selectedBorder.StrokePattern = new DoubleCollection() { 3, 3 };
                            IPaintBusiness painterRectangle = _painterPrototypes["Rectangle"];
                            UIElement selectedShape = painterRectangle.Draw(_selectedBorder); // vẽ ra tương ứng với loại entity
                            canvas.Children.Add(selectedShape);
                            _drawnShapes.Add(_selectedBorder);

                            break;
                        }
                    default:
                        // code for other types
                        break;
                }
            }
            if (_selectItemMode && _isFoundItem)
            {
                _canBeMoved = true;
            }
            if (!_selectItemMode)
            {
                if (_currentType == "") return;
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
                Assembly assembly = Assembly.LoadFrom(dll.FullName);

                Type[] types = assembly.GetTypes();

                //  Giả định dll chi có một entity và business tương ứng
                IShapeEntity? entity = null;
                IPaintBusiness? business = null;

                foreach (Type type in types)
                {
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

            //Tạo ra các nút bấm tương ứng
            foreach (var (name, entity) in _shapePrototypes)
            {
                var button = new Button();
                button.Tag = entity;
                button.Content = new Image
                {
                    Source = entity.Icon,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Fill,
                    Height = 40,
                    Width = 40,
                };
                button.Height = 40;
                button.MinWidth = 56;
                button.Padding = new Thickness(8, 0, 8, 0);
                button.BorderBrush = Brushes.DarkGray;
                button.BorderThickness = new Thickness(1);
                button.Background = Brushes.Transparent;
                Border customBorder = new Border();
                var style = new Style(typeof(Border));
                style.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(4)));
                button.Resources.Add(typeof(Border), style);
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

            StrokePatternCombobox.ItemsSource = listOfPatternNames;
            StrokePatternCombobox.SelectedIndex = 0;

            ColorPicker.Color = Colors.Black;

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_selectItemMode)
            {
                if (_posOfSelectedItem == -1)
                {
                    var previewPainter = _painterPrototypes[_drawnShapes[_drawnShapes.Count - 1].Name];
                    var previewElement = previewPainter.Draw(_drawnShapes[_drawnShapes.Count - 1]);
                    canvas.Children.Add(previewElement);
                }
                _selectItemMode = false;
                _canBeMoved = false;
                _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                //canvas.Children.RemoveAt(canvas.Children.Count - 1);
                _posOfSelectedItem = -1;
                _isFoundItem = false;
            }
            var button = sender as Button;
            var entity = button!.Tag as IShapeEntity;

            _currentType = entity.Name;
            _preview = (_shapePrototypes[entity.Name].Clone() as IShapeEntity)!;
        }

        private void chooseShapeButton_Click(object sender, RoutedEventArgs e)
        {
            if (canvas.Children.Count == 0) return;
            if (_selectItemMode) return;
            _selectItemMode = true;
            _isFoundItem = false;
            _posOfSelectedItem = -1;
            _canBeMoved = false;

            canvas.Children.RemoveAt(canvas.Children.Count - 1);
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_drawnShapes.Count <= 0) return;
            if (_selectItemMode)
            {
                if (_redoList.Count <= 0) return;

                _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                _drawnShapes.RemoveAt(_drawnShapes.Count - 1);

                IShapeEntity element1 = _redoList[_redoList.Count - 2];
                _redoList.RemoveAt(_redoList.Count - 2);
                _drawnShapes.Add(element1);

                IShapeEntity element2 = _redoList[_redoList.Count - 1];
                _redoList.RemoveAt(_redoList.Count - 1);
                _drawnShapes.Add(element2);
            }
            else
            {
                canvas.Children.RemoveAt(canvas.Children.Count -1);
                IShapeEntity element = _drawnShapes[_drawnShapes.Count - 1];
                _drawnShapes.RemoveAt(_drawnShapes.Count - 1);
                _redoList.Add(element);
            }
            //Xóa đi tất cả bản vẽ củ
            canvas.Children.Clear();

            //Vẽ lại các điểm đã lưu (convert nó thành list chứa UI element và loại
            foreach (var item in _drawnShapes)
            {
                IPaintBusiness painter = _painterPrototypes[item.Name];
                UIElement shape = painter.Draw(item); // vẽ ra tương ứng với loại entity
                UIElement previewShape = painter.Draw(item);
                canvas.Children.Add(shape);
                canvas.Children.Add(previewShape);
            }
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_redoList.Count <= 0) return;
            if (_selectItemMode)
            {
                return;
            }
            if (canvas.Children.Count > 0)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
            
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
                UIElement previewShape = painter.Draw(item);
                canvas.Children.Add(shape);
                canvas.Children.Add(previewShape);
            }
        }

        private void StrokeSizeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSize = StrokeSizeCombobox.SelectedIndex;
            strokeThickness = thicknessValues[selectedSize];
        }

        private void StrokePatternSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoubleCollection pattern = listOfPatterns[StrokePatternCombobox.SelectedIndex];
            strokePattern = pattern;
        }

        private void ColorPicker_SelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorAutoChange newColor = new ColorAutoChange
            {
                color = ColorPicker.Color
            };
            strokeColor = newColor;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML File|*.xml";
            saveFileDialog.Title = "Save File";
            saveFileDialog.ShowDialog();
            //string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shapes.xml");
            if (saveFileDialog.FileName != "")
            {
                SerializeInterface.SerializeShapes(_drawnShapes, saveFileDialog.FileName);
            }
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "XML File|*.xml";
            open.ShowDialog();
            if (open.FileName != "")
            {
                //string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shapes.xml");
                _drawnShapes = SerializeInterface.DeserializeShapes(open.FileName);
                //Xóa đi tất cả bản vẽ cũ
                canvas.Children.Clear();

                UIElement previewPic = new UIElement();
                //Vẽ lại các điểm đã lưu (convert nó thành list chứa UI element và loại
                foreach (var item in _drawnShapes)
                {
                    IPaintBusiness painter = _painterPrototypes[item.Name];
                    UIElement shape = painter.Draw(item); // vẽ ra tương ứng với loại entity
                    canvas.Children.Add(shape);
                    if (item.Equals(_drawnShapes.Last()))
                    {
                        previewPic = painter.Draw(item);
                        canvas.Children.Add(previewPic);
                    }
                }
            }
        }

        private void SaveAsButtonClick(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                                        (int)canvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);
            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 100, rtb.PixelWidth, rtb.PixelHeight - 100));
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            BitmapEncoder jpegEncoder = new JpegBitmapEncoder();
            BitmapEncoder bmpEncoder = new BmpBitmapEncoder();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpeg|Bitmap Image|*.bmp";
            saveFileDialog.Title = "Save an Image File";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {

                using System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();

                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                        pngEncoder.Frames.Add(BitmapFrame.Create(crop));
                        pngEncoder.Save(fs);
                        break;

                    case 2:
                        jpegEncoder.Frames.Add(BitmapFrame.Create(crop));
                        jpegEncoder.Save(fs);
                        break;

                    case 3:
                        bmpEncoder.Frames.Add(BitmapFrame.Create(crop));
                        bmpEncoder.Save(fs);
                        break;
                }

                fs.Close();
            }
        }

        private void LoadImageButtonClick(object sender, RoutedEventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpeg; *.png; *.bmp)|*.jpeg; *.png; *.bmp";
            open.ShowDialog();
            if (open.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)open.OpenFile();
                string filename = open.FileName;
                ImageDrawing newImage = new ImageDrawing();
                newImage.Rect = new Rect(0, 0, canvas.RenderSize.Width, canvas.RenderSize.Height - 100);
                newImage.ImageSource = new BitmapImage(new Uri(filename));

                var width = newImage.Bounds.Width;
                var height = newImage.Bounds.Height;

                var previewImageUI = new Image { Source = new DrawingImage(newImage) };
                var newImageUI = new Image { Source = new DrawingImage(newImage) };
                previewImageUI.Arrange(new Rect(0, 0, width, height));
                newImageUI.Arrange(new Rect(0, 0, width, height));

                canvas.Children.Add(previewImageUI);
                canvas.Children.Add(newImageUI);
                fs.Close();
            }
        }
    }
}