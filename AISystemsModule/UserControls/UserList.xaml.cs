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
    /// Логика взаимодействия для UserList.xaml
    /// </summary>
    public partial class UserList : UserControl
    {
        public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
            nameof(UserName),
            typeof(string),
            typeof(UserControl),
            new FrameworkPropertyMetadata("Новый пользователь"),
            new ValidateValueCallback(UserNameValidate));
        public static readonly DependencyProperty AddedUsersProperty = DependencyProperty.Register(
            nameof(AddedUsers),
            typeof(ObservableCollection<User>),
            typeof(UserControl),
            new FrameworkPropertyMetadata(new ObservableCollection<User>(), AddedUsersPropertyChanged));
        public static readonly DependencyProperty CurrentUserProperty = DependencyProperty.Register(
            nameof(CurrentUser),
            typeof(User),
            typeof(UserControl),
            new FrameworkPropertyMetadata(null, CurrentUserPropertyChanged));

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void Set(DependencyProperty property, object value, [CallerMemberName] string propertyName = "")
        {
            SetValue(property, value);
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public UserList()
        {
            InitializeComponent();
            ((FrameworkElement)Content).DataContext = this;
        }

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => Set(UserNameProperty, value);
        }

        private static bool UserNameValidate(object value) => (string)value != null;

        // Не срабатывает при инициализации свойства.
        //private static void UserNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        //{
        //    UserList thisControl = (UserList)dependencyObject;
        //    Button addButton = (Button)thisControl.FindName("AddButton");
        //    ObservableCollection<User> addedUsers = (ObservableCollection<User>)thisControl.GetValue(AddedUsersProperty);
        //    string username = (string)ea.NewValue;
        //    addButton.IsEnabled = !string.IsNullOrEmpty(username) && !addedUsers.Where(u => u.Name == username).Any();
        //}

        public ObservableCollection<User> AddedUsers
        {
            get => (ObservableCollection<User>)GetValue(AddedUsersProperty);
            set => Set(AddedUsersProperty, value);
        }

        private static void AddedUsersPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UserList thisControl = (UserList)dependencyObject;
            ListBox usersListBox = (ListBox)thisControl.FindName("UsersListBox");
            usersListBox.Visibility = ((ObservableCollection<User>)ea.NewValue).Count > 0
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public User? CurrentUser
        {
            get => (User?)GetValue(CurrentUserProperty);
            set => Set(CurrentUserProperty, value);
        }
        
        private static void CurrentUserPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UserList thisControl = (UserList)dependencyObject;
            ObservableCollection<User> addedUsers = (ObservableCollection<User>)thisControl.GetValue(AddedUsersProperty);
            User user = (User)ea.NewValue;

            if (user != null)
            {
                if (!addedUsers.Contains(user))
                {
                    thisControl.SetValue(CurrentUserProperty, null);
                }
                else if (user.IsCurrent == false)
                {
                    user.IsCurrent = true;
                    thisControl.SetValue(CurrentUserProperty, user);
                }   
            }
        }

        private void UsersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string username = ((TextBox)sender).Text;
            AddButton.IsEnabled = !string.IsNullOrEmpty(username) && !AddedUsers.Where(u => u.Name == username).Any();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;

            if (!AddedUsers.Where(u => u.Name == username).Any())
            {
                AddedUsers.Insert(0, new User(username));
                AddButton.IsEnabled = false;
                UsersListBox.Visibility = Visibility.Visible;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string username = (string)button.Tag;
            User user = AddedUsers.Where(u => u.Name == username).Single();
            AddedUsers.Remove(user);

            if (CurrentUser == user)
            {
                CurrentUser = null;
            }            

            if (AddedUsers.Count == 0)
            {
                UsersListBox.Visibility = Visibility.Collapsed;
            }

            if (username == UserNameTextBox.Text)
            {
                AddButton.IsEnabled = true;
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string username = (string)button.Tag;

            if (CurrentUser != null)
            {
                CurrentUser.IsCurrent = false;
            }

            CurrentUser = AddedUsers.Where(u => u.Name == username).SingleOrDefault();

            if (CurrentUser != null)
            {
                CurrentUser.IsCurrent = true;
            }
        }
    }
}
