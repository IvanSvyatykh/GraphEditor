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
        private bool move;
        private bool isValid;
        public string txt;
        private bool _move;
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

        public GraphView Graph
        {
            get { return graphView; }
        }

        public ViewNode View
        {
            get { return graphNode; }
        }

        public NodeView(GraphView graphView)
        {
            this.graphView = graphView;

            graphNode = new ViewNode(this);
            graphNode.textBox1.Text = "";

            IsValid = false;
        }

        public bool Move
        {
            get
            {
                return move;
            }
            set
            {
                _move = value;
            }
        }

        public void OnMouseLeave()
        {
            if (_move)
                pointPositionChange(this);
        }

        public void OnMouseMove()
        {
            if (_move)
                pointPositionChange(this);
        }

        public void UpdCurs()
        {
            graphNode.point.Cursor = Cursors.Hand;
        }

        public void UpdPos()
        {
            Point p = Mouse.GetPosition(graphView.GraphCanvas);
            position = new Point(p.X - GRNode.Width / 2, p.Y - GRNode.Height / 4);
        }

        public ViewNode GRNode
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
                    graphNode.textBox1.Foreground = System.Windows.Media.Brushes.Black;
                }
                else
                {
                    graphNode.textBox1.Foreground = System.Windows.Media.Brushes.Red;
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
