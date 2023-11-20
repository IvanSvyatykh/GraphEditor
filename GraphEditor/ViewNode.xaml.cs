using Graph;
using System.Security.AccessControl;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            Canvas.SetZIndex(this, 2);
            this.parentViewNode = parentViewNode;
            
            this.point.Width = this.point.Height = NodeRadius;
            invisibleBrush = new SolidColorBrush(Colors.White);
            invisibleBrush.Opacity = 0;
        }

        SolidColorBrush invisibleBrush;
        public const int NodeRadius = 25;
        private Brush pointColor = Brushes.Blue; 
        private NodeView parentViewNode;

        private void TxtBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            parentViewNode.txt = TextBoxForNodeLabel.Text;

            parentViewNode.Validate();
            parentViewNode.Graph.ValidateNamesIsUnique();
        }

        public void SetColor(Brush color)
        {
            point.Fill = color;
            pointColor = color;
        }

        private void NodeMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentViewNode.IsNodeMove = false;
        }
        private void NodeMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            if (parentViewNode.Graph.IsEdgeAdding)
            {
                if (parentViewNode.Graph.FirstTop == null)
                    parentViewNode.Graph.FirstTop = parentViewNode;
                else
                {
                    parentViewNode.Graph.AddEdge(parentViewNode.Graph.FirstTop, parentViewNode);
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
        private void NodeMouseEnter(object sender, MouseEventArgs e)
        {
            if (!parentViewNode.Graph.IsNodeAdding)
            {
                point.Fill = Brushes.LightBlue;
            }
        }
        private void NodeMouseLeave(object sender, MouseEventArgs e)
        {   
            point.Fill = pointColor;
            
            parentViewNode.OnMouseLeave();
        }
        private void NodeMouseMove(object sender, MouseEventArgs e)
        {
            parentViewNode.OnMouseMove();
        }

        private void TxtBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                parentViewNode.txt = TextBoxForNodeLabel.Text;
                TextBoxForNodeLabel.CaretBrush = invisibleBrush;
                Keyboard.ClearFocus();
            }
        }
        private void TxtBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            TextBoxForNodeLabel.CaretBrush = Brushes.Black;
        }
    }
}
