using System;
using AISystemsModule.Models;

namespace DataGenerator.Extensions
{
    static class TreeExtension
    {
        /// <summary>
        /// Заполнение листьев дерева случайными данными.
        /// </summary>
        public static void AddAttributesToLeafs(this Tree tree) => tree.Root.AddAttributesToLeafs(new Random());
    }
}
