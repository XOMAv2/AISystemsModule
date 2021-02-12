using AISystemsModule.Helpers;
using AISystemsModule.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace AISystemsModule.ViewModels
{
    partial class MainViewModel : Observable
    {
        private void ConfigTreeAndTreeView(Node? root = null)
        {
            try
            {
                MoscowTree = new Tree(root);
                MoscowTree.SetParents();

                var rootViewItem = new TreeViewItem();
                rootViewItem.Header = "Дерево";
                MoscowTree.TranslateIntoTreeView(
                    rootViewItem,
                    new[] { AddAsNode1Command, AddAsNode2Command, AddToChosen, AddToBlackList },
                    new[]
                    {
                        "Установить как начальную ноду",
                        "Установить как конечную ноду",
                        "Добавить в список выбранных пользователем нод",
                        "Добавить в чёрный список"
                    });
                var treeViewItems = new ObservableCollection<TreeViewItem>();
                treeViewItems.Add(rootViewItem);
                MoscowTreeView = treeViewItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("В процессе выполнения программы произошла ошибка.\n" + ex.Message);
            }
        }

        private void LoadTreeFromFile()
        {
            try
            {
                if (SourcePath != "")
                {
                    string text = File.ReadAllText(SourcePath);
                    Node root = JsonSerializer.Deserialize<Node>(text);
                    ConfigTreeAndTreeView(root);
                }
                else
                {
                    MessageBox.Show($"Не указан путь к json-файлу.");
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("В процессе загрузки дерева произошла ошибка.\nУказан неверный путь к файлу.");
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("В процессе загрузки дерева произошла ошибка.\nУказанный путь не существует.");
            }
            catch (JsonException)
            {
                MessageBox.Show("В процессе загрузки дерева произошла ошибка.\nНеверный формат json-файла.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("В процессе выполнения программы произошла ошибка.\n" + ex.Message);
            }
        }

        private void GenerateUsers(int userCount = 6)
        {
            Users = new ObservableCollection<User>();
            List<Node> leafs = MoscowTree.GetLeafs();
            Random rand = new Random();

            for (int i = 0; i < userCount; i++)
            {
                ObservableCollection<Node> chosenLeafs = new ObservableCollection<Node>();
                HashSet<int> chosenLeafIndexes = new HashSet<int>();
                int chosenCount = rand.Next(10);

                for (int j = 0; j < chosenCount; j++)
                {
                    chosenLeafIndexes.Add(rand.Next(leafs.Count));
                }

                foreach (int index in chosenLeafIndexes)
                {
                    chosenLeafs.Add(leafs[index]);
                }

                Users.Add(new User($"Пользователь №{i + 1}", chosenLeafs));
            }
        }
    }
}
