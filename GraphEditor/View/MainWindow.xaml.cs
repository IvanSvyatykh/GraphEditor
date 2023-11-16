﻿using Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private graph_view _grView;
        bool orient = false;
        public MainWindow()
        {
            InitializeComponent();
            _grView = new graph_view(CanvasForGraph, orient);
            ViewNode a;
        }

        //private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
            
        //    ContextMenu cm = BuildContextMenu();
            
        //}
        private ContextMenu BuildContextMenu()
        {
            var contextMenu = new ContextMenu();

            MenuItem menuItem = new MenuItem();

            menuItem.Header = "Insert node";
            menuItem.Click += InsertNode;
            contextMenu.Items.Add(menuItem);

            contextMenu.Items.Add(new Separator());

            menuItem = new MenuItem();
            menuItem.Header = "Delete selected node";
            menuItem.Click += DeleteSelectedNode;
            contextMenu.Items.Add(menuItem);

            return contextMenu;
        }

        private void DeleteSelectedNode(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InsertNode(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }



        ////protected ArrayList m_NodeTypes = new ArrayList();
        ////protected Microsoft.Msagl.Core.Geometry.Point m_MouseRightButtonDownPoint;
        ////int id = 120;
        ////int downX;
        ////int downY;
        ////PlaneTransformation downTransform = null;
        ////IViewerNode currentNode;
        //public GraphTestWindow()
        //{
        //    InitializeComponent();

        //    //graphViewerPanel.Focusable = true;
        //    //graphViewerPanel.Focus();

        //    //graphViewer.LayoutEditingEnabled = true;
        //    //(graphViewer as IViewer).MouseDown += MouseDown;
        //    //(graphViewer as IViewer).MouseUp += MouseUp;

        //    //Graph graph = new Graph("graph0");
        //    //graph.Directed = true;
        //    //graph.Attr.Margin = 56;
        //    //graph.Attr.MinimalWidth = 1000;
        //    //graph.Attr.BackgroundColor = Microsoft.Msagl.Drawing.Color.WhiteSmoke;
        //    //graphViewer.Graph = graph;
        //}

        //void StartMenuItem_Click(object sender, EventArgs e)
        //{
        //    LoadOnnxFile();
        //}
        ////void MouseDown(object sender, MsaglMouseEventArgs e)
        ////{
        ////    if (e.RightButtonIsPressed && !e.Handled)
        ////    {
        ////        m_MouseRightButtonDownPoint = (graphViewer).ScreenToSource(e);

        ////        ContextMenuStrip cm = BuildContextMenu(m_MouseRightButtonDownPoint);

        ////        cm.Show(graphViewer, new System.Drawing.Point(e.X, e.Y));
        ////    }
        ////}
        ////void MouseUp(object sender, EventArgs e)
        ////{
        ////    if (e.LeftButtonIsPressed)
        ////    {
        ////        var al = new ArrayList();
        ////        foreach (IViewerObject ob in graphViewer.Entities)
        ////        {
        ////            if (ob.MarkedForDragging)
        ////            {
        ////                var edge = ob.DrawingObject as IViewerEdge;
        ////                if (edge == null)
        ////                {
        ////                    var node = ob as IViewerNode;
        ////                    NodeValue.Text = node.Node.Label.Text;
        ////                    currentNode = node;
        ////                    ChangeCurrentNodeLabel.IsEnabled = true;
        ////                }
        ////            }
        ////        }
        ////    }
        ////}
        ////private void ChangeCurrentNodeLabel_Click(object sender, RoutedEventArgs e)
        ////{
        ////    currentNode.Node.Label.Text = NodeValue.Text;
        ////    graphViewer.ResizeNodeToLabel(currentNode.Node);
        ////    graphViewer.Update();

        ////}
        ////protected ContextMenuStrip BuildContextMenu()
        ////{
        ////    var contextMenu = new ContextMenuStrip();

        ////    ToolStripMenuItem menuItem = new ToolStripMenuItem();

        ////    menuItem.Text = "Insert node";
        ////    menuItem.Click += InsertNode;
        ////    contextMenu.Items.Add(menuItem);

        ////    contextMenu.Items.Add(new ToolStripSeparator());

        ////    menuItem = new ToolStripMenuItem();
        ////    menuItem.Text = "Delete selected node";
        ////    menuItem.Click += DeleteSelectedNode;
        ////    contextMenu.Items.Add(menuItem);

        ////    return contextMenu;
        ////}
        ////void InsertNode(object sender, EventArgs e)
        ////{
        ////    DrawingNode node = InsertNode(m_MouseRightButtonDownPoint, (id++).ToString());
        ////}
        ////private DrawingNode InsertNode(Microsoft.Msagl.Core.Geometry.Point center, string id)
        ////{
        ////    var node = new DrawingNode(id);
        ////    node.Label.Text = "Узел"+id;
        ////    node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
        ////    node.Label.FontColor = Microsoft.Msagl.Drawing.Color.Black;
        ////    node.Label.FontSize = 2;
        ////    node.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Ellipse;

        ////    node.Attr.XRadius = 2;
        ////    node.Attr.YRadius = 2;
        ////    //string s = "12";
        ////    //node.UserData = s;
        ////    IViewerNode dNode = graphViewer.CreateIViewerNode(node, center, null);
        ////    graphViewer.AddNode(dNode, true);

        ////    return node;
        ////}
        ////private void InsertWeightNode(object sender, EventArgs e)
        ////{
        ////    DrawingNode node = InsertWeightNode(m_MouseRightButtonDownPoint, (id++).ToString());

        ////}
        ////private DrawingNode InsertWeightNode(Microsoft.Msagl.Core.Geometry.Point center, string id)
        ////{
        ////    var node = new DrawingNode(id);
        ////    node.Label.Text = "34";
        ////    node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
        ////    node.Label.FontColor = Microsoft.Msagl.Drawing.Color.Black;
        ////    node.Label.FontSize = 5;
        ////    node.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;

        ////    //string s = "12";
        ////    //node.UserData = s;

        ////    IViewerNode dNode = graphViewer.CreateIViewerNode(node, center, null);
        ////    graphViewer.AddNode(dNode, true);

        ////    return node;
        ////}

        ////void DeleteSelectedNode(object sender, EventArgs e)
        ////{
        ////    var al = new ArrayList();
        ////    foreach (IViewerObject ob in graphViewer.Entities)
        ////        if (ob.MarkedForDragging)
        ////            al.Add(ob);
        ////    foreach (IViewerObject ob in al)
        ////    {
        ////        var edge = ob.DrawingObject as IViewerEdge;
        ////        if (edge != null)
        ////            graphViewer.RemoveEdge(edge, true);
        ////        else
        ////        {
        ////            var node = ob as IViewerNode;
        ////            if (node != null)
        ////                graphViewer.RemoveNode(node, true);
        ////        }
        ////    }
        ////}
        //void LoadOnnxFile()
        //{
        //    try
        //    {
        //        //Graph graph = new Graph("graph");

        //        ////Всякого говна по мелочи
        //        ////graph.AddEdge("A", "C");
        //        ////graph.AddEdge("A", "C");
        //        ////graph.AddEdge("A", "C");
        //        ////graph.AddEdge("A", "C");
        //        ////graph.AddEdge("A", "C");
        //        ////graph.AddEdge("B", "C");
        //        ////graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
        //        ////graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
        //        ////graph.FindNode("B").Attr.FillColor =Microsoft.Msagl.Drawing.Color.MistyRose;
        //        ////Node c = graph.FindNode("C");
        //        ////c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
        //        ////c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
        //        ////graph.AddEdge("A", "D");
        //        ////graph.AddEdge("A", "E");
        //        ////graph.AddEdge("A", "F");
        //        ////graph.AddEdge("A", "G");
        //        ////graph.AddEdge("G", "B");
        //        ////graph.AddEdge("B", "G");

        //        ////Этим можно воспользоваться для создания весовых графов
        //        ////graph.AddEdge("A", "B").TargetNode.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;

        //        ////ДЕРЕВО

        //        //graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.PaleGreen;

        //        //graph.AddEdge("C", "A");

        //        //graph.AddEdge("A", "B");
        //        //graph.AddEdge("B", "D");
        //        //graph.AddEdge("B", "F");

        //        //graph.AddEdge("C", "E");
        //        //graph.AddEdge("C", "Q");

        //        //graph.Attr.BackgroundColor = Microsoft.Msagl.Drawing.Color.WhiteSmoke;


        //        //graphViewer.Graph = graph;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Windows.MessageBox.Show(e.ToString());
        //    }
        //}
    }
}
