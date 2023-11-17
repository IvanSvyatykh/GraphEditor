using Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GraphEditor.ViewModel
{
    public class GraphWindowViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public ICommand InsertNodeCommand { get; }
        public ICommand RemoveNodeCommand { get; }

        private graphView graphView;

        private MainWindow window;
        public GraphWindowViewModel(MainWindow window)
        {
            this.window = window;

            graphView = new graphView(window.CanvasForGraph);

            InsertNodeCommand = new RelayCommand(InsertNode);
            RemoveNodeCommand = new RelayCommand(DeleteSelectedNode);
        }
        private void InsertNode()
        {
           
            graphView.AddTop();
        }
        private void DeleteSelectedNode()
        {
            
            graphView.StartDeleteNode();
        }

        public bool HasErrors => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
