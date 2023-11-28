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
using System.Windows.Media.TextFormatting;

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

        private bool isOriented;

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
        private Line leftLineForOrientedEdgeDemo;
        private Line rightLineForOrientedEdgeDemo;
        public GraphView(Canvas canvas, bool isOriented)
        {
            this.canvas = canvas;
            this.isOriented = isOriented;

            lineForEdgeDemonstration = new Line();
            lineForEdgeDemonstration.Stroke = Brushes.Black;
            lineForEdgeDemonstration.StrokeThickness = 2;
            
            leftLineForOrientedEdgeDemo = new Line();
            leftLineForOrientedEdgeDemo.Stroke = Brushes.Black;
            leftLineForOrientedEdgeDemo.StrokeThickness = 2;

            rightLineForOrientedEdgeDemo = new Line();
            rightLineForOrientedEdgeDemo.Stroke = Brushes.Black;
            rightLineForOrientedEdgeDemo.StrokeThickness = 2;


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

            ValidateNamesIsUnique();
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
                ValidateNamesIsUnique();
            }
        }

        public void AddEdge(NodeView startNode, NodeView endNode,int weight)
        {
            FirstTop = null;

            if (canvas.Children.Contains(lineForEdgeDemonstration))
            {
                canvas.Children.Remove(lineForEdgeDemonstration);
            }

            if (canvas.Children.Contains(leftLineForOrientedEdgeDemo) && canvas.Children.Contains(rightLineForOrientedEdgeDemo))
            {
                canvas.Children.Remove(leftLineForOrientedEdgeDemo);
                canvas.Children.Remove(rightLineForOrientedEdgeDemo);
            }
            bool isEdgeTwoWays = false;
            
            foreach (EdgeView edge in edgeList)
            {
                if (isOriented)
                {
                    if (edge.StartNode == startNode && edge.EndNode == endNode)
                    {
                        return;
                    }
                    else if (edge.StartNode == endNode && edge.EndNode == startNode)
                    {
                        isEdgeTwoWays = true;
                        break;
                    }
                }
                else
                {
                    if ((edge.StartNode == startNode && edge.EndNode == endNode) ||
                        (edge.StartNode == endNode && edge.EndNode == startNode))
                    {
                        return;
                    }
                }
            }

            EdgeView line = new EdgeView(this, startNode, endNode, weight, isEdgeTwoWays);
            edgeList.Add(line);       
        }
        private void StopEdgeAdding(object sender, MouseButtonEventArgs e)
        {
            FirstTop = null;

            if (canvas.Children.Contains(lineForEdgeDemonstration))
            {
                canvas.Children.Remove(lineForEdgeDemonstration);
            }

            if (canvas.Children.Contains(leftLineForOrientedEdgeDemo) && canvas.Children.Contains(rightLineForOrientedEdgeDemo))
            {
                canvas.Children.Remove(leftLineForOrientedEdgeDemo);
                canvas.Children.Remove(rightLineForOrientedEdgeDemo);
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

                if (isOriented)
                {
                    double u_l = Math.Atan2(lineForEdgeDemonstration.X1 - lineForEdgeDemonstration.X2, lineForEdgeDemonstration.Y1 - lineForEdgeDemonstration.Y2);
                    double u = Math.PI / 33;

                    leftLineForOrientedEdgeDemo.X1 = lineForEdgeDemonstration.X2 + 10 * Math.Sin(u_l);
                    leftLineForOrientedEdgeDemo.Y1 = lineForEdgeDemonstration.Y2 + 10 * Math.Cos(u_l);

                    leftLineForOrientedEdgeDemo.X2 = lineForEdgeDemonstration.X2 + 30 * Math.Sin(u_l + 2 * u);
                    leftLineForOrientedEdgeDemo.Y2 = lineForEdgeDemonstration.Y2 + 30 * Math.Cos(u_l + 2 * u);

                    rightLineForOrientedEdgeDemo.X1 = lineForEdgeDemonstration.X2 + 10 * Math.Sin(u_l);
                    rightLineForOrientedEdgeDemo.Y1 = lineForEdgeDemonstration.Y2 + 10 * Math.Cos(u_l);
                        
                    rightLineForOrientedEdgeDemo.X2 = lineForEdgeDemonstration.X2 + 30 * Math.Sin(u_l - 2 * u);
                    rightLineForOrientedEdgeDemo.Y2 = lineForEdgeDemonstration.Y2 + 30 * Math.Cos(u_l - 2 * u);
                    
                    if (!canvas.Children.Contains(leftLineForOrientedEdgeDemo) && !canvas.Children.Contains(rightLineForOrientedEdgeDemo))
                    {
                        canvas.Children.Add(leftLineForOrientedEdgeDemo);
                        canvas.Children.Add(rightLineForOrientedEdgeDemo);
                    }
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

            if (canvas.Children.Contains(leftLineForOrientedEdgeDemo) && canvas.Children.Contains(rightLineForOrientedEdgeDemo))
            {
                canvas.Children.Remove(leftLineForOrientedEdgeDemo);
                canvas.Children.Remove(rightLineForOrientedEdgeDemo);
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
        public void ChangeNodesColorFromTo(Brush fromColor, Brush toColor)
        {
            foreach (NodeView node in nodeList)
            {
                if (node.Color == fromColor)
                {
                    node.Color = toColor;
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
        public void ChangeEdgesColorFromTo(Brush fromColor, Brush toColor)
        {
            foreach (EdgeView edge in edgeList)
            {
                if (edge.Color==fromColor)
                {
                    edge.Color = toColor;
                }
            }
        }
        public void DrawTheWay(List<GraphNode> nodes,Brush color)
        {
            for (int i =0; i<nodes.Count-1; i++)
            {
                MessageBox.Show(nodes[i+1].Name);
                foreach(EdgeView edge in edgeList)
                {
                    if (edge.StartNode.NodeName == nodes[i].Name && edge.EndNode.NodeName == nodes[i + 1].Name)
                    {
                        edge.Color = color;
                        edge.StartNode.Color = color;
                        edge.EndNode.Color = color;
                    }
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
        public bool IsEdgeExist()
        {
            return !(edgeList.Count == 0);
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
        public Dictionary<string, List<Tuple<int, string>>> GetEdgeMatrixWithWeightsWhereColorIs(Brush color)
        {
            Dictionary<string, List<Tuple<int, string>>> edgeMatrix = new Dictionary<string, List<Tuple<int, string>>>();

            foreach (NodeView node in nodeList)
            {   
                if (node.Color == color)
                {
                    edgeMatrix.Add(node.NodeName, new List<Tuple<int, string>>());
                }
            }

            foreach (EdgeView line in edgeList)
            {
                if (line.Color == color)
                {
                    edgeMatrix[line.StartNode.NodeName].Add(Tuple.Create(line.Weight, line.EndNode.NodeName));
                }
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
        public Dictionary<string, Point> GetNodeNamesAndCoordinatsWhereColorIs(Brush color)
        {
            Dictionary<string, Point> nodeNamesAndCoordinats = new Dictionary<string, Point>();
            foreach (NodeView node in nodeList)
            {
                if (node.Color == color)
                {
                    nodeNamesAndCoordinats.Add(node.NodeName, node.Position);
                }
            }
            return nodeNamesAndCoordinats;
        }
        public void RenameNodes(Dictionary<string,string> oldAndNewNames)
        {
            for(int i=0; i<nodeList.Count; i++)
            {
                nodeList[i].ViewPartNode.TextBoxForNodeLabel.Text = oldAndNewNames[nodeList[i].NodeName];
            }
        }
        public void BackNodeNamesToBase()
        {
            for (int i = 0; i < nodeList.Count; i++)
            {             
                nodeList[i].ViewPartNode.TextBoxForNodeLabel.Text = nodeList[i].NodeName;
            }
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
