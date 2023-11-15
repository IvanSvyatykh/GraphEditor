using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Border b = new Border();
            
            Line line = new Line();
            line.X1 = 100;
            line.Y1 = 100;
            line.X2 = 1000;
            line.Y2 = 1000;
            line.Stroke = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            line.StrokeThickness = 10;
            

            //Ellipse a = new Ellipse();
            //a.Height = 10;
            //a.Width = 10;
            //a.Fill = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            //Rectangle a = new Rectangle();
            //a.Width = 40;
            //a.Height = 40;
            //a.Fill = new SolidColorBrush(Color.FromRgb(211, 211, 211));

            b.Child = line;
            b.MouseDown += B_MouseDown;
            
            
            Canvas.Children.Add(b);
         
        }

        private void B_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Николаев идёт нахуй сука во второй раз");
        }
    }
}
