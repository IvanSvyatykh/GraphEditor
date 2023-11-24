using Graph;
using GraphEditor.ViewModel;
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
    public partial class DFSandBFSWindow : Window
    {   
        private DFSandBFSWindowViewModel windowViewModel;
        public DFSandBFSWindow()
        {
            InitializeComponent();
            windowViewModel = new DFSandBFSWindowViewModel(this);
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
    }
}
