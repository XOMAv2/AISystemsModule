using AISystemsModule.Measures;
using AISystemsModule.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AISystemsModule.Helpers
{
    static class Recommendations
    {
        public static List<Node> GenerateCollabRecommendations(IList<User> users, User currentUser)
        {
            users.Select(u => u.Rate = u.Chosen.Intersect(currentUser.Chosen).Count()).Count();

            List<User> usersByRate = users.OrderByDescending(u => u.Rate).ToList();

            List<Node> recommendations = new List<Node>();

            foreach (User user in usersByRate)
            {
                // Для отображения в UI числа совпавших нод между текущим пользователем
                // и каждым другим пользователем.
                user.Chosen.Select(n => n.Rate = user.Rate).Count();

                recommendations.AddRange(user.Chosen.Except(currentUser.Chosen));
            }

            return new List<Node>(recommendations.Distinct().Except(currentUser.BlackList));
        }

        public static List<Node> GenerateContentRecommendations(Tree tree, User currentUser, MeasureType mType)
        {
            return GenerateContentRecommendations(tree.ToList(), currentUser, mType);
        }

        public static List<Node> GenerateContentRecommendations(List<Node> nodes, User currentUser, MeasureType mType)
        {
            if (currentUser.Chosen.Count == 0)
            {
                return new List<Node>();
            }

            nodes = nodes
                .Except(currentUser.Chosen)
                .Except(currentUser.BlackList)
                .ToList();

            foreach (Node node in nodes)
            {
                double distanceSum = currentUser.Chosen
                    .Select(n => new DistanceCalculator(node, n).CalculateDistance(mType))
                    .Sum();
                node.Distance = distanceSum / currentUser.Chosen.Count;
            }

            return nodes.OrderBy(n => n.Distance).ToList();
        }
    }
}
