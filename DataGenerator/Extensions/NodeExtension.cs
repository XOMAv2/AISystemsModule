using AISystemsModule.Models;
using System;
using System.Collections.Generic;

namespace DataGenerator.Extensions
{
    static class NodeExtension
    {
        private static (double Min, double Max) avgPriceRange = (40000, 2000000);
        private static (double Min, double Max) maxDistanceRange = (1000, 10000);
        private static (double Min, double Max) schoolsNumberRange = (0.001, 0.01);
        private static (int Min, int Max) universityRatingRange = (100, 5000);

        /// <summary>
        /// Поиск листьев среди нод и заполнение их случайными данными.
        /// </summary>
        public static void AddAttributesToLeafs(this Node node)
        {
            node.AddAttributesToLeafs(new Random());
        }

        /// <summary>
        /// Поиск листьев среди нод и заполнение их случайными данными.
        /// </summary>
        public static void AddAttributesToLeafs(this Node node, Random rand)
        {
            if (node.IsLeaf)
            {
                List<ArchitecturalStyle> styles = new List<ArchitecturalStyle>();

                foreach (var v in Enum.GetValues(typeof(ArchitecturalStyle)))
                {
                    styles.Add((ArchitecturalStyle)v!);
                }

                int styleCount = rand.Next(styles.Count + 1);

                for (int i = 0; i < styleCount; i++)
                {
                    styles.RemoveAt(rand.Next(styles.Count));
                }

                node.Attributes = new NodeAttributes
                {
                    AvgPricePerSquareMeter = rand.NextDouble(avgPriceRange.Min, avgPriceRange.Max),
                    MaxDistanceToTheMetroStation = rand.NextDouble(maxDistanceRange.Min, maxDistanceRange.Max),
                    SchoolsNumberPerCapita = rand.NextDouble(schoolsNumberRange.Min, schoolsNumberRange.Max),
                    BestRatingOfTheLocalUniversity = rand.Next(universityRatingRange.Min, universityRatingRange.Max),
                    AreThereAnyHeritageSites = rand.Next(2) == 1,
                    SpecificArchitecturalStyles = new HashSet<ArchitecturalStyle>(styles)
                };
            }
            else
            {
                foreach (var child in node.Children)
                {
                    child.AddAttributesToLeafs(rand);
                }
            }
        }
    }
}
