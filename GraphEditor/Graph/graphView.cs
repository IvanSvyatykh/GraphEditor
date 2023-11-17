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
using Model.Graph;

namespace Graph
{
    public class GraphView : property_base
    {
        private Canvas canvas;

        private List<EdgeView> edgeList = new List<EdgeView>();
        private List<NodeView> nodeList = new List<NodeView>();

        private bool isOriented = true;

        private bool isNodeAdding;
        private bool isNodeDeleting;
        private bool isEdgeAdding; 
        private bool isEdgeDeleting;
        
        
        public NodeView FirstTop;

        //Не знаю нужен ли он мне
        private Dictionary<string, List<string>> neighbors_nodes;

        //Нужно при валидации
        private Dictionary<KeyValuePair<string, string>, int> edge_weight;

        public delegate void PointPositionChanged(NodeView top);


        public GraphView(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public List<NodeView> Tops
        {
            get { return nodeList; }
        }
        public Canvas GraphCanvas
        {
            get { return canvas; }
            set { canvas = value; }
        }
        public bool IsNodeAdding
        {
            get { return isNodeAdding; }
        }
        public bool IsNodeDeleting
        {
            get { return isNodeDeleting; }
        }
        public bool IsEdgeAdding
        {
            get { return isEdgeAdding; }
        }
        public bool IsEdgeDeleting
        {
            get { return isEdgeDeleting; }
        }
        public bool IsOriented
        {
            get { return isOriented; }
            set { isOriented = value; OnPropertyChanged("IsOriented"); }
        }
        
        public bool CheckNameIsUnique(NodeView node)
        {
            foreach (NodeView anotherNode in nodeList)
            {
                if (node.NodeName == anotherNode.NodeName && anotherNode != node)
                {
                    return false;
                }
            }
            return true;
        }
        public void ValidateNamesIsUnique()
        {
            foreach (NodeView node in nodeList)
            {
                node.Validate();
            }
        }

        public void AddNode()
        {   
            NodeView newNode = new NodeView(this);

            nodeList.Add(newNode);
            canvas.Children.Add(newNode.ViewPartNode);
            newNode.pointPositionChange += OnPointPositionChanged;

            OnPointPositionChanged(newNode);
        }
        public void OnPointPositionChanged(NodeView node)
        {   
            node.UpdPos();
            Canvas.SetLeft(node.ViewPartNode, node.Position.X);
            Canvas.SetTop(node.ViewPartNode, node.Position.Y);
        }
        public void DeleteNode(NodeView node)
        {
            if (nodeList.Contains(node))
            {
                foreach (EdgeView model in edgeList)
                    if (model.From == node || model.To == node)
                    {
                        foreach (Shape line in model.Edge)
                            canvas.Children.Remove(line);
                        canvas.Children.Remove(model.TxtBox);
                        edgeList.Remove(model);
                    }
                nodeList.Remove(node);
                canvas.Children.Remove(node.ViewPartNode);
            }
        }

        public void AddEdge(NodeView from_node, NodeView to_node)
        {
            EdgeView line = new EdgeView(this, from_node, to_node);
            edgeList.Add(line);

            FirstTop = null;

            //EndAddingEdge();
        }
        public void DeleteEdge(EdgeView line)
        {
            if (edgeList.Contains(line))
            {
                edgeList.Remove(line);
                foreach (Shape shape in line.Edge)
                    canvas.Children.Remove(shape);
                canvas.Children.Remove(line.TxtBox);
            }
        }

        // node-edge  add-delete
        public void StartAddingNode()
        {
            isNodeAdding = true;
        }
        public void EndAddingNode()
        {
            isNodeAdding = false;
        }
        public void StartDeletingNode()
        {
            isNodeDeleting = true;
        }
        public void EndDeletingNode()
        {
            isNodeDeleting = false;
        }
        public void StartAddingEdge()
        {
            isEdgeAdding = true;
            canvas.Cursor = Cursors.ScrollAll;
        }
        public void EndAddingEdge()
        {
            isEdgeAdding = false;
            FirstTop = null;
            canvas.Cursor = Cursors.Arrow;
        }
        public void StartDeletingEdge()
        {
            isEdgeDeleting = true;
            canvas.Cursor = Cursors.ScrollAll;
        }
        public void EndDeletingEdge()
        {
            isEdgeDeleting = false;
            canvas.Cursor = Cursors.Arrow;
        }

        //Можно использовать для Бека

        ////deijkstra
        //public bool IsShowDeijkstra
        //{
        //    get { return showDeijkstra; }
        //}
        //public void StartDeijkstra()
        //{
        //    if (showDeijkstra)
        //        EndShowDeijkstra();
        //    deijkstra = true;
        //    canvas.Cursor = Cursors.ScrollAll;
        //}
        //public void EndDeijkstra()
        //{
        //    deijkstra = false;
        //    canvas.Cursor = Cursors.Arrow;
        //}
        //public void EndShowDeijkstra()
        //{
        //    showDeijkstra = false;
        //    foreach (Label label in labels_list.Values)
        //        canvas.Children.Remove(label);
        //    labels_list = null;
        //}
        //public void StartShowDeijkstra()
        //{
        //    try
        //    {
        //        showDeijkstra = true;
        //        labels_list = new Dictionary<node_view, Label>();
        //        Label label;
        //        for (int i = 0; i < TopList.Count; ++i)
        //        {
        //            label = new Label();
        //            labels_list[TopList[i]] = label;
        //            if (distances_list[TopList[i].TopNum] == 10000)
        //            {
        //                label.Content = "∞";
        //            }
        //            else
        //            {
        //                label.Content = distances_list[TopList[i].TopNum].ToString();
        //            }
        //            label.FontSize = 15;
        //            label.Foreground = System.Windows.Media.Brushes.Blue;
        //            canvas.Children.Add(label);
        //            Canvas.SetLeft(label, TopList[i].RELPos.X + ViewNode.Radius / 2);
        //            Canvas.SetTop(label, TopList[i].RELPos.Y - ViewNode.Radius * 1.5);
        //            Canvas.SetZIndex(label, 2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Помилка виведення алгоритму Дейкстри");
        //        EndShowDeijkstra();
        //    }
        //}
        //public void Deijkstra(node_view from)
        //{
        //    if (!ValidateState())
        //    {
        //        MessageBox.Show("Граф заданий невірно");
        //        return;
        //    }
        //    foreach (edge_view line in edgeList)
        //        if (line.Weight < 0)
        //        {
        //            MessageBox.Show("Помилка виконання алгоритму:\nІснують ребра з від'ємною вагою");
        //            return;
        //        }
        //    if (!BeginDeijkstra(from))
        //    {
        //        MessageBox.Show("Помилка при виконанні алгоритму Дейкстри");
        //        return;
        //    }
        //    StartShowDeijkstra();
        //}
        //private bool BeginDeijkstra(node_view from)
        //{
        //    try
        //    {
        //        FillDictionaries();
        //        distances_list = new Dictionary<int, int>();
        //        List<int> went = new List<int>();
        //        foreach (int top in neighbors_nodes.Keys)
        //            if (top != from.TopNum)
        //                distances_list[top] = INFINITY;
        //            else
        //                distances_list[top] = 0;

        //        int curTop = from.TopNum;
        //        int pathSum;
        //        int index;
        //        List<int> list;
        //        int minDistance;
        //        while (went.Count != TopList.Count)
        //        {
        //            foreach (int neighbor in neighbors_nodes[curTop])
        //            {
        //                pathSum = distances_list[curTop] + edge_weight[new KeyValuePair<int, int>(curTop, neighbor)];
        //                if (pathSum < distances_list[neighbor])
        //                    distances_list[neighbor] = pathSum;
        //            }
        //            went.Add(curTop);
        //            index = went.Count - 1;
        //            do
        //            {
        //                if (curTop == INFINITY)
        //                    curTop = went[--index];
        //                list = neighbors_nodes[curTop];
        //                curTop = minDistance = INFINITY;
        //                for (int i = 0; i < list.Count; ++i)
        //                {
        //                    if (!went.Contains(list[i]) && distances_list[list[i]] < minDistance)
        //                    {
        //                        minDistance = distances_list[list[i]];
        //                        curTop = list[i];
        //                    }
        //                }
        //            }
        //            while (curTop == INFINITY && index != 0);
        //            if (index == -1 || curTop == INFINITY)
        //                break;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        private bool ValidateState()
        {
            foreach (EdgeView line in edgeList)
                if (!line.isValid)
                    return false;
            foreach (NodeView top in nodeList)
                if (!top.IsValid)
                    return false;
            return true;
        }
        private void FillDictionaries()
        {
            neighbors_nodes = new Dictionary<string, List<string>>();
            edge_weight = new Dictionary<KeyValuePair<string, string>, int>();
            foreach (NodeView top in nodeList)
                neighbors_nodes[top.NodeName] = new List<string>();
            foreach (EdgeView line in edgeList)
            {
                neighbors_nodes[line.From.NodeName].Add(line.To.NodeName);
                edge_weight[new KeyValuePair<string, string>(line.From.NodeName, line.To.NodeName)] = line.Weight;
                if (!IsOriented)
                {
                    neighbors_nodes[line.To.NodeName].Add(line.From.NodeName);
                    edge_weight[new KeyValuePair<string, string>(line.To.NodeName, line.From.NodeName)] = line.Weight;
                }
            }
        }
    }
}
