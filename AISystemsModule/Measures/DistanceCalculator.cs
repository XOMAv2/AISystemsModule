using AISystemsModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AISystemsModule.Measures
{
    class DistanceCalculator
    {
        private readonly Node node1;
        private readonly Node node2;

        public DistanceCalculator(Node node1, Node node2) => (this.node1, this.node2) = (node1, node2);

        public double CalculateDistance(MeasureType type = MeasureType.EuclideanDistance)
        {
            return type switch
            {
                MeasureType.EuclideanDistance => CalculateEuclideanDistance(node1, node2),
                MeasureType.ManhattanDistance => CalculateManhattanDistance(node1, node2),
                MeasureType.Correlation => CalculateCorrelationDistance(node1, node2),
                MeasureType.TreeDistance => CalculateTreeDistance(node1, node2),
                _ => 0,
            };
        }

        /// <summary> Евклидово расстояние. </summary>
        public static double CalculateEuclideanDistance(Node x, Node y)
        {
            NodeAttributes n1 = x.Attributes ?? new NodeAttributes();
            NodeAttributes n2 = y.Attributes ?? new NodeAttributes();

            double a1 = Math.Pow(n1.AvgPricePerSquareMeter - n2.AvgPricePerSquareMeter, 2);
            double a2 = Math.Pow(n1.MaxDistanceToTheMetroStation - n2.MaxDistanceToTheMetroStation, 2);
            double a3 = Math.Pow(n1.SchoolsNumberPerCapita - n2.SchoolsNumberPerCapita, 2);
            double a4 = Math.Pow(n1.BestRatingOfTheLocalUniversity - n2.BestRatingOfTheLocalUniversity, 2);
            double a5 = Math.Pow((n1.AreThereAnyHeritageSites ? 1 : 0) - (n2.AreThereAnyHeritageSites ? 1 : 0), 2);

            return Math.Sqrt(a1 + a2 + a3 + a4 + a5);
        }

        /// <summary> Манхэттенское расстояние. </summary>
        public static double CalculateManhattanDistance(Node x, Node y)
        {
            NodeAttributes n1 = x.Attributes ?? new NodeAttributes();
            NodeAttributes n2 = y.Attributes ?? new NodeAttributes();

            double a1 = Math.Abs(n1.AvgPricePerSquareMeter - n2.AvgPricePerSquareMeter);
            double a2 = Math.Abs(n1.MaxDistanceToTheMetroStation - n2.MaxDistanceToTheMetroStation);
            double a3 = Math.Abs(n1.SchoolsNumberPerCapita - n2.SchoolsNumberPerCapita);
            double a4 = Math.Abs(n1.BestRatingOfTheLocalUniversity - n2.BestRatingOfTheLocalUniversity);
            double a5 = Math.Abs((n1.AreThereAnyHeritageSites ? 1 : 0) - (n2.AreThereAnyHeritageSites ? 1 : 0));

            return a1 + a2 + a3 + a4 + a5;
        }

        /// <summary> Расстояние между узлами дерева. </summary>
        public double CalculateTreeDistance(Node x, Node y)
        {
            var distances = new Dictionary<string, double>()
            {
                { node1.Title, 0 }
            };

            var stack = new Stack<Node>();
            stack.Push(x);

            while (stack.Any())
            {
                Node node = stack.Pop();
                var nodes = node.Parent == null
                    ? node.Children
                    : node.Children.Union(new[] { node.Parent });

                foreach (Node child in nodes)
                {
                    if (!distances.TryGetValue(child.Title, out _))
                    {
                        distances.Add(child.Title, distances[node.Title] + 1);
                        stack.Push(child);
                    }
                }
            }

            return distances[y.Title];
        }

        /// <summary>
        /// Линейный коэффициент корреляции (коэффициент корреляции Пирсона).
        /// </summary>
        public static double CalculateCorrelationDistance(Node x, Node y)
        {
            NodeAttributes n1 = x.Attributes ?? new NodeAttributes();
            NodeAttributes n2 = y.Attributes ?? new NodeAttributes();

            double avg1 = (n1.AvgPricePerSquareMeter
                + n1.MaxDistanceToTheMetroStation
                + n1.SchoolsNumberPerCapita
                + n1.BestRatingOfTheLocalUniversity
                + (n1.AreThereAnyHeritageSites ? 1 : 0)) / 5;
            double avg2 = (n2.AvgPricePerSquareMeter
                + n2.MaxDistanceToTheMetroStation
                + n2.SchoolsNumberPerCapita
                + n2.BestRatingOfTheLocalUniversity
                + (n2.AreThereAnyHeritageSites ? 1 : 0)) / 5;

            double[] a1 = new[]
            {
                n1.AvgPricePerSquareMeter - avg1,
                n1.MaxDistanceToTheMetroStation - avg1,
                n1.SchoolsNumberPerCapita - avg1,
                n1.BestRatingOfTheLocalUniversity - avg1,
                (n1.AreThereAnyHeritageSites ? 1 : 0) - avg1,
            };
            double[] a2 = new[]
            {
                n2.AvgPricePerSquareMeter - avg2,
                n2.MaxDistanceToTheMetroStation - avg2,
                n2.SchoolsNumberPerCapita - avg2,
                n2.BestRatingOfTheLocalUniversity - avg2,
                (n2.AreThereAnyHeritageSites ? 1 : 0) - avg2,
            };

            double numerator = a1.Select((a, i) => a * a2[i]).Sum();
            double denominator = a1.Select(a => a * a).Sum() * a2.Select(a => a * a).Sum();

            return numerator / Math.Sqrt(denominator);
        }
    }
}
