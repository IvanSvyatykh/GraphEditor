using GraphEditor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphEditor.ViewModel.Servises
{
    internal interface INavigationService
    {
        static void SetDFSandBFSWindow(Window window)
        {
            window.Hide();
            DFSandBFSWindow newWindow = new DFSandBFSWindow();
            newWindow.Show();
            window.Close();
        }
        static void SetPrimaAlgorithmWindow(Window window)
        {
            window.Hide();
            PrimaAlgorithmWindow newWindow = new PrimaAlgorithmWindow();
            newWindow.Show();
            window.Close();
        }
        static void SetDijkstraAlgorithmWindow(Window window)
        {
            window.Hide();
            DijkstraAlgorithmWindow newWindow = new DijkstraAlgorithmWindow();
            newWindow.Show();
            window.Close();
        }
    }
}
