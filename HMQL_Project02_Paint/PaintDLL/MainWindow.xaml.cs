using IContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintDLL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //State
        private bool _isDrawing = false;

        private string _currentType = "";

        private IShapeEntity _preview = null;

        private Point _start;

        private List<IShapeEntity> _drawnShapes = new List<IShapeEntity>();

        private Dictionary<string, IPaintBusiness> _painterPrototypes = new Dictionary<string, IPaintBusiness>();
        private Dictionary<string, IShapeEntity> _shapePrototypes = new Dictionary<string, IShapeEntity>();

        public MainWindow()
        {
            InitializeComponent();
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
                button.Width = 80;
                button.Height = 35;
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

            //Hiển thị entity đã tìm ra
            //foreach (IShapeEntity entity in list)
            //{
            //    Console.WriteLine(entity.Name);
            //}
        }

        //đổi lựa chọn
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var entity = button!.Tag as IShapeEntity;

            _currentType = entity.Name;
            _preview = (_shapePrototypes[entity.Name].Clone() as IShapeEntity)!;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;

            _start = e.GetPosition(canvas); // Lưu điểm bắt dầu

            _preview.HandleStart(_start);
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                var end = e.GetPosition(canvas);
                _preview.HandleEnd(end);

                //Xoá đi điểm cũ và vẽ lại điểm trước đó
                canvas.Children.Clear();

                foreach (var item in _drawnShapes)
                {
                    var painter = _painterPrototypes[item.Name];
                    //vẽ tương ứng với loại entity
                    var shape = painter.Draw(item);

                    canvas.Children.Add(shape);
                }

                var previewPainter = _painterPrototypes[_preview.Name];

                var previewElement = previewPainter.Draw(_preview);

                //Canvas.SetLeft(line, _start.X);
                //Canvas.SetTop(line, _start.Y);

                canvas.Children.Add(previewElement);
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;

            var end = e.GetPosition(canvas);

            _preview.HandleEnd(end);

            _drawnShapes.Add(_preview.Clone() as IShapeEntity);
        }
    }
}