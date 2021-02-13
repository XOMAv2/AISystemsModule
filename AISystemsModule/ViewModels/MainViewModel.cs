using AISystemsModule.Helpers;
using AISystemsModule.Measures;
using AISystemsModule.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AISystemsModule.ViewModels
{
    partial class MainViewModel : Observable
    {
        private Tree moscowTree;
        private ObservableCollection<TreeViewItem> moscowTreeView;
        private string sourcePath = @"D:\Users\Nikita\source\repos\AISystemsModule\AISystemsModule\Resources\moscow.attributes.json";
        private string node1Title = "";
        private string node2Title = "";
        private double euclideanDistance = 0;
        private double manhattanDistance = 0;
        private double treeDistance = 0;
        private double correlationDistance = 0;
        private Visibility resultVisibility = Visibility.Collapsed;
        private ObservableCollection<User> users;
        private User? currentUser;
        private ObservableCollection<Node>? collabRecommendations;
        private ObservableCollection<Node>? contentRecommendations;
        private string[] measureTypes;
        private MeasureType selectedMeasureType;
        private string avgPriceFromString = "";
        private string avgPriceToString = "";
        private string maxMetroDistanceFromString = $"{0}";
        private string maxMetroDistanceToString = $"{1000}";
        private string schoolsNumberFromString = "";
        private string schoolsNumberToString = "";
        private string bestUniversityRatingFromString = "";
        private string bestUniversityRatingToString = "";
        private Dictionary<string, bool?> heritageOptionsDictionary = new Dictionary<string, bool?>()
        {
            { "Есть", true },
            { "Отсутствуют", false },
            { "Не важно", null }
        };
        private string selectedHeritageOptionString = "Не важно";
        private ObservableCollection<string>? selectedArchStyles;
        private ObservableCollection<Node>? searchResults;
        private bool collabInUse = true;
        private bool contentInUse = true;
        private bool parametricInUse = true;
        private ObservableCollection<Node>? hybridResult;

        public MainViewModel()
        {
            ConfigTreeAndTreeView();
            LoadTreeFromFile();
            GenerateUsers();
            measureTypes = new[]
            {
                "Евклидово расстояние",
                "Манхэттенское расстояние",
                "Расстояние между узлами дерева",
                "Линейный коэффициент корреляции (коэффициент корреляции Пирсона)"
            };
            selectedMeasureType = MeasureType.TreeDistance;
        }

        public Tree MoscowTree
        {
            get => moscowTree;
            set => Set(ref moscowTree, value);
        }

        public ObservableCollection<TreeViewItem> MoscowTreeView
        {
            get => moscowTreeView;
            set => Set(ref moscowTreeView, value);
        }

        public string SourcePath
        {
            get => sourcePath;
            set => Set(ref sourcePath, value);
        }

        public string Node1Title
        {
            get => node1Title;
            set => Set(ref node1Title, value);
        }

        public string Node2Title
        {
            get => node2Title;
            set => Set(ref node2Title, value);
        }

        public double EuclideanDistance
        {
            get => euclideanDistance;
            set => Set(ref euclideanDistance, value);
        }

        public double ManhattanDistance
        {
            get => manhattanDistance;
            set => Set(ref manhattanDistance, value);
        }

        public double TreeDistance
        {
            get => treeDistance;
            set => Set(ref treeDistance, value);
        }

        public double CorrelationDistance
        {
            get => correlationDistance;
            set => Set(ref correlationDistance, value);
        }

        public Visibility ResultVisibility
        {
            get => resultVisibility;
            set => Set(ref resultVisibility, value);
        }

        public ObservableCollection<User> Users
        {
            get => users;
            set => Set(ref users, value);
        }

        public User? CurrentUser
        {
            get => currentUser;
            set => Set(ref currentUser, value);
        }

        public ObservableCollection<Node>? CollabRecommendations
        {
            get => collabRecommendations;
            set => Set(ref collabRecommendations, value);
        }

        public ObservableCollection<Node>? ContentRecommendations
        {
            get => contentRecommendations;
            set => Set(ref contentRecommendations, value);
        }

        public string[] MeasureTypes
        {
            get => measureTypes;
            set => Set(ref measureTypes, value);
        }

        public int SelectedMeasureType
        {
            get => (int)selectedMeasureType;
            set
            {
                MeasureType mType = (MeasureType)value;
                Set(ref selectedMeasureType, mType);
            }
        }

        public string AvgPriceFromString
        {
            get => avgPriceFromString;
            set
            {
                if (value == "" || (double.TryParse(value, out double number) && number >= 0))
                {
                    Set(ref avgPriceFromString, value);
                }
            }
        }

        public double? AvgPriceFrom
        {
            get => avgPriceFromString == ""
                ? (double?)null
                : double.Parse(avgPriceFromString);
            set => AvgPriceFromString = value.ToString();
        }

        public string AvgPriceToString
        {
            get => avgPriceToString;
            set
            {
                if (value == "" || (double.TryParse(value, out double number) && number >= 0))
                {
                    Set(ref avgPriceToString, value);
                }
            }
        }

        public double? AvgPriceTo
        {
            get => avgPriceToString == ""
                ? (double?)null
                : double.Parse(avgPriceToString);
            set => AvgPriceToString = value.ToString();
        }

        public string MaxMetroDistanceFromString
        {
            get => maxMetroDistanceFromString;
            set
            {
                if (value == "" || (double.TryParse(value, out double number) && number >= 0))
                {
                    Set(ref maxMetroDistanceFromString, value);
                }
            }
        }

        public double? MaxMetroDistanceFrom
        {
            get => maxMetroDistanceFromString == ""
                ? (double?)null
                : double.Parse(maxMetroDistanceFromString);
            set => MaxMetroDistanceFromString = value.ToString();
        }

        public string MaxMetroDistanceToString
        {
            get => maxMetroDistanceToString;
            set
            {
                if (value == "" || (double.TryParse(value, out double number) && number >= 0))
                {
                    Set(ref maxMetroDistanceToString, value);
                }
            }
        }

        public double? MaxMetroDistanceTo
        {
            get => maxMetroDistanceToString == ""
                ? (double?)null
                : double.Parse(maxMetroDistanceToString);
            set => MaxMetroDistanceToString = value.ToString();
        }

        public string SchoolsNumberFromString
        {
            get => schoolsNumberFromString;
            set
            {
                if (value == "" || (double.TryParse(value, out double number) && number >= 0))
                {
                    Set(ref schoolsNumberFromString, value);
                }
            }
        }

        public double? SchoolsNumberFrom
        {
            get => schoolsNumberFromString == ""
                ? (double?)null
                : double.Parse(schoolsNumberFromString);
            set => SchoolsNumberFromString = value.ToString();
        }

        public string SchoolsNumberToString
        {
            get => schoolsNumberToString;
            set
            {
                if (value == "" || (double.TryParse(value, out double number) && number >= 0))
                {
                    Set(ref schoolsNumberToString, value);
                }
            }
        }

        public double? SchoolsNumberTo
        {
            get => schoolsNumberToString == ""
                ? (double?)null
                : double.Parse(schoolsNumberToString);
            set => SchoolsNumberToString = value.ToString();
        }

        public string BestUniversityRatingFromString
        {
            get => bestUniversityRatingFromString;
            set
            {
                if (value == "" || (double.TryParse(value, out double number) && number >= 0))
                {
                    Set(ref bestUniversityRatingFromString, value);
                }
            }
        }

        public double? BestUniversityRatingFrom
        {
            get => bestUniversityRatingFromString == ""
                ? (double?)null
                : double.Parse(bestUniversityRatingFromString);
            set => BestUniversityRatingFromString = value.ToString();
        }

        public string BestUniversityRatingToString
        {
            get => bestUniversityRatingToString;
            set
            {
                if (value == "" || (double.TryParse(value, out double number) && number >= 0))
                {
                    Set(ref bestUniversityRatingToString, value);
                }
            }
        }

        public double? BestUniversityRatingTo
        {
            get => bestUniversityRatingToString == ""
                ? (double?)null
                : double.Parse(bestUniversityRatingToString);
            set => BestUniversityRatingToString = value.ToString();
        }

        public string[] HeritageOptions
        {
            get => heritageOptionsDictionary.Keys.ToArray();
        }

        public string SelectedHeritageOptionString
        {
            get => selectedHeritageOptionString;
            set => Set(ref selectedHeritageOptionString, value);
        }

        public bool? SelectedHeritageOption
        {
            get => heritageOptionsDictionary[selectedHeritageOptionString];
        }

        public ObservableCollection<string>? SelectedArchStyles
        {
            get => selectedArchStyles;
            set => Set(ref selectedArchStyles, value);
        }

        public ObservableCollection<Node>? SearchResults
        {
            get => searchResults;
            set => Set(ref searchResults, value);
        }

        public bool CollabInUse
        {
            get => collabInUse;
            set => Set(ref collabInUse, value);
        }

        public bool ContentInUse
        {
            get => contentInUse;
            set => Set(ref contentInUse, value);
        }

        public bool ParametricInUse
        {
            get => parametricInUse;
            set => Set(ref parametricInUse, value);
        }

        public ObservableCollection<Node>? HybridResult
        {
            get => hybridResult;
            set => Set(ref hybridResult, value);
        }
    }
}
