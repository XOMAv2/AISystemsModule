using AISystemsModule.Extensions;
using AISystemsModule.Helpers;
using System.Linq;
using System.Windows.Controls;
using System.Text;
using System.Windows.Input;
using System;
using System.Collections.Generic;

namespace AISystemsModule.Models
{
    public class Tree : Observable
    {
        private Node root;

        /// <summary> Корневой узел дерева. </summary>
        public Node Root
        {
            get => root;
            set => Set(ref root, value);
        }

        public Tree(Node? root = null) => Root = root ?? new Node(nameof(Root));

        /// <summary> Установка ссылки на родителя для каждой ноды дерева. </summary>
        public void SetParents() => SetParents(Root, null);

        /// <summary> Установка ссылки на родителя для каждой ноды дерева. </summary>
        /// <param name="node"> Текущая нода дерева. </param>
        /// <param name="parent"> Родитель текущей ноды. </param>
        private void SetParents(Node node, Node? parent)
        {
            node.Parent = parent;

            foreach (Node child in node.Children)
            {
                SetParents(child, node);
            }
        }

        /// <summary> Получить ноду дерева по его заголовку (интерпретируется как идентификатор). </summary>
        /// <param name="title"> Заголовок ноды. </param>
        /// <param name="node"> Нода дерева, с которой начинается поиск (по умолчанию Root). </param>
        public Node? GetNodeByTitle(string title, Node? node = null)
        {
            node ??= Root;

            if (node.Title == title)
            {
                return node;
            }

            foreach (Node child in node.Children)
            {
                node = GetNodeByTitle(title, child);

                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        public List<Node> GetLeafs()
        {
            List<Node> leafs = new List<Node>();
            var stack = new Stack<Node>();
            stack.Push(root);

            while (stack.Any())
            {
                Node node = stack.Pop();

                if (node.IsLeaf)
                {
                    leafs.Add(node);
                }
                else
                {
                    foreach (Node child in node.Children)
                    {
                        stack.Push(child);
                    }
                }                
            }

            return leafs;
        }
        
        public void TranslateIntoTreeView(TreeViewItem treeViewItem, ICommand[] commands, string[] titles)
            => TranslateIntoTreeView(treeViewItem, commands, titles, Root);

        private void TranslateIntoTreeView(TreeViewItem treeViewItem, ICommand[] commands, string[] titles, Node node)
        {
            if (commands.Length > titles.Length)
            {
                throw new ArgumentException();
            }

            ContextMenu menu = new ContextMenu();
            TreeViewItem item = new TreeViewItem
            {
                Header = node.Title,
                ContextMenu = menu
            };

            for (int i = 0; i < commands.Length; i++)
            {
                menu.Items.Add(new MenuItem
                {
                    Header = titles[i],
                    Command = commands[i],
                    CommandParameter = item
                });
            }

            treeViewItem.Items.Add(item);

            if (node.IsLeaf)
            {
                TranslateNodeAttributesIntoTreeView(item, node);
            }
            else
            {
                foreach (Node child in node.Children)
                {
                    TranslateIntoTreeView(item, commands, titles, child);
                }
            }
        }

        private void TranslateNodeAttributesIntoTreeView(TreeViewItem item, Node node)
        {
            if (node.Attributes == null)
            {
                item.Items.Add(new TreeViewItem { Header = "Атрибуты не указаны." });
                return;
            }

            item.Items.Add(new TreeViewItem { Header = $"Средняя цена квадратного метра: {Math.Round(node.Attributes.AvgPricePerSquareMeter, 3)} руб." });
            item.Items.Add(new TreeViewItem { Header = $"Максимальная удалённость до станции метро: {Math.Round(node.Attributes.MaxDistanceToTheMetroStation, 3)} м." });
            item.Items.Add(new TreeViewItem { Header = $"Количество школ на душу населения: {Math.Round(node.Attributes.SchoolsNumberPerCapita, 3)}." });
            item.Items.Add(new TreeViewItem { Header = $"Лучший рейтинг районного университета: {node.Attributes.BestRatingOfTheLocalUniversity}." });
            item.Items.Add(new TreeViewItem { Header = "Есть ли в районе объекты культурного наследия: " + (node.Attributes.AreThereAnyHeritageSites ? "да." : "нет.") });
            TreeViewItem styleItem = new TreeViewItem { Header = "Характерные для района архитектурные стили:" };
            item.Items.Add(styleItem);

            if (node.Attributes.SpecificArchitecturalStyles.Any())
            {
                foreach (ArchitecturalStyle style in node.Attributes.SpecificArchitecturalStyles)
                {
                    styleItem.Items.Add(new TreeViewItem { Header = style.GetDescription() });
                }
            }
            else
            {
                styleItem.Items.Add(new TreeViewItem { Header = "Стили не указаны." });
            }
        }

        public List<Node> ToList()
        {
            List<Node> nodes = new List<Node>();
            ToList(nodes, Root);
            return nodes;
        }

        private void ToList(List<Node> nodes, Node currentNode)
        {
            if (currentNode.IsLeaf)
            {
                nodes.Add(currentNode);
            }
            else
            {
                foreach (Node child in currentNode.Children)
                {
                    ToList(nodes, child);
                }
            }
        }
    }
}
