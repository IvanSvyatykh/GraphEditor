using GraphEditor.ViewModel.Servises;
using GraphEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GraphEditor.ViewModel.WindowViewModels;

namespace GraphEditor.View
{
    /// <summary>
    /// Логика взаимодействия для DijkstraAlgorithmWindow.xaml
    /// </summary>
    public partial class DijkstraAlgorithmWindow : Window
    {
        private DijkstraAlgorithmWindowViewModel windowViewModel;
        public DijkstraAlgorithmWindow()
        {
            InitializeComponent();
            windowViewModel = new DijkstraAlgorithmWindowViewModel(this);
            DataContext = windowViewModel;
        }
        private void DFSandBFSMenuItemClick(object sender, RoutedEventArgs e)
        {
            INavigationService.SetDFSandBFSWindow(this);
        }
        private void PrimaAlgorithmMenuItemClick(object sender, RoutedEventArgs e)
        {
            INavigationService.SetPrimaAlgorithmWindow(this);
        }
        private void DijkstraAlgorithmMenuItemClick(object sender, RoutedEventArgs e)
        {
            INavigationService.SetDijkstraAlgorithmWindow(this);
        }
    }
}
