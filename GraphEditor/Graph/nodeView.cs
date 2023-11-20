using GraphEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Graph
{
    public class NodeView
    {
        private string name;

        private ViewNode graphNode;
        private GraphView graphView;

        private bool isValid;
        //private bool move;

        public string txt;

        private bool isNodeMove;
        private Point position;


        public event GraphView.PointPositionChanged pointPositionChange;

        public Point Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public bool IsNodeMove
        {
            get
            {
                return isNodeMove;
            }
            set
            {
                isNodeMove = value;
            }
        }
        public GraphView Graph
        {
            get { return graphView; }
        }

        public ViewNode ViewPartNode
        {
            get { return graphNode; }
        }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                isValid = value;
                if (isValid)
                {
                    graphNode.TextBoxForNodeLabel.Foreground = System.Windows.Media.Brushes.Black;
                    graphNode.TextBoxForNodeLabel.BorderBrush = System.Windows.Media.Brushes.Transparent;
                }
                else
                {
                    graphNode.TextBoxForNodeLabel.Foreground = System.Windows.Media.Brushes.Red;
                    graphNode.TextBoxForNodeLabel.BorderBrush = System.Windows.Media.Brushes.Red;
                }
            }
        }

        public string NodeName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }


        public NodeView(GraphView graphView,string uniqueName)
        {
            this.graphView = graphView;

            graphNode = new ViewNode(this);
            graphNode.TextBoxForNodeLabel.Text = uniqueName;

            IsValid = true;
        }

        public void OnMouseLeave()
        {
            if (isNodeMove)
                pointPositionChange(this);
        }

        public void OnMouseMove()
        {
            if (isNodeMove)
                pointPositionChange(this);
        }

        public void UpdPos()
        {
            Point p = Mouse.GetPosition(graphView.GraphCanvas);

            position = new Point(p.X - ViewPartNode.Width / 2, p.Y - ViewPartNode.Height / 4);
            
            if (p.X < ViewPartNode.Width / 2)
            {
                position.X = 0;
            }
            else if (p.X + ViewPartNode.Width / 2 > graphView.GraphCanvas.ActualWidth)
            {
                position.X = graphView.GraphCanvas.ActualWidth - ViewPartNode.Width;
            }

            if(p.Y < ViewPartNode.Height / 4)
            {
                position.Y = 0;
            }
            else if(p.Y + ViewPartNode.Height / 2 + 10 > graphView.GraphCanvas.ActualHeight)
            {
                position.Y = graphView.GraphCanvas.ActualHeight - ViewPartNode.Height;
            }
        }

        public void Validate()
        {
            name = txt;

            if (Graph.CheckNameIsUnique(this))
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }
    }
}
