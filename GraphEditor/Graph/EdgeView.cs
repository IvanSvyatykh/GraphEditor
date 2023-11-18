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

namespace Graph
{
    public class EdgeView
    {
        private Shape Line;

        private Line LeftLine;
        private Line RightLine;

        private TextBox textBox1;

        private Brush opaqueBrush;
        private Brush borderBrush;

        private List<Shape> Lines;
        
        private int edge_weight;
        private bool isLine;
        
        private GraphView graph;
        private NodeView from_node;
        private NodeView to_node;



        Thickness th = new Thickness(1.1);
        Brush br;

        public EdgeView(GraphView _graph, NodeView from_node, NodeView to_node)
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
            Line.Stroke = Brushes.Black;
            Line.StrokeThickness = 2;

            if (isLine)
            {
                ((Line)Line).X1 = 0;
                ((Line)Line).Y1 = 0;
                if (_graph.IsOriented)
                {
                    LeftLine = new Line();
                    RightLine = new Line();
                    RightLine.Stroke = LeftLine.Stroke = Brushes.Black;

                    RightLine.StrokeThickness = LeftLine.StrokeThickness = 7;
                    
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


            textBox1 = new TextBox();
            textBox1.Width = 50;
            textBox1.BorderBrush = new SolidColorBrush(Colors.Transparent);
            textBox1.VerticalContentAlignment = VerticalAlignment.Center;
            textBox1.HorizontalContentAlignment = HorizontalAlignment.Center;
            textBox1.FontSize = 14;

            TxtBox1_TextChanged(null, null);

            textBox1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(TxtBox1_PreviewMouseLeftButtonDown);
            textBox1.TextChanged += new TextChangedEventHandler(TxtBox1_TextChanged);
            textBox1.KeyDown += new KeyEventHandler(TxtBox1_KeyDown);
            textBox1.Text = "empty";
            
            opaqueBrush = new SolidColorBrush(Colors.Black);
            opaqueBrush.Opacity = 0;

            _graph.GraphCanvas.Children.Add(textBox1);
            _graph.GraphCanvas.Children.Add(Line);
            
            Canvas.SetZIndex(Line, 0);
            Canvas.SetZIndex(textBox1, 2);
            
            this.from_node = from_node;
            this.to_node = to_node;
            
            to_node.pointPositionChange += new GraphView.PointPositionChanged(OnPointPositionChanged);
            from_node.pointPositionChange += new GraphView.PointPositionChanged(OnPointPositionChanged);
            
            OnPointPositionChanged(to_node);
            
            if (isLine)
            {
                OnPointPositionChanged(from_node);
            }
        }

        public bool isValid { get; set; }
        public int Weight
        {
            get { return edge_weight; }
        }

        public TextBox TxtBox
        {
            get { return textBox1; }
        }

        public NodeView From
        {
            get { return from_node; }
        }

        public NodeView To
        {
            get { return to_node; }
        }

        public bool IsLine
        {
            get { return isLine; }
        }

        public List<Shape> Edge
        {
            get
            {
                return Lines;
            }
        }

        private void TxtBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {   
                Brush br;
                br = new SolidColorBrush(Colors.Transparent);
                textBox1.BorderBrush = br;

                Keyboard.ClearFocus();
                
                OnMouseLeave();
            }            
        }
        private void TxtBox1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            br = new SolidColorBrush(Colors.Red);
            textBox1.BorderBrush = br;
            
            OnMouseEnter();
        }
        private void TxtBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                edge_weight = int.Parse(textBox1.Text);
                
                if (edge_weight >= 1000)
                {
                    throw new Exception();
                }
                if (edge_weight < 0)
                {
                    throw new Exception();
                }

                textBox1.Foreground = Brushes.Green;
                isValid = true;
            }
            catch (Exception ex)
            {
                textBox1.Foreground = Brushes.Red;
                isValid = false;
            }
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
            if (!graph.IsEdgeAdding && !graph.IsNodeAdding)
            {
                Line.Stroke = Brushes.Green;
                if (isLine && graph.IsOriented)
                {
                    LeftLine.Stroke = RightLine.Stroke = Brushes.Green;
                }

                foreach (Shape shape in Lines)
                {
                    Canvas.SetZIndex(Line, 1);
                }

                textBox1.IsReadOnly = false;

                textBox1.CaretBrush = Brushes.Black;
                textBox1.Background = Brushes.White;
            } 
        }
        private void OnMouseLeave()
        {
            if (!graph.IsEdgeAdding && !graph.IsNodeAdding)
            {
                Line.Stroke = Brushes.Black;
                if (isLine && graph.IsOriented)
                {
                    LeftLine.Stroke = RightLine.Stroke = Brushes.Black;
                }

                foreach (Shape shape in Lines)
                {
                    Canvas.SetZIndex(Line, 0);
                }

                textBox1.CaretBrush = textBox1.Background = opaqueBrush;
                textBox1.IsReadOnly = true;
            }
        }

        public void OnPointPositionChanged(NodeView top)
        {
            if (isLine)
            {
                if (top == from_node)
                {
                    Canvas.SetLeft(Line, from_node.Position.X + from_node.ViewPartNode.Width / 2);
                    Canvas.SetTop(Line, from_node.Position.Y + ViewNode.NodeRadius / 2);
                }

                ((Line)Line).X2 = to_node.Position.X - from_node.Position.X;
                ((Line)Line).Y2 = to_node.Position.Y - from_node.Position.Y;

                Canvas.SetLeft(textBox1, from_node.Position.X + from_node.ViewPartNode.Width / 2 + ((Line)Line).X2 / 2);
                Canvas.SetTop(textBox1, from_node.Position.Y + ((Line)Line).Y2 / 2 - textBox1.FontSize / 3);

                if (graph.IsOriented)
                {
                    double u_l = Math.Atan2(((Line)Line).X1 - ((Line)Line).X2, ((Line)Line).Y1 - ((Line)Line).Y2);
                    double u = Math.PI / 33;

                    LeftLine.StrokeThickness = 1;
                    RightLine.StrokeThickness = 1;

                    LeftLine.X1 = ((Line)Line).X2 + 10 * Math.Sin(u_l);
                    LeftLine.Y1 = ((Line)Line).Y2 + 10 * Math.Cos(u_l);

                    LeftLine.X2 = ((Line)Line).X2 + 30 * Math.Sin(u_l + 2 * u);
                    LeftLine.Y2 = ((Line)Line).Y2 + 30 * Math.Cos(u_l + 2 * u);

                    RightLine.X1 = ((Line)Line).X2 + 10 * Math.Sin(u_l);
                    RightLine.Y1 = ((Line)Line).Y2 + 10 * Math.Cos(u_l);

                    RightLine.X2 = ((Line)Line).X2 + 30 * Math.Sin(u_l - 2 * u);
                    RightLine.Y2 = ((Line)Line).Y2 + 30 * Math.Cos(u_l - 2 * u);

                    Canvas.SetLeft(LeftLine, from_node.Position.X + to_node.ViewPartNode.Width / 2);
                    Canvas.SetTop(LeftLine, from_node.Position.Y + ViewNode.NodeRadius / 2);
                    
                    Canvas.SetLeft(RightLine, from_node.Position.X + to_node.ViewPartNode.Width / 2);
                    Canvas.SetTop(RightLine, from_node.Position.Y + ViewNode.NodeRadius / 2);
                }
            }
            else
            {
                Canvas.SetLeft(Line, from_node.Position.X);
                Canvas.SetTop(Line, from_node.Position.Y - 35);

                Canvas.SetLeft(textBox1, from_node.Position.X + 10);
                Canvas.SetTop(textBox1, from_node.Position.Y - 35);
            }
        }
    }
}
