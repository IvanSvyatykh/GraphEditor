﻿using GraphEditor.ViewModel;
using GraphEditor.ViewModel.Servises;
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

namespace GraphEditor
{
    /// <summary>
    /// Логика взаимодействия для PrimaAndDeijkstraAlgorithmsWindow.xaml
    /// </summary>
    public partial class PrimaAlgorithmWindow : Window
    {
        private PrimaAlgorithmWindowViewModel windowViewModel;
        public PrimaAlgorithmWindow()
        {
            InitializeComponent();
            windowViewModel = new PrimaAlgorithmWindowViewModel(this);
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
        private void FordFulkersonAlgorithmMenuItemClick(object sender, RoutedEventArgs e)
        {
            INavigationService.SetFordFulkersonAlgorithmWindow(this);
        }
    }
}
