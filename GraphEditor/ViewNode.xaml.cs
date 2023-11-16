using Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphEditor
{
    /// <summary>
    /// Interaction logic for graphNode.xaml
    /// </summary>
    public partial class ViewNode : UserControl
    {
        public ViewNode(node_view parentView)
        {
            InitializeComponent();
            //this.parentViewModel = parentViewModel;
            this.parentView = parentView;
            Canvas.SetZIndex(this, 2);
            this.point.Width = this.point.Height = NodeRadius;
            opaqueBrush = new SolidColorBrush(Colors.Black);
            opaqueBrush.Opacity = 0;
        }

        private static int NodeRadius = 25;
        private node_view parentView;
       // private static int ellipseRadius = 20;
        private Brush opaqueBrush;
        bool isMove = false;

        public static int Radius
        {
            get { return NodeRadius; }
        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (parentView.Graph.IsShowDeijkstra)
            {
                parentView.Graph.EndShowDeijkstra();
            }
            parentView.txt = textBox1.Text;
            parentView.Validate();
            parentView.Graph.ValidateTopNumbers();
        }

        private void node_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //System.Windows.Point position = e.GetPosition(this.node);
            //double pX = position.X;
            //double pY = position.Y;
            //textBox1.Text = pX.ToString()+" "+pY.ToString();
            //var location = node.PointToScreen(new Point(0, 0));

           
            //location.X = pX;
            //location.Y = pY;
            isMove = false;
            parentView.Move = false;
        }
        private void node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //isMove = true;
            if (parentView.Graph.IsEdgeAdd)
            {
                if (parentView.Graph.FirstTop == null)
                    parentView.Graph.FirstTop = parentView;
                else
                {
                    parentView.Graph.AddEdge(parentView.Graph.FirstTop, parentView);
                    parentView.Graph.EndAddEdge();
                }
            }
            else if (parentView.Graph.IsNodeDelete)
            {
                parentView.Graph.DeleteNode(parentView);
                parentView.Graph.EndDeleteNode();
            }
            else if (parentView.Graph.IsDeijkstra)
            {
                parentView.Graph.Deijkstra(parentView);
                parentView.Graph.EndDeijkstra();
            }
            else
                parentView.Move = true;
        }
        private void node_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (!parentView.Graph.IsTopDelete && !parentView.Graph.IsLineAdd && !parentView.Graph.IsDejkstra)
            //{
                point.Cursor = Cursors.Hand;
            //}
            //else
            //{
            //    point.Cursor = Cursors.ScrollAll;
            //}
            point.Fill = Brushes.LightBlue;
            textBox1.IsReadOnly = false;
            //TxtBox_MouseEnter(null, null);
        }
        private void node_MouseLeave(object sender, MouseEventArgs e)
        {
            point.Fill = Brushes.Blue;
            parentView.OnMouseLeave();
            //TxtBox_MouseLeave(null, null);
        }
        private void node_MouseMove(object sender, MouseEventArgs e)
        {
            
            System.Windows.Point position = e.GetPosition(this.node);
            double pX = position.X+4;
            double pY = position.Y+4;
            //Canvas.SetLeft(node, 29.0);
            //Canvas.SetTop(node,15.5);
           // textBox1.Text = pX.ToString() + " " + pY.ToString();
            //System.Windows.Point position = e.GetPosition(this);
            //double pX = position.X;
            //double pY = position.Y;

            //var location = node.PointToScreen(new Point(0, 0));

            //location.X = pX;
            //location.Y = pY;
        
            parentView.OnMouseMove();
        }

        private void TxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                parentView.txt = textBox1.Text;
                textBox1.IsReadOnly = true;
                textBox1.CaretBrush = opaqueBrush;
                textBox1.Background = opaqueBrush;
                Canvas.SetZIndex(this, 2);
            }
        }
        private void TxtBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox1.IsReadOnly = false;
            textBox1.CaretBrush = Brushes.Black;
            textBox1.Background = Brushes.White;
            Canvas.SetZIndex(this, 4);
        }

    }
}
