using AISystemsModule.Extensions;
using AISystemsModule.Helpers;
using AISystemsModule.Measures;
using AISystemsModule.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AISystemsModule.ViewModels
{
    partial class MainViewModel : Observable
    {
        private ICommand openFileDialogCommand;
        private ICommand loadTreeCommand;
        private ICommand calcDistanceCommand;
        private ICommand addAsNode1Command;
        private ICommand addAsNode2Command;
        private ICommand addToChosen;
        private ICommand addToBlackList;
        private ICommand collabRefreshCommand;
        private ICommand contentRefreshCommand;
        private ICommand searchCommand;

        public ICommand OpenFileDialogCommand => openFileDialogCommand ??= new RelayCommand<object>(_ =>
        {
            OpenFileDialog fileDialog = new OpenFileDialog { Filter = "JSON|*.json|Все файлы|*.*" };

            if (fileDialog.ShowDialog() ?? throw new Exception())
            {
                SourcePath = fileDialog.FileName;
            }
        });

        public ICommand LoadTreeCommand => loadTreeCommand ??= new RelayCommand<object>(_ =>
        {
            LoadTreeFromFile();
        });

        public ICommand CalcDistanceCommand => calcDistanceCommand ??= new RelayCommand<object>(obj =>
        {
            try
            {
                if (Node1Title != "" && Node2Title != "")
                {
                    Node? node1 = MoscowTree.GetNodeByTitle(Node1Title);
                    Node? node2 = MoscowTree.GetNodeByTitle(Node2Title);

                    if (node1 != null && node2 != null)
                    {
                        DistanceCalculator calculator = new DistanceCalculator(node1, node2);

                        if (!node1.IsLeaf || !node2.IsLeaf)
                        {
                            MessageBox.Show("Значение каждого атрбибута нелистовой ноды будет приравнено к нулю.");
                        }

                        EuclideanDistance = Math.Round(calculator.CalculateDistance(MeasureType.EuclideanDistance), 3);
                        ManhattanDistance = Math.Round(calculator.CalculateDistance(MeasureType.ManhattanDistance), 3);
                        TreeDistance = Math.Round(calculator.CalculateDistance(MeasureType.TreeDistance), 3);
                        CorrelationDistance = Math.Round(calculator.CalculateDistance(MeasureType.Correlation), 3);

                        ResultVisibility = Visibility.Visible;
                    }
                    else if (node1 == null)
                    {
                        MessageBox.Show($"Нода с заголовком {node1Title} не найдена.");
                    }
                    else
                    {
                        MessageBox.Show($"Нода с заголовком {Node2Title} не найдена.");
                    }
                }
                else
                {
                    MessageBox.Show($"Не указан заголовок ноды.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("В процессе выполнения программы произошла ошибка.\n" + ex.Message);
            }
        });

        public ICommand AddAsNode1Command => addAsNode1Command ??= new RelayCommand<TreeViewItem>(item =>
        {
            Node1Title = (string)item.Header;
        });

        public ICommand AddAsNode2Command => addAsNode2Command ??= new RelayCommand<TreeViewItem>(item =>
        {
            Node2Title = (string)item.Header;
        });

        public ICommand AddToChosen => addToChosen ??= new RelayCommand<TreeViewItem>(item =>
        {
            Node? n = MoscowTree.GetNodeByTitle((string)item.Header);

            if (n != null && CurrentUser != null && !CurrentUser.Chosen.Contains(n))
            {
                CurrentUser.Chosen.Add(n);
                CurrentUser.BlackList.Remove(n);
            }
        });

        public ICommand AddToBlackList => addToBlackList ??= new RelayCommand<TreeViewItem>(item =>
        {
            Node? n = MoscowTree.GetNodeByTitle((string)item.Header);

            if (n != null && CurrentUser != null && !CurrentUser.BlackList.Contains(n))
            {
                CurrentUser.BlackList.Add(n);
                CurrentUser.Chosen.Remove(n);
            }
        });

        public ICommand CollabRefreshCommand => collabRefreshCommand ??= new RelayCommand<object>(_ =>
        {
            if (CurrentUser != null)
            {
                List<Node> result = Recommendations.GenerateCollabRecommendations(Users, CurrentUser);
                CollabRecommendations = new ObservableCollection<Node>(result);
            }
            else
            {
                MessageBox.Show("Не удалось сгенерировать рекомендации, так как не выбран целевой пользователь.");
            }
        });

        public ICommand ContentRefreshCommand => contentRefreshCommand ??= new RelayCommand<object>(_ =>
        {
            if (CurrentUser != null)
            {
                if (CurrentUser.Chosen.Count != 0)
                {
                    List<Node> result = Recommendations.GenerateContentRecommendations(MoscowTree, CurrentUser, selectedMeasureType);
                    ContentRecommendations = new ObservableCollection<Node>(result);
                }
                else
                {
                    MessageBox.Show("Не удалось сгенерировать рекомендации, так как пользователь не выбрал ни одной ноды.");
                }
            }
            else
            {
                MessageBox.Show("Не удалось сгенерировать рекомендации, так как не выбран целевой пользователь.");
            }
        });

        public ICommand SearchCommand => searchCommand ??= new RelayCommand<object>(_ =>
        {
            List<Node> result = Searches.ParametricSearch(MoscowTree, new SearchRanges(
                avgPricePerSquareMeter: new Models.Range(AvgPriceFrom, AvgPriceTo),
                maxDistanceToTheMetroStation: new Models.Range(MaxMetroDistanceFrom, MaxMetroDistanceTo),
                schoolsNumberPerCapita: new Models.Range(SchoolsNumberFrom, SchoolsNumberTo),
                bestRatingOfTheLocalUniversity: new Models.Range(BestUniversityRatingFrom, BestUniversityRatingTo),
                areThereAnyHeritageSites: SelectedHeritageOption,
                specificArchitecturalStyles: SelectedArchStyles
                    ?.Select(s => ArchitecturalStyleExtension.GetValueByDescription(s))
                    .Where(s => s != null)
                    .Select(s => (ArchitecturalStyle)s)
                    .ToHashSet()
            ));
            SearchResults = new ObservableCollection<Node>(result);
        });
    }
}
