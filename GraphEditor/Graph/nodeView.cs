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
    [Serializable]
    public class nodeView
    {
        private int topNum;


        private ViewNode _grNode;
        private graphView _graph_view;
        private bool move;
        private bool isValid;
        public string txt;
        private bool _move;
        private Point rel_pos;


        public event graphView.PointPositionChanged pointPositionChange;

        public Point RELPos
        {
            get
            {
                return rel_pos;
            }
            set
            {
                rel_pos = value;
            }
        }

        public graphView Graph
        {
            get { return _graph_view; }
        }

        public ViewNode View
        {
            get { return _grNode; }
        }

        public nodeView(graphView _graph_view)
        {
            this._graph_view = _graph_view;
            _graph_view.Tops.Add(this);
            _grNode = new ViewNode(this);
            _grNode.textBox1.Text = "null";
            _graph_view.GRCanvas.Children.Add(_grNode);
            pointPositionChange += _graph_view.OnPointPositionChanged;
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
            _grNode.point.Cursor = Cursors.Hand;
        }

        public void UpdPos()
        {
            Point p = Mouse.GetPosition(_graph_view.GRCanvas);
            rel_pos = new Point(p.X - GRNode.Width / 2, p.Y - GRNode.Height / 4);
        }

        public ViewNode GRNode
        {
            get { return _grNode; }
        }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                isValid = value;
                if (isValid)
                    _grNode.textBox1.Foreground = System.Windows.Media.Brushes.Black;
                else
                    _grNode.textBox1.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        public int TopNum
        {
            get
            {
                return topNum;
            }
            set
            {
                topNum = value;
            }
        }
    }
}
