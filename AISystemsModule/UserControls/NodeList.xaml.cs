using AISystemsModule.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AISystemsModule.UserControls
{
    /// <summary>
    /// Interaction logic for NodeList.xaml
    /// </summary>
    public partial class NodeList : UserControl
    {
        public static readonly DependencyProperty NodesProperty = DependencyProperty.Register(
            nameof(Nodes),
            typeof(ObservableCollection<Node>),
            typeof(UserControl),
            new FrameworkPropertyMetadata(new ObservableCollection<Node>()));
        public static readonly DependencyProperty ShowCollabScoreProperty = DependencyProperty.Register(
            nameof(ShowCollabScore),
            typeof(bool),
            typeof(UserControl),
            new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ShowContentScoreProperty = DependencyProperty.Register(
            nameof(ShowContentScore),
            typeof(bool),
            typeof(UserControl),
            new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ShowParametricScoreProperty = DependencyProperty.Register(
            nameof(ShowParametricScore),
            typeof(bool),
            typeof(UserControl),
            new FrameworkPropertyMetadata(false));

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void Set(DependencyProperty property, object value, [CallerMemberName] string propertyName = "")
        {
            SetValue(property, value);
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public NodeList()
        {
            InitializeComponent();
            ((FrameworkElement)Content).DataContext = this;
        }

        public ObservableCollection<Node>? Nodes
        {
            get => (ObservableCollection<Node>?)GetValue(NodesProperty);
            set => Set(NodesProperty, value);
        }

        public bool ShowCollabScore
        {
            get => (bool)GetValue(ShowCollabScoreProperty);
            set => Set(ShowCollabScoreProperty, value);
        }

        public bool ShowContentScore
        {
            get => (bool)GetValue(ShowContentScoreProperty);
            set => Set(ShowContentScoreProperty, value);
        }

        public bool ShowParametricScore
        {
            get => (bool)GetValue(ShowParametricScoreProperty);
            set => Set(ShowParametricScoreProperty, value);
        }
    }
}
