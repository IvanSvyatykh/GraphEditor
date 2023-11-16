using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Text;
using GraphEditor;

namespace Graph
{
    public class graph_view : property_base
    {
        [NonSerialized]
        private Canvas canvas;
        private List<edge_view> edgeList = new List<edge_view>();
       // private List<GraphLineViewModel> lineList = new List<GraphLineViewModel>();
        private List<node_view> TopList = new List<node_view>();
        private bool isClear = true;
        private bool isOriented = true;
        private bool orient = false;
        private bool edgeAdd; 
        private bool edgeDelete;
        private bool nodeDelete;
        private bool deijkstra;
        private bool topDelete;
        private bool showDeijkstra;
        private const int INFINITY = 10000;
        public node_view FirstTop;
        private Dictionary<int, int> distances_list;
        private Dictionary<int, List<int>> neighbors_nodes;
        private Dictionary<KeyValuePair<int, int>, int> edge_weight;
        public delegate void PointPositionChanged(node_view top);
        private Dictionary<node_view, Label> labels_list;

        public graph_view(Canvas canvas, bool orient)
        {
            this.canvas = canvas;
            this.orient = orient;
        }

        public List<node_view> Tops
        {
            get { return TopList; }
        }

        public bool IsEdgeAdd
        {
            get { return edgeAdd; }
        }
        public bool IsEdgeDelete
        {
            get { return edgeDelete; }
        }
        public bool CheckTopNum(node_view top)
        {
            foreach (node_view model in TopList)
                if (top.TopNum == model.TopNum && model != top)
                    return false;
            return true;
        }
        public void AddTop(bool center)
        {
            node_view top = new node_view(this);
            if (center)
            {
                top.CNTRPosition();
            }
            else
            {
                OnPointPositionChanged(top);
            }
            UpdateIsClear();
        }
        public Canvas GRCanvas
        {
            get { return canvas; }
            set { canvas = value; }
        }
        public void OnPointPositionChanged(node_view top)
        {
            top.UpdPos();
            Canvas.SetLeft(top.GRNode, top.RELPos.X);
            Canvas.SetTop(top.GRNode, top.RELPos.Y);
            if (labels_list != null)
            {
                Canvas.SetLeft(labels_list[top], top.RELPos.X + ViewNode.Radius / 2);
                Canvas.SetTop(labels_list[top], top.RELPos.Y - ViewNode.Radius * 1.5);
            }
        }
        public void DeleteNode(node_view top)
        {
            if (TopList.Contains(top))
            {

                foreach (edge_view model in edgeList)
                    if (model.From == top || model.To == top)
                    {
                        foreach (Shape line in model.Edge)
                            canvas.Children.Remove(line);
                        canvas.Children.Remove(model.TxtBox);
                        edgeList.Remove(model);
                    }
                TopList.Remove(top);
                canvas.Children.Remove(top.View);
            }
            UpdateIsClear();
        }
        public void DeleteEdge(edge_view line)
        {
            if (edgeList.Contains(line))
            {
                edgeList.Remove(line);
                foreach (Shape shape in line.Edge)
                    canvas.Children.Remove(shape);
                canvas.Children.Remove(line.TxtBox);
            }
            UpdateIsClear();
        }
        public void ValidateTopNumbers()
        {
            foreach (node_view top in TopList)
                top.Validate();
        }
        private void UpdateIsClear()
        {
           // IsClear = topList.Count == 0 && lineList.Count == 0;
        }

        public bool IsClear
        {
            get { return isClear; }
            set { isClear = value; OnPropertyChanged("IsClear"); }
        }
        public bool IsOriented
        {
            get { return isOriented; }
            set { isOriented = value; OnPropertyChanged("IsOriented"); }
        }
        public bool IsNodeDelete
        {
            get { return nodeDelete; }
        }
        public bool IsDeijkstra
        {
            get { return deijkstra; }
        }


        public void AddEdge(node_view from_node, node_view to_node)
        {
            edge_view line = new edge_view(this, from_node, to_node);
            edgeList.Add(line);
            FirstTop = null;
            EndAddEdge();
            to_node.UpdCurs();
            UpdateIsClear();
        }


        // node-edge  add-delete
        public void StartAddEdge()
        {
            if (showDeijkstra)
                EndShowDeijkstra();
            edgeAdd = true;
            canvas.Cursor = Cursors.ScrollAll;
        }
        public void EndAddEdge()
        {
            edgeAdd = false;
            FirstTop = null;
            canvas.Cursor = Cursors.Arrow;
        }
        public void StartDeleteEdge()
        {
            if (showDeijkstra)
                EndShowDeijkstra();
            edgeDelete = true;
            canvas.Cursor = Cursors.ScrollAll;
        }
        public void EndDeleteEdge()
        {
            edgeDelete = false;
            canvas.Cursor = Cursors.Arrow;
        }
        public void StartDeleteNode()
        {
            if (showDeijkstra)
                EndShowDeijkstra();
            nodeDelete = true;
            canvas.Cursor = Cursors.ScrollAll;
        }
        public void EndDeleteNode()
        {
            nodeDelete = false;
            canvas.Cursor = Cursors.Arrow;
        }



        //deijkstra
        public bool IsShowDeijkstra
        {
            get { return showDeijkstra; }
        }
        public void StartDeijkstra()
        {
            if (showDeijkstra)
                EndShowDeijkstra();
            deijkstra = true;
            canvas.Cursor = Cursors.ScrollAll;
        }
        public void EndDeijkstra()
        {
            deijkstra = false;
            canvas.Cursor = Cursors.Arrow;
        }
        public void EndShowDeijkstra()
        {
            showDeijkstra = false;
            foreach (Label label in labels_list.Values)
                canvas.Children.Remove(label);
            labels_list = null;
        }
        public void StartShowDeijkstra()
        {
            try
            {
                showDeijkstra = true;
                labels_list = new Dictionary<node_view, Label>();
                Label label;
                for (int i = 0; i < TopList.Count; ++i)
                {
                    label = new Label();
                    labels_list[TopList[i]] = label;
                    if (distances_list[TopList[i].TopNum] == 10000)
                    {
                        label.Content = "∞";
                    }
                    else
                    {
                        label.Content = distances_list[TopList[i].TopNum].ToString();
                    }
                    label.FontSize = 15;
                    label.Foreground = System.Windows.Media.Brushes.Blue;
                    canvas.Children.Add(label);
                    Canvas.SetLeft(label, TopList[i].RELPos.X + ViewNode.Radius / 2);
                    Canvas.SetTop(label, TopList[i].RELPos.Y - ViewNode.Radius * 1.5);
                    Canvas.SetZIndex(label, 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка виведення алгоритму Дейкстри");
                EndShowDeijkstra();
            }
        }
        public void Deijkstra(node_view from)
        {
            if (!ValidateState())
            {
                MessageBox.Show("Граф заданий невірно");
                return;
            }
            foreach (edge_view line in edgeList)
                if (line.Weight < 0)
                {
                    MessageBox.Show("Помилка виконання алгоритму:\nІснують ребра з від'ємною вагою");
                    return;
                }
            if (!BeginDeijkstra(from))
            {
                MessageBox.Show("Помилка при виконанні алгоритму Дейкстри");
                return;
            }
            StartShowDeijkstra();
        }
        private bool BeginDeijkstra(node_view from)
        {
            try
            {
                FillDictionaries();
                distances_list = new Dictionary<int, int>();
                List<int> went = new List<int>();
                foreach (int top in neighbors_nodes.Keys)
                    if (top != from.TopNum)
                        distances_list[top] = INFINITY;
                    else
                        distances_list[top] = 0;

                int curTop = from.TopNum;
                int pathSum;
                int index;
                List<int> list;
                int minDistance;
                while (went.Count != TopList.Count)
                {
                    foreach (int neighbor in neighbors_nodes[curTop])
                    {
                        pathSum = distances_list[curTop] + edge_weight[new KeyValuePair<int, int>(curTop, neighbor)];
                        if (pathSum < distances_list[neighbor])
                            distances_list[neighbor] = pathSum;
                    }
                    went.Add(curTop);
                    index = went.Count - 1;
                    do
                    {
                        if (curTop == INFINITY)
                            curTop = went[--index];
                        list = neighbors_nodes[curTop];
                        curTop = minDistance = INFINITY;
                        for (int i = 0; i < list.Count; ++i)
                        {
                            if (!went.Contains(list[i]) && distances_list[list[i]] < minDistance)
                            {
                                minDistance = distances_list[list[i]];
                                curTop = list[i];
                            }
                        }
                    }
                    while (curTop == INFINITY && index != 0);
                    if (index == -1 || curTop == INFINITY)
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool ValidateState()
        {
            foreach (edge_view line in edgeList)
                if (!line.isValid)
                    return false;
            foreach (node_view top in TopList)
                if (!top.IsValid)
                    return false;
            return true;
        }
        private void FillDictionaries()
        {
            neighbors_nodes = new Dictionary<int, List<int>>();
            edge_weight = new Dictionary<KeyValuePair<int, int>, int>();
            foreach (node_view top in TopList)
                neighbors_nodes[top.TopNum] = new List<int>();
            foreach (edge_view line in edgeList)
            {
                neighbors_nodes[line.From.TopNum].Add(line.To.TopNum);
                edge_weight[new KeyValuePair<int, int>(line.From.TopNum, line.To.TopNum)] = line.Weight;
                if (!IsOriented)
                {
                    neighbors_nodes[line.To.TopNum].Add(line.From.TopNum);
                    edge_weight[new KeyValuePair<int, int>(line.To.TopNum, line.From.TopNum)] = line.Weight;
                }
            }
        }
    }
}
