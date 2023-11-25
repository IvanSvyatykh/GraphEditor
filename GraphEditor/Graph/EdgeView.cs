using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GraphEditor;
using Model.Graph;
using System.Threading;

namespace Graph
{
    public class EdgeView
    {
        public bool isValid { get; set; }
        public int Weight
        {
            get { return edge_weight; }
        }

        public TextBox TxtBox
        {
            get { return textBox; }
        }

        public NodeView StartNode
        {
            get { return startNode; }
        }

        public NodeView EndNode
        {
            get { return endNode; }
        }

        public bool IsLine
        {
            get { return isLine; }
        }
        public Brush Color
        {
            get => baseColor;
            set
            {
                SetColor(value);
            }
        }

        public List<Shape> Edge
        {
            get
            {
                return Lines;
            }
        }

        private Shape Line;

        private Line LeftLine;
        private Line RightLine;

        private TextBox textBox;

        private Brush invisibleBrush;

        private List<Shape> Lines;
        
        private int edge_weight;
        private bool isLine;
        private bool isEdgeTwoWays;
        private Brush baseColor = Brushes.Black;

        private GraphView graph;
        private NodeView startNode;
        private NodeView endNode;

        public EdgeView(GraphView _graph, NodeView from_node, NodeView to_node, int weight, bool isEdgeTwoWays)
        {
            this.graph = _graph;

            if (from_node != to_node)
            {
                isLine = true;
            }
            else
            {
                isLine = false;
            }

            if (isLine)
            {
                Line = new Line();
            }
            else
            {
                Line = new Ellipse();
            }

            Line.MouseEnter += new MouseEventHandler(Line_MouseEnter);
            Line.MouseLeave += new MouseEventHandler(Line_MouseLeave);
            Line.MouseLeftButtonDown += new MouseButtonEventHandler(Line_MouseLeftButtonDown);
            Line.Stroke = baseColor;
            Line.StrokeThickness = 2;

            if (isLine)
            {
                ((Line)Line).X1 = 0;
                ((Line)Line).Y1 = 0;
                if (_graph.IsOriented)
                {
                    LeftLine = new Line();
                    RightLine = new Line();
                    RightLine.Stroke = LeftLine.Stroke = baseColor;

                    RightLine.StrokeThickness = LeftLine.StrokeThickness = 1.25;
                    
                    _graph.GraphCanvas.Children.Add(LeftLine);
                    _graph.GraphCanvas.Children.Add(RightLine);
                }
            }
            else
            {
                ((Ellipse)Line).Width = 50;
                ((Ellipse)Line).Height = 50;
            }


            if (Lines == null)
            {
                Lines = new List<Shape>();
                Lines.Add(Line);

                if (_graph.IsOriented)
                {
                    Lines.Add(LeftLine);
                    Lines.Add(RightLine);
                }
            }

            isValid = true;

            textBox = new TextBox();
            textBox.Width = 50;
            textBox.BorderBrush = new SolidColorBrush(Colors.Transparent);
            textBox.Background = new SolidColorBrush(Colors.Transparent);
            textBox.VerticalContentAlignment = VerticalAlignment.Center;
            textBox.HorizontalContentAlignment = HorizontalAlignment.Center;
            textBox.FontWeight = FontWeights.Bold;
            textBox.FontSize = 14;
            textBox.IsReadOnly = true;

            textBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(TxtBox_PreviewMouseLeftButtonDown);
            textBox.TextChanged += new TextChangedEventHandler(TxtBox_TextChanged);
            textBox.KeyDown += new KeyEventHandler(TxtBox_KeyDown);
            textBox.MouseEnter += new MouseEventHandler(TextBox_MouseEnter);
            textBox.MouseLeave += new MouseEventHandler(TextBox_MouseLeave);

            textBox.Text = weight.ToString();

            _graph.GraphCanvas.Children.Add(textBox);
            
            Canvas.SetZIndex(textBox, 2);
            this.isEdgeTwoWays = isEdgeTwoWays;


            _graph.GraphCanvas.Children.Add(Line);
            Canvas.SetZIndex(Line, 0);
            
            this.startNode = from_node;
            this.endNode = to_node;
            
            to_node.pointPositionChange += new GraphView.PointPositionChanged(OnPointPositionChanged);
            from_node.pointPositionChange += new GraphView.PointPositionChanged(OnPointPositionChanged);
            
            OnPointPositionChanged(to_node);
            
            if (isLine)
            {
                OnPointPositionChanged(from_node);
            }
            
            invisibleBrush = new SolidColorBrush(Colors.White);
            invisibleBrush.Opacity = 0;
        }

        private void SetColor(Brush color)
        {
            Line.Stroke = color;
            if (graph.IsOriented)
            {
                RightLine.Stroke = LeftLine.Stroke = color;
            }
            baseColor = color;
        }

        private void TxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {   
                Keyboard.ClearFocus();
                OnMouseLeave();
            }            
        }
        private void TxtBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseEnter();
        }
        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                edge_weight = int.Parse(textBox.Text);
                
                if (edge_weight >= 1000)
                {
                    throw new Exception();
                }
                if (edge_weight < 0)
                {
                    throw new Exception();
                }

                textBox.Foreground = Brushes.ForestGreen;
                isValid = true;
            }
            catch (Exception ex)
            {
                textBox.Foreground = Brushes.Red;
                isValid = false;
            }
        }
        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            OnMouseLeave();
        }
        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            OnMouseEnter();
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            OnMouseLeave();
        }
        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            OnMouseEnter();
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (graph.IsEdgeDeleting)
            {
                graph.DeleteEdge(this);
            }
        }

        private void OnMouseEnter()
        {
            if (!graph.IsTaskWork)
            {
                if (!graph.IsEdgeAdding && !graph.IsNodeAdding)
                {
                    Canvas.SetZIndex(Line, 1);
                    Line.Stroke = Brushes.Green;
                    if (isLine && graph.IsOriented)
                    {
                        LeftLine.Stroke = RightLine.Stroke = Brushes.Green;
                    }

                    if (!graph.IsEdgeDeleting)
                    {
                        textBox.IsReadOnly = false;

                        textBox.CaretBrush = Brushes.Black;
                        textBox.Background = Brushes.White;
                    }
                }
            }
        }
        private void OnMouseLeave()
        {
            if (!graph.IsEdgeAdding && !graph.IsNodeAdding && !graph.IsTaskWork)
            {
                Canvas.SetZIndex(Line, 0);
                Line.Stroke = Brushes.Black;
                if (isLine && graph.IsOriented)
                {
                    LeftLine.Stroke = RightLine.Stroke = Brushes.Black;
                }

                textBox.IsReadOnly = true;
                
                textBox.CaretBrush = invisibleBrush;
                textBox.Background = new SolidColorBrush(Colors.Transparent);
                
            }
        }

        public void OnPointPositionChanged(NodeView top)
        {
            if (isLine)
            {
                if (top == startNode)
                {
                    ((Line)Line).X1 = startNode.Position.X + startNode.ViewPartNode.Width / 2;
                    ((Line)Line).Y1 = startNode.Position.Y + ViewNode.NodeRadius / 2;
                }

                ((Line)Line).X2 = endNode.Position.X + startNode.ViewPartNode.Width / 2;
                ((Line)Line).Y2 = endNode.Position.Y + ViewNode.NodeRadius / 2;

                double hypotenuse = Math.Sqrt(Math.Abs(((Line)Line).X2 - ((Line)Line).X1) * Math.Abs(((Line)Line).X2 - ((Line)Line).X1) +
                    Math.Abs(((Line)Line).Y2 - ((Line)Line).Y1) * Math.Abs(((Line)Line).Y2 - ((Line)Line).Y1));

                double angle = Math.Asin(Math.Abs(((Line)Line).Y2 - ((Line)Line).Y1) / hypotenuse);
                
                if (graph.IsOriented)
                {
                    if (angle <= 7 * Math.PI / 24)
                    {
                        Canvas.SetLeft(textBox, startNode.Position.X + 0.65 * (((Line)Line).X2 - ((Line)Line).X1));
                    }
                    else
                    {
                        Canvas.SetLeft(textBox, startNode.Position.X + 12 + 0.65 * (((Line)Line).X2 - ((Line)Line).X1));
                    }

                    if (isEdgeTwoWays)
                    {
                        Canvas.SetTop(textBox, startNode.Position.Y + 0.65 * (((Line)Line).Y2 - ((Line)Line).Y1) + textBox.FontSize / 3 + 10);
                    }
                    else
                    {
                        Canvas.SetTop(textBox, startNode.Position.Y + 0.65 * (((Line)Line).Y2 - ((Line)Line).Y1) - textBox.FontSize / 3);
                    }
                }
                else
                {
                    if (angle <= 7 * Math.PI / 24)
                    {
                        Canvas.SetLeft(textBox, startNode.Position.X + 0.5 * (((Line)Line).X2 - ((Line)Line).X1));
                    }
                    else
                    {
                        Canvas.SetLeft(textBox, startNode.Position.X + 12 + 0.5 * (((Line)Line).X2 - ((Line)Line).X1));
                    }

                    Canvas.SetTop(textBox, startNode.Position.Y + 0.5 * (((Line)Line).Y2 - ((Line)Line).Y1) - textBox.FontSize / 3);
                    
                }
                
                    
                
                if (graph.IsOriented)
                {
                    double u_l = Math.Atan2(((Line)Line).X1 - ((Line)Line).X2, ((Line)Line).Y1 - ((Line)Line).Y2);
                    double u = Math.PI / 33;

                    LeftLine.X1 = ((Line)Line).X2 + 10 * Math.Sin(u_l);
                    LeftLine.Y1 = ((Line)Line).Y2 + 10 * Math.Cos(u_l);

                    LeftLine.X2 = ((Line)Line).X2 + 30 * Math.Sin(u_l + 2 * u);
                    LeftLine.Y2 = ((Line)Line).Y2 + 30 * Math.Cos(u_l + 2 * u);

                    RightLine.X1 = ((Line)Line).X2 + 10 * Math.Sin(u_l);
                    RightLine.Y1 = ((Line)Line).Y2 + 10 * Math.Cos(u_l);

                    RightLine.X2 = ((Line)Line).X2 + 30 * Math.Sin(u_l - 2 * u);
                    RightLine.Y2 = ((Line)Line).Y2 + 30 * Math.Cos(u_l - 2 * u);
                }
            }
            else
            {
                Canvas.SetLeft(Line, startNode.Position.X);
                Canvas.SetTop(Line, startNode.Position.Y - 35);
                
                Canvas.SetLeft(textBox, startNode.Position.X + 10);
                Canvas.SetTop(textBox, startNode.Position.Y - 35);
               
            }
        }
    }
}
