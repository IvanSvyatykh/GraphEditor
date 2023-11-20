﻿using Graph;
using GraphEditor.Model.Graph;
using Model.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphEditor.ViewModel
{
    public class GraphWindowViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {   
        public ICommand SetAddNodesModeCommand { get; }
        public ICommand SetAddEdgesModeCommand { get; }
        public ICommand SetDeletingModeCommand { get; }
        public ICommand LeftButtonClickCommand { get; }

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
        public string Explanation
        {
            get => explanation;
            set
            {
                explanation = value;
                OnPropertyChanged(nameof(Explanation));
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


        private SolidColorBrush offModeButtonBackground = new SolidColorBrush(Color.FromRgb(221, 221, 221));
        private SolidColorBrush onModeButtonBackground = new SolidColorBrush(Color.FromRgb(255, 199, 199));

        private byte currentMode = 0;

        private BFSLogger loggerForBFS;
        private string startNodeName = "A";
        private string explanation = "Тут пока ничего нет";

        private int stepIndex = 0;
        private ObservableCollection<Button> stepsButtons;
        private bool isStepForwardEnabled = false;
        private bool isStepBackwardEnabled = false;

        private GraphView graphView;
        private MainWindow window;
        public GraphWindowViewModel(MainWindow window)
        {
            this.window = window;

            graphView = new GraphView(window.CanvasForGraph);

            SetAddNodesModeCommand = new RelayCommand(SetAddNodesMode);
            SetAddEdgesModeCommand = new RelayCommand(SetAddEdgesMode);
            SetDeletingModeCommand = new RelayCommand(SetDeletingMode);

            LeftButtonClickCommand = new RelayCommand(LeftButtonClick);

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
                graphView.EndAddingEdge();
                graphView.EndAddingNode();
                graphView.EndDeletingNode();
                graphView.EndDeletingEdge();

                currentMode = 0;
                SetCurrentModeButtonBackgrounds(offModeButtonBackground, offModeButtonBackground, offModeButtonBackground);
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
                graphView.EndAddingEdge();
                graphView.EndAddingNode();
                graphView.EndDeletingNode();
                graphView.EndDeletingEdge();

                currentMode = 0;
                SetCurrentModeButtonBackgrounds(offModeButtonBackground, offModeButtonBackground, offModeButtonBackground);
            }
        }
        private void SetDeletingMode()
        {   if (currentMode != 3)
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
                graphView.EndAddingEdge();
                graphView.EndAddingNode();
                graphView.EndDeletingNode();
                graphView.EndDeletingEdge();

                currentMode = 0;
                SetCurrentModeButtonBackgrounds(offModeButtonBackground, offModeButtonBackground, offModeButtonBackground);
            }
        }

        private void SetCurrentModeButtonBackgrounds(SolidColorBrush color1, SolidColorBrush color2, SolidColorBrush color3)
        {
            window.SetAddNodesModeButton.Background = color1;
            window.SetAddEdgesModeButton.Background = color2;
            window.SetDeletingModeButton.Background = color3;
        }
        private void LeftButtonClick()
        {
            if (currentMode == 1)
            {
                graphView.AddNode();
            }
        }

        private void StartProgramm()
        {
            if (graphView.ValidateState())
            {   
                if (graphView.IsThisNodeExistInGraph(startNodeName))
                {
                    BFS bFS = new BFS(graphView.GetEdgeMatrix());
                    loggerForBFS = bFS.StartAlgorithm(startNodeName);

                    IsStepForwardEnabled = true;
                    ShowSteps();
                }
                else
                {
                    MessageBox.Show("В графе нет такого узла");
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
            if (true)
            {
                for (int i = 0; i < loggerForBFS.Visited.Count; i++)
                {
                    Button step = new Button();
                    step.Content = "Шаг номер " + (i + 1);

                    step.Height = 20;
                    step.Width = (900d / 41.7) * 8.5 - 25;
                    step.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                    step.Click += StepClick;
                    stepsButtons.Add(step);
                }
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

                graphView.ChangeNodesColorToBlue();
                IsStepBackwardEnabled= false;
                IsStepForwardEnabled= false;
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
            Explanation = loggerForBFS.Visited[stepIndex].Item3;
            graphView.ChangeNodesColorToBlue();
            for(int i=0; i<=stepIndex; i++)
            {
                graphView.ChangeNodeColor(loggerForBFS.Visited[i].Item1.Name,Brushes.Green);

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
