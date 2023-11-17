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
    public class graphView : property_base
    {
        private Canvas canvas;

        private List<edge_view> edgeList = new List<edge_view>();
        private List<nodeView> nodeList = new List<nodeView>();

        private bool isOriented = true;

        private bool edgeAdd; 
        private bool edgeDelete;
        private bool nodeDelete;
        
        public nodeView FirstTop;

        //Не знаю нужен ли он мне
        private Dictionary<int, List<int>> neighbors_nodes;

        //Нужно при валидации
        private Dictionary<KeyValuePair<int, int>, int> edge_weight;

        public delegate void PointPositionChanged(nodeView top);


        public graphView(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public List<nodeView> Tops
        {
            get { return nodeList; }
        }

        public bool IsEdgeAdd
        {
            get { return edgeAdd; }
        }
        public bool IsEdgeDelete
        {
            get { return edgeDelete; }
        }

        //Не знаю нужно ли оно мне
        public bool CheckTopNum(nodeView top)
        {
            foreach (nodeView model in nodeList)
                if (top.TopNum == model.TopNum && model != top)
                    return false;
            return true;
        }

        public void AddTop()
        {
            nodeView newNode = new nodeView(this);
 
            OnPointPositionChanged(newNode);
        }
        public Canvas GRCanvas
        {
            get { return canvas; }
            set { canvas = value; }
        }
        public void OnPointPositionChanged(nodeView node)
        {
            node.UpdPos();
            Canvas.SetLeft(node.GRNode, node.RELPos.X);
            Canvas.SetTop(node.GRNode, node.RELPos.Y);
        }
        public void DeleteNode(nodeView top)
        {
            if (nodeList.Contains(top))
            {

                foreach (edge_view model in edgeList)
                    if (model.From == top || model.To == top)
                    {
                        foreach (Shape line in model.Edge)
                            canvas.Children.Remove(line);
                        canvas.Children.Remove(model.TxtBox);
                        edgeList.Remove(model);
                    }
                nodeList.Remove(top);
                canvas.Children.Remove(top.View);
            }
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
        }
        //public void ValidateTopNumbers()
        //{
        //    foreach (nodeView top in nodeList)
        //       top.Validate();
        //}
        public bool IsOriented
        {
            get { return isOriented; }
            set { isOriented = value; OnPropertyChanged("IsOriented"); }
        }
        public bool IsNodeDelete
        {
            get { return nodeDelete; }
        }

        public void AddEdge(nodeView from_node, nodeView to_node)
        {
            edge_view line = new edge_view(this, from_node, to_node);
            edgeList.Add(line);

            FirstTop = null;

            EndAddEdge();
            to_node.UpdCurs();
        }


        // node-edge  add-delete
        public void StartAddEdge()
        {
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
            nodeDelete = true;
            canvas.Cursor = Cursors.ScrollAll;
        }
        public void EndDeleteNode()
        {
            nodeDelete = false;
            canvas.Cursor = Cursors.Arrow;
        }



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
            foreach (edge_view line in edgeList)
                if (!line.isValid)
                    return false;
            foreach (nodeView top in nodeList)
                if (!top.IsValid)
                    return false;
            return true;
        }
        private void FillDictionaries()
        {
            neighbors_nodes = new Dictionary<int, List<int>>();
            edge_weight = new Dictionary<KeyValuePair<int, int>, int>();
            foreach (nodeView top in nodeList)
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
