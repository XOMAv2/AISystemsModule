using AISystemsModule.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    /// Логика взаимодействия для ChosenList.xaml
    /// </summary>
    public partial class ChosenList : UserControl
    {
        public static readonly DependencyProperty ChosenProperty = DependencyProperty.Register(
            nameof(Chosen),
            typeof(ObservableCollection<Node>),
            typeof(UserControl),
            new FrameworkPropertyMetadata(new ObservableCollection<Node>()));
        public static readonly DependencyProperty BlackListProperty = DependencyProperty.Register(
            nameof(BlackList),
            typeof(ObservableCollection<Node>),
            typeof(UserControl),
            new FrameworkPropertyMetadata(new ObservableCollection<Node>()));

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void Set(DependencyProperty property, object value, [CallerMemberName] string propertyName = "")
        {
            SetValue(property, value);
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ChosenList()
        {
            InitializeComponent();
            ((FrameworkElement)Content).DataContext = this;
        }

        public ObservableCollection<Node>? Chosen
        {
            get => (ObservableCollection<Node>?)GetValue(ChosenProperty);
            set => Set(ChosenProperty, value);
        }

        public ObservableCollection<Node>? BlackList
        {
            get => (ObservableCollection<Node>?)GetValue(BlackListProperty);
            set => Set(BlackListProperty, value);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string title = (string)button.Tag;
            Chosen?.Remove(Chosen.Where(u => u.Title == title).Single());
        }

        private void BlockButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string title = (string)button.Tag;
            Node node = Chosen?.Where(u => u.Title == title).Single() ?? throw new Exception();
            Chosen.Remove(node);
            BlackList ??= new ObservableCollection<Node>();
            BlackList.Insert(0, node);
        }
    }
}
