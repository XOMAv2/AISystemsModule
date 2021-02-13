using AISystemsModule.Helpers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AISystemsModule.Models
{
    public class Node : Observable
    {
        private string title;
        private Node? parent;
        private List<Node> children;
        private NodeAttributes? attributes;
        private double distance = 0;
        private int beans = 0;
        private int rate = 0;

        public Node() => (Title, Children) = ("node", new List<Node>());

        public Node(string title, Node? parent = null, List<Node>? children = null, NodeAttributes? attributes = null)
        {
            Title = title;
            Parent = parent;
            Children = children ?? new List<Node>();
            Attributes = attributes;
        }

        /// <summary> Имя ноды. </summary>
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        /// <summary> Родитель ноды. </summary>
        [JsonIgnore]
        public Node? Parent
        {
            get => parent;
            set => Set(ref parent, value);
        }

        /// <summary> Потомки ноды. </summary>
        public List<Node> Children
        {
            get => children;
            set => Set(ref children, value);
        }

        public NodeAttributes? Attributes
        {
            get => attributes;
            set => Set(ref attributes, value);
        }

        [JsonIgnore]
        public bool IsLeaf => Children.Count == 0;

        [JsonIgnore]
        public double Distance
        {
            get => distance;
            set => Set(ref distance, value);
        }

        /// <summary>
        /// Бобы, начисляющиеся за попадание в диапазон
        /// при параметрическом поиске.
        /// </summary>
        [JsonIgnore]
        public int Beans
        {
            get => beans;
            set => Set(ref beans, value);
        }

        /// <summary>
        /// Для отображения в UI числа совпавших нод между текущим пользователем
        /// и каждым другим пользователем.
        /// </summary>
        [JsonIgnore]
        public int Rate
        {
            get => rate;
            set => Set(ref rate, value);
        }
    }
}
