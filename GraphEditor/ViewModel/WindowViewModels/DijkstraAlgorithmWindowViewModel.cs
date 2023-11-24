using Graph;
using GraphEditor.Model.Loggers;
using Model.Graph;
using Model.WorkWithFile;
using Model.WriteToFile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using GraphEditor.View;

namespace GraphEditor.ViewModel.WindowViewModels
{
    internal class DijkstraAlgorithmWindowViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public ICommand SetAddNodesModeCommand { get; }
        public ICommand SetAddEdgesModeCommand { get; }
        public ICommand SetDeletingModeCommand { get; }
        public ICommand LeftButtonClickCommand { get; }
        public ICommand SaveGraphCommand { get; }
        public ICommand LoadGraphCommand { get; }

        public ICommand ChangeTaskCommand { get; }

        public ICommand StartProgrammCommand { get; }
        public ICommand StepForwardCommand { get; }
        public ICommand StepBackwardCommand { get; }

        public ObservableCollection<Button> StepsButtons
        {
            get => stepsButtons;
        }
        public string StartNodeName
        {
            get => startNodeName;
            set => startNodeName = value;
        }
        public string EndNodeName
        {
            get => endNodeName;
            set => endNodeName = value;
        }
        public string Explanation
        {
            get => explanation;
            set
            {
                explanation = value;
                OnPropertyChanged(nameof(Explanation));
            }
        }
        public SolidColorBrush BackgroundForOrientedGraph
        {
            get => backgroundForOrientedGraph;
            set
            {
                backgroundForOrientedGraph = value;
                OnPropertyChanged(nameof(BackgroundForOrientedGraph));
            }
        }
        public SolidColorBrush BackgroundForNoOrientedGraph
        {
            get => backgroundForNoOrientedGraph;
            set
            {
                backgroundForNoOrientedGraph = value;
                OnPropertyChanged(nameof(BackgroundForNoOrientedGraph));
            }
        }
        public bool IsTaskWorking
        {
            get => !isTaskWorking;
            set
            {
                isTaskWorking = value;
                OnPropertyChanged(nameof(IsTaskWorking));
            }
        }
        public bool IsStepForwardEnabled
        {
            get => isStepForwardEnabled;
            set
            {
                isStepForwardEnabled = value;
                OnPropertyChanged(nameof(IsStepForwardEnabled));
            }
        }
        public bool IsStepBackwardEnabled
        {
            get => isStepBackwardEnabled;
            set
            {
                isStepBackwardEnabled = value;
                OnPropertyChanged(nameof(IsStepBackwardEnabled));
            }
        }

        private SolidColorBrush backgroundForOrientedGraph = new SolidColorBrush(Color.FromRgb(255, 199, 199));
        private SolidColorBrush backgroundForNoOrientedGraph = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        private bool isGraphOriented = true;

        private SolidColorBrush offModeButtonBackground = new SolidColorBrush(Color.FromRgb(221, 221, 221));
        private SolidColorBrush onModeButtonBackground = new SolidColorBrush(Color.FromRgb(255, 199, 199));

        private byte currentMode = 0;

        private List<Tuple<GraphNode, GraphEdge, string>> visited;

        private string startNodeName = "A";
        private string endNodeName = "B";

        private string explanation = "Тут пока ничего нет";

        private int stepIndex = 0;
        private ObservableCollection<Button> stepsButtons;

        private bool isStepForwardEnabled = false;
        private bool isStepBackwardEnabled = false;
        private bool isTaskWorking = false;

        private GraphView graphView;
        private DijkstraAlgorithmWindow window;
        public DijkstraAlgorithmWindowViewModel(DijkstraAlgorithmWindow window)
        {
            this.window = window;

            graphView = new GraphView(window.CanvasForGraph, isGraphOriented, true);

            SetAddNodesModeCommand = new RelayCommand(SetAddNodesMode);
            SetAddEdgesModeCommand = new RelayCommand(SetAddEdgesMode);
            SetDeletingModeCommand = new RelayCommand(SetDeletingMode);
            SaveGraphCommand = new RelayCommand(SaveGraph);
            LoadGraphCommand = new RelayCommand(LoadGraph);

            LeftButtonClickCommand = new RelayCommand(LeftButtonClick);

            ChangeTaskCommand = new RelayCommand(ChangeTask);


            StartProgrammCommand = new RelayCommand(StartProgramm);
            StepForwardCommand = new RelayCommand(StepForward);
            StepBackwardCommand = new RelayCommand(StepBackward);
        }

        private void SetAddNodesMode()
        {
            if (currentMode != 1)
            {
                currentMode = 1;
                SetCurrentModeButtonBackgrounds(onModeButtonBackground, offModeButtonBackground, offModeButtonBackground);


                graphView.StartAddingNode();

                graphView.EndDeletingNode();
                graphView.EndAddingEdge();
                graphView.EndDeletingEdge();

            }
            else
            {
                SetZeroMode();
            }
        }
        private void SetAddEdgesMode()
        {
            if (currentMode != 2)
            {
                currentMode = 2;
                SetCurrentModeButtonBackgrounds(offModeButtonBackground, onModeButtonBackground, offModeButtonBackground);

                graphView.StartAddingEdge();

                graphView.EndAddingNode();
                graphView.EndDeletingNode();
                graphView.EndDeletingEdge();
            }
            else
            {
                SetZeroMode();
            }
        }
        private void SetDeletingMode()
        {
            if (currentMode != 3)
            {
                currentMode = 3;
                SetCurrentModeButtonBackgrounds(offModeButtonBackground, offModeButtonBackground, onModeButtonBackground);

                graphView.EndAddingEdge();
                graphView.EndAddingNode();
                graphView.StartDeletingEdge();
                graphView.StartDeletingNode();
            }
            else
            {
                SetZeroMode();
            }
        }
        private void SetZeroMode()
        {
            graphView.EndAddingEdge();
            graphView.EndAddingNode();
            graphView.EndDeletingNode();
            graphView.EndDeletingEdge();

            currentMode = 0;
            SetCurrentModeButtonBackgrounds(offModeButtonBackground, offModeButtonBackground, offModeButtonBackground);
        }
        private void SetCurrentModeButtonBackgrounds(SolidColorBrush color1, SolidColorBrush color2, SolidColorBrush color3)
        {
            window.SetAddNodesModeButton.Background = color1;
            window.SetAddEdgesModeButton.Background = color2;
            window.SetDeletingModeButton.Background = color3;
        }
        private void SaveGraph()
        {
            if (!graphView.ValidateState())
            {
                MessageBox.Show("Вы не можете сохранять граф, содержащий ошибки");
                return;
            }

            var fileDialog = new SaveFileDialog
            {
                Filter = "Text files(*.txt)| *.txt"
            };

            fileDialog.ShowDialog();

            if (!(fileDialog.FileName == ""))
            {
                Writer.WriteGraph(graphView.GetEdgeMatrixWithWeights(), graphView.GetNodeNamesAndCoordinats(), fileDialog.FileName, false);
            }
        }
        private void LoadGraph()
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "Text files(*.txt)| *.txt"
            };
            fileDialog.ShowDialog();

            if (!(fileDialog.FileName == ""))
            {
                try
                {
                    window.CanvasForGraph.Children.Clear();
                    graphView = new GraphView(window.CanvasForGraph, isGraphOriented, true);

                    Tuple<Dictionary<string, List<Tuple<int, string>>>, Dictionary<string, Point>> loadedGraph = Reader.ReadGraph(fileDialog.FileName);

                    graphView.CreateNodes(loadedGraph.Item2);
                    graphView.CreateEdges(loadedGraph.Item1);
                }
                catch (Exception e)
                {
                    MessageBox.Show("К сожалению, при загрузке файла произошла ошибка:\n" + e.Message);
                }
            }
        }

        private void LeftButtonClick()
        {
            if (currentMode == 1)
            {
                graphView.AddNode();
            }
        }

        private void ChangeTask()
        {
            if (isGraphOriented)
            {
                isGraphOriented = false;
                BackgroundForOrientedGraph = offModeButtonBackground;
                BackgroundForNoOrientedGraph = onModeButtonBackground;
            }
            else
            {
                isGraphOriented = true;
                BackgroundForOrientedGraph = onModeButtonBackground;
                BackgroundForNoOrientedGraph = offModeButtonBackground;
            }
        }
        private void StartProgramm()
        {
            if (graphView.ValidateState())
            {
                if (graphView.IsThisNodeExistInGraph(startNodeName) && graphView.IsThisNodeExistInGraph(endNodeName))
                {
                    SetZeroMode();
                    IsTaskWorking = true;

                    ILogger logger;
                    if (isGraphOriented)
                    {
                        graphView.StartTaskWork();
                        BFS bFS = new BFS(graphView.GetEdgeMatrix());
                        logger = bFS.StartAlgorithm(startNodeName);
                        visited = logger.GetVisisted();
                    }
                    else
                    {
                        graphView.StartTaskWork();
                        DFS dFS = new DFS(graphView.GetEdgeMatrix());
                        logger = dFS.StartAlgorithm(startNodeName);
                        visited = logger.GetVisisted();
                    }

                    IsStepForwardEnabled = true;
                    ShowSteps();
                }
                else
                {
                    MessageBox.Show("В графе нет одного из этих узлов");
                }
            }
            else
            {
                MessageBox.Show("Ваш граф содержит ошибки");
            }
        }

        private void ShowSteps()
        {
            stepsButtons = new ObservableCollection<Button>();
            stepIndex = 0;

            for (int i = 0; i < visited.Count; i++)
            {
                Button step = new Button();
                step.Content = "Шаг номер " + (i + 1);

                step.Height = 20;
                step.Width = (900d / 41.7) * 8.5 - 25;
                step.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                step.Click += StepClick;
                stepsButtons.Add(step);
            }

            OnPropertyChanged(nameof(StepsButtons));

            ChooseStep(stepsButtons[stepIndex]);
        }

        private void StepClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)e.Source;
            ChooseStep(b);
        }
        public void ChooseStep(Button step)
        {
            try
            {
                stepsButtons[stepIndex].Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                int newStepIndex = stepsButtons.IndexOf(step);
                stepIndex = newStepIndex;
                stepsButtons[newStepIndex].Background = new SolidColorBrush(Color.FromRgb(140, 200, 255));

                ShowCurrentSituationOfGraph();

                if (stepIndex == 0)
                {
                    IsStepBackwardEnabled = false;
                }
                else
                {
                    IsStepForwardEnabled = true;
                    IsStepBackwardEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StepForward()
        {
            if (stepIndex == stepsButtons.Count - 1)
            {
                stepsButtons = new ObservableCollection<Button>();
                OnPropertyChanged(nameof(StepsButtons));

                visited = new List<Tuple<GraphNode, GraphEdge, string>>();

                graphView.EndTaskWork();
                graphView.ChangeNodesColorToBlue();
                graphView.ChangeEdgesColorToBlack();

                IsStepBackwardEnabled = false;
                IsStepForwardEnabled = false;
                IsTaskWorking = false;
            }
            else
            {
                IsStepBackwardEnabled = true;

                ChooseStep(stepsButtons[stepIndex + 1]);
            }
        }

        private void StepBackward()
        {
            IsStepForwardEnabled = true;

            ChooseStep(stepsButtons[stepIndex - 1]);

            if (stepIndex == 0)
            {
                IsStepBackwardEnabled = false;
            }
        }

        private void ShowCurrentSituationOfGraph()
        {
            Explanation = visited[stepIndex].Item3;
            graphView.ChangeNodesColorToBlue();
            graphView.ChangeEdgesColorToBlack();

            if (isGraphOriented)
            {
                ShowCurrentSituationOfGraphForBFS();
            }
            else
            {
                ShowCurrentSituationOfGraphForDFS();
            }

        }

        private void ShowCurrentSituationOfGraphForDFS()
        {
            string lastChosenNodeName = visited[0].Item1.Name;
            for (int i = 0; i <= stepIndex; i++)
            {
                if (visited[i].Item1 is not null)
                {
                    graphView.ChangeNodeColor(lastChosenNodeName, Brushes.Green);
                    lastChosenNodeName = visited[i].Item1.Name;
                    graphView.ChangeNodeColor(visited[i].Item1.Name, Brushes.YellowGreen);
                }

                if (visited[i].Item2 is not null)
                {
                    graphView.ChangeEdgeColor(visited[i].Item2.FirstNode.Name, visited[i].Item2.SecondNode.Name, Brushes.Green);
                }
            }
        }

        private void ShowCurrentSituationOfGraphForBFS()
        {
            string currentNodeName = "";
            for (int i = 0; i <= stepIndex; i++)
            {
                if (visited[i].Item1 is not null && visited[i].Item1.Name != currentNodeName)
                {
                    graphView.ChangeNodeColor(visited[i].Item1.Name, Brushes.Green);
                }

                if (visited[i].Item2 is not null)
                {
                    graphView.ChangeEdgeColor(visited[i].Item2.FirstNode.Name, visited[i].Item2.SecondNode.Name, Brushes.Green);
                }
                else if (visited[i].Item1 is not null)
                {
                    graphView.ChangeNodeColor(currentNodeName, Brushes.Green);
                    currentNodeName = visited[i].Item1.Name;
                    graphView.ChangeNodeColor(currentNodeName, Brushes.GreenYellow);
                }
            }
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
