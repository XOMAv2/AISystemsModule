using AISystemsModule.Helpers;
using System.Collections.ObjectModel;

namespace AISystemsModule.Models
{
    public class User : Observable
    {
        private string name;
        private bool isCurrnet = false;
        private ObservableCollection<Node> chosen;
        private ObservableCollection<Node> blackList = new ObservableCollection<Node>();
        private int rate = 0;

        public User(string username)
        {
            name = username;
            chosen = new ObservableCollection<Node>();
        }

        public User(string username, ObservableCollection<Node> chosenItems)
        {
            name = username;
            chosen = chosenItems;
        }

        /// <summary> Имя пользователя. </summary>
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        /// <summary>
        /// Это свойство нужно исключительно для корректного отображения
        /// пользователя в списке в UserList.xaml.
        /// </summary>
        public bool IsCurrent
        {
            get => isCurrnet;
            set => Set(ref isCurrnet, value);
        }

        /// <summary> Ноды, выбранные пользователем. </summary>
        public ObservableCollection<Node> Chosen
        {
            get => chosen;
            set => Set(ref chosen, value);
        }

        /// <summary> Ноды, которые пользователь не желает видеть. </summary>
        public ObservableCollection<Node> BlackList
        {
            get => blackList;
            set => Set(ref blackList, value);
        }

        /// <summary> Рейтинг совпадения. </summary>
        public int Rate
        {
            get => rate;
            set => Set(ref rate, value);
        }
    }
}