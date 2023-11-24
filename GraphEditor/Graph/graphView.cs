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
using System.Windows.Media;
using System.Xml.Linq;

namespace Graph
{
    public class GraphView : property_base
    {
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
        public bool IsTaskWork
        {
            get { return isTaskWork; }
        }
        public bool IsOriented
        {
            get { return isOriented; }
            set { isOriented = value; OnPropertyChanged("IsOriented"); }
        }

        private Canvas canvas;

        private List<EdgeView> edgeList = new List<EdgeView>();
        private List<NodeView> nodeList = new List<NodeView>();

        private bool isOriented = false;

        private bool isNodeAdding;
        private bool isNodeDeleting;
        private bool isEdgeAdding; 
        private bool isEdgeDeleting;
        private bool isTaskWork;

        private int uniqueNameLetterNumber = 65;
        private int uniqueNameLettersCount = 1;

        public NodeView FirstTop;

        public delegate void PointPositionChanged(NodeView top);

        private Line lineForEdgeDemonstration;

        public GraphView(Canvas canvas)
        {
            this.canvas = canvas;
            
            lineForEdgeDemonstration = new Line();
            lineForEdgeDemonstration.Stroke = Brushes.Black;
            lineForEdgeDemonstration.StrokeThickness = 2;
            
            canvas.MouseMove += DrawingFutureEdge;
            canvas.MouseRightButtonDown += StopEdgeAdding;
        }
        public bool CheckNameIsUnique(NodeView node)
        {   
            if (node.NodeName == "")
            {
                return false;
            }

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
            if (uniqueNameLetterNumber == 91)
            {
                uniqueNameLetterNumber=65;
                uniqueNameLettersCount++;
            }
            string uniqueName = "";
            for (int i=0; i < uniqueNameLettersCount;i++)
            {
                uniqueName += ((char)uniqueNameLetterNumber).ToString();
            }
            
            uniqueNameLetterNumber++;
            
            NodeView newNode = new NodeView(this, uniqueName.ToString());

            nodeList.Add(newNode);
            canvas.Children.Add(newNode.ViewPartNode);
            newNode.pointPositionChange += OnPointPositionChanged;

            OnPointPositionChanged(newNode);
        }
        private void AddNodeFromLoadedFile(string nodeName, Point coordinats)
        {
            NodeView newNode = new NodeView(this, nodeName);
            nodeList.Add(newNode);
            canvas.Children.Add(newNode.ViewPartNode);
            newNode.pointPositionChange += OnPointPositionChanged;

            newNode.Position = coordinats;
            Canvas.SetLeft(newNode.ViewPartNode, coordinats.X);
            Canvas.SetTop(newNode.ViewPartNode, coordinats.Y);
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
                for (int i=0; i<edgeList.Count; i++)
                {
                    EdgeView currentEdge = edgeList[i];
                    if (currentEdge.StartNode == node || currentEdge.EndNode == node)
                    {
                        foreach (Shape line in currentEdge.Edge)
                        {
                            canvas.Children.Remove(line);
                        }

                        canvas.Children.Remove(currentEdge.TxtBox);
                        edgeList.Remove(currentEdge);
                        i--;
                    }
                }

                canvas.Children.Remove(node.ViewPartNode);
                nodeList.Remove(node);
            }
        }

        public void AddEdge(NodeView startNode, NodeView endNode,int weight)
        {
            FirstTop = null;

            if (canvas.Children.Contains(lineForEdgeDemonstration))
            {
                canvas.Children.Remove(lineForEdgeDemonstration);
            }

            foreach (EdgeView edge in edgeList)
            {
                if ((edge.StartNode == startNode && edge.EndNode == endNode)||
                    (edge.StartNode == endNode && edge.EndNode == startNode))
                {
                    return;
                }
            }

            EdgeView line = new EdgeView(this, startNode, endNode, weight);
            edgeList.Add(line);       
        }
        private void StopEdgeAdding(object sender, MouseButtonEventArgs e)
        {
            FirstTop = null;

            if (canvas.Children.Contains(lineForEdgeDemonstration))
            {
                canvas.Children.Remove(lineForEdgeDemonstration);
            }
        }

        private Point GiveMeСorrectMousePosition()
        {
            Point p = Mouse.GetPosition(canvas);

            if (p.X < 0)
            {
                p.X = 0;
            }
            else if (p.X > canvas.ActualWidth)
            {
                p.X = canvas.ActualWidth;
            }

            if (p.Y < 0)
            {
                p.Y = 0;
            }
            else if (p.Y > canvas.ActualHeight)
            {
                p.Y = canvas.ActualHeight;
            }

            return p;
        }
        private void DrawingFutureEdge(object sender, MouseEventArgs e)
        {
            if (FirstTop is not null)
            {
                lineForEdgeDemonstration.X1 = FirstTop.Position.X + FirstTop.ViewPartNode.Width / 2;
                lineForEdgeDemonstration.Y1 = FirstTop.Position.Y + ViewNode.NodeRadius / 2;

                Point point = GiveMeСorrectMousePosition();

                lineForEdgeDemonstration.X2 = point.X;
                lineForEdgeDemonstration.Y2 = point.Y;

                if (!canvas.Children.Contains(lineForEdgeDemonstration))
                {
                    canvas.Children.Add(lineForEdgeDemonstration);
                }
            }
        }
        private void AddEdgeFromLoadedFile(string startNodeName, string endNodeName, int weight)
        {   
            NodeView firstNode = null;
            NodeView endNode = null;
            foreach(NodeView node in nodeList)
            {
                if (node.NodeName == startNodeName)
                {
                    firstNode = node;
                }
                if (node.NodeName == endNodeName)
                {
                    endNode = node;
                }
                if (firstNode is not null && endNode is not null)
                {
                    AddEdge(firstNode, endNode, weight);
                    return;
                }
            }
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
        }
        public void EndAddingEdge()
        {
            isEdgeAdding = false;
            FirstTop = null;
            if (canvas.Children.Contains(lineForEdgeDemonstration))
            {
                canvas.Children.Remove(lineForEdgeDemonstration);
            }
        }
        public void StartDeletingEdge()
        {
            isEdgeDeleting = true;
        }
        public void EndDeletingEdge()
        {
            isEdgeDeleting = false;
        }
        public void StartTaskWork()
        {
            isTaskWork = true;
        }
        public void EndTaskWork()
        {
            isTaskWork = false;
        }
        public void ChangeNodeColor(string nodeName,Brush color)
        {
            foreach (NodeView node in nodeList)
            {
                if (node.NodeName == nodeName)
                {
                    node.Color = color;
                    return;
                }
            }
        }
        public void ChangeEdgeColor(string startNodeName, string endNodeName, Brush color)
        {
            foreach (EdgeView edge in edgeList)
            {
                if ((edge.StartNode.NodeName == startNodeName && edge.EndNode.NodeName == endNodeName) 
                    || (edge.StartNode.NodeName == endNodeName && edge.EndNode.NodeName == startNodeName))
                {
                    edge.Color = color;
                    return;
                }
            }
        }
        public void ChangeNodesColorToBlue()
        {
            foreach (NodeView node in nodeList)
            {
                node.Color = Brushes.Blue;
            }
        }
        public void ChangeEdgesColorToBlack()
        {
            foreach (EdgeView edge in edgeList)
            {
                edge.Color = Brushes.Black;
            }
        }
        public bool ValidateState()
        {
            foreach (EdgeView edge in edgeList)
            {
                if (!edge.isValid)
                {
                    return false;
                }
            }
            foreach (NodeView node in nodeList)
            {
                if (!node.IsValid)
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsThisNodeExistInGraph(string nodeName)
        {
            foreach (NodeView node in nodeList)
            {
                if (node.NodeName == nodeName)
                {
                    return true;
                }
                
            }
            return false;
        }
        public Dictionary<string, List<string>> GetEdgeMatrix()
        {
            Dictionary<string, List<string>> edgeMatrix = new Dictionary<string, List<string>>();

            foreach (NodeView node in nodeList)
            {
                edgeMatrix.Add(node.NodeName, new List<string>());
            }

            foreach (EdgeView line in edgeList)
            {
                edgeMatrix[line.StartNode.NodeName].Add(line.EndNode.NodeName);
            }
            return edgeMatrix;
        }
        
        public Dictionary<string, List<Tuple<int, string>>> GetEdgeMatrixWithWeights()
        {
            Dictionary<string, List<Tuple<int, string>>> edgeMatrix = new Dictionary<string, List<Tuple<int, string>>>();

            foreach (NodeView node in nodeList)
            {
                edgeMatrix.Add(node.NodeName, new List<Tuple<int, string>>());
            }

            foreach (EdgeView line in edgeList)
            {
                edgeMatrix[line.StartNode.NodeName].Add(Tuple.Create(line.Weight,line.EndNode.NodeName));
            }
            return edgeMatrix;
        }
        public Dictionary<string, Point> GetNodeNamesAndCoordinats()
        {
            Dictionary<string, Point> nodeNamesAndCoordinats = new Dictionary<string, Point>();
            foreach (NodeView node in nodeList)
            {
                nodeNamesAndCoordinats.Add(node.NodeName, node.Position);
            }
            return nodeNamesAndCoordinats;
        }

        public void CreateNodes(Dictionary<string, Point> nodeNamesAndCoordinats)
        {
            foreach (string nodeName in nodeNamesAndCoordinats.Keys) 
            {
                if (nodeNamesAndCoordinats[nodeName].X > canvas.ActualWidth || nodeNamesAndCoordinats[nodeName].X < 0 ||
                    nodeNamesAndCoordinats[nodeName].Y > canvas.ActualHeight || nodeNamesAndCoordinats[nodeName].Y < 0)
                {
                    throw new Exception("Узлы в вашем графе выходят за пределы холста");
                }
                AddNodeFromLoadedFile(nodeName, nodeNamesAndCoordinats[nodeName]);
            }
        }
        public void CreateEdges(Dictionary<string, List<Tuple<int, string>>> edgeMatrix)
        {
            foreach(string firstNodeName in edgeMatrix.Keys)
            {
                foreach(Tuple<int,string> weightAndEndNodeName in edgeMatrix[firstNodeName])
                {
                    AddEdgeFromLoadedFile(firstNodeName, weightAndEndNodeName.Item2, weightAndEndNodeName.Item1);
                }
            }
        }
    }
}
