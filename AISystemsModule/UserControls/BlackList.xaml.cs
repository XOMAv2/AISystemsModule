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
    /// Логика взаимодействия для BlackList.xaml
    /// </summary>
    public partial class BlackList : UserControl
    {
        public static readonly DependencyProperty BlackListCollProperty = DependencyProperty.Register(
            nameof(BlackListColl),
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

        public BlackList()
        {
            InitializeComponent();
            ((FrameworkElement)Content).DataContext = this;
        }

        public ObservableCollection<Node>? BlackListColl
        {
            get => (ObservableCollection<Node>?)GetValue(BlackListCollProperty);
            set => Set(BlackListCollProperty, value);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string title = (string)button.Tag;
            BlackListColl?.Remove(BlackListColl.Where(u => u.Title == title).Single());
        }
    }
}
