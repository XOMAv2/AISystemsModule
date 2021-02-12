using AISystemsModule.Extensions;
using AISystemsModule.Helpers;
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
    /// Interaction logic for ArchitecturalStyleList.xaml
    /// </summary>
    public partial class ArchitecturalStyleList : UserControl
    {
        public static readonly DependencyProperty StylesProperty = DependencyProperty.Register(
            nameof(Styles),
            typeof(string[]),
            typeof(UserControl),
            new FrameworkPropertyMetadata(ArchitecturalStyleExtension.GetAllDescriptions()));
        public static readonly DependencyProperty SelectedStylesProperty = DependencyProperty.Register(
            nameof(SelectedStyles),
            typeof(ObservableCollection<string>),
            typeof(UserControl),
            new FrameworkPropertyMetadata(new ObservableCollection<string>()));

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void Set(DependencyProperty property, object value, [CallerMemberName] string propertyName = "")
        {
            SetValue(property, value);
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ArchitecturalStyleList()
        {
            InitializeComponent();
            ((FrameworkElement)Content).DataContext = this;
        }

        public string[] Styles
        {
            get => (string[])GetValue(StylesProperty);
            set => Set(StylesProperty, value);
        }

        public ObservableCollection<string>? SelectedStyles
        {
            get => (ObservableCollection<string>?)GetValue(SelectedStylesProperty);
            set => Set(SelectedStylesProperty, value);
        }

        private void StylesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRating = (string)((ComboBox)sender).SelectedValue;
            AddButton.IsEnabled = !string.IsNullOrEmpty(selectedRating);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedStyle = (string)StylesComboBox.SelectedValue;
            SelectedStyles ??= new ObservableCollection<string>();

            if (SelectedStyles.Any(s => s == selectedStyle))
            {
                string style = SelectedStyles.Single(s => s == selectedStyle);
                SelectedStyles.Remove(style);
                SelectedStyles.Insert(0, style);
            }
            else
            {
                SelectedStyles.Insert(0, selectedStyle);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string name = (string)button.Tag;
            SelectedStyles?.Remove(SelectedStyles.Where(s => s == name).Single());
        }
    }
}
