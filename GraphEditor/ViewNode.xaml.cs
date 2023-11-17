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
        public ViewNode(NodeView parentViewNode)
        {
            InitializeComponent();
            Keyboard.ClearFocus();

            this.parentViewNode = parentViewNode;
            
            this.point.Width = this.point.Height = NodeRadius;
            invisibleBrush = new SolidColorBrush(Colors.White);
            invisibleBrush.Opacity = 0;
        }

        SolidColorBrush invisibleBrush;
        public const int NodeRadius = 25;

        private NodeView parentViewNode;

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentViewNode.txt = textBox1.Text;

            parentViewNode.Validate();
            parentViewNode.Graph.ValidateNamesIsUnique();
        }

        private void node_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentViewNode.IsNodeMove = false;
        }
        private void node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            if (parentViewNode.Graph.IsEdgeAdding)
            {
                if (parentViewNode.Graph.FirstTop == null)
                    parentViewNode.Graph.FirstTop = parentViewNode;
                else
                {
                    parentViewNode.Graph.AddEdge(parentViewNode.Graph.FirstTop, parentViewNode);
                    parentViewNode.Graph.EndAddingEdge();
                }
            }
            else if (parentViewNode.Graph.IsNodeDeleting)
            {
                parentViewNode.Graph.DeleteNode(parentViewNode);
            }
            else if (!parentViewNode.Graph.IsNodeAdding)
            {
                parentViewNode.IsNodeMove = true;
            }
        }
        private void node_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!parentViewNode.Graph.IsNodeAdding)
            {
                point.Cursor = Cursors.Hand;

                point.Fill = Brushes.LightBlue;
                textBox1.IsReadOnly = false;
            }
        }
        private void node_MouseLeave(object sender, MouseEventArgs e)
        {   
            point.Fill = Brushes.Blue;
            
            parentViewNode.OnMouseLeave();
        }
        private void node_MouseMove(object sender, MouseEventArgs e)
        {
            parentViewNode.OnMouseMove();
        }

        private void TxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                parentViewNode.txt = textBox1.Text;
                textBox1.CaretBrush = invisibleBrush;
                Keyboard.ClearFocus();
            }
        }
        private void TxtBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox1.CaretBrush = Brushes.Black;
        }
    }
}
