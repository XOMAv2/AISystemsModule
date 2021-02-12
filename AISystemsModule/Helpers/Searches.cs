using AISystemsModule.Extensions;
using AISystemsModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISystemsModule.Helpers
{
    static class Searches
    {
        public static List<Node> ParametricSearch(Tree tree, SearchRanges ranges)
        {
            return ParametricSearch(tree.ToList(), ranges);
        }

        public static List<Node> ParametricSearch(List<Node> nodes, SearchRanges ranges)
        {
            return nodes
                .Where(n => n.Attributes != null)
                .Select(n =>
                {
                    n.Beans = ranges.AvgPricePerSquareMeter.Contains(n.Attributes.AvgPricePerSquareMeter).ToInt()
                        + ranges.MaxDistanceToTheMetroStation.Contains(n.Attributes.MaxDistanceToTheMetroStation).ToInt()
                        + ranges.SchoolsNumberPerCapita.Contains(n.Attributes.SchoolsNumberPerCapita).ToInt()
                        + ranges.BestRatingOfTheLocalUniversity.Contains(n.Attributes.BestRatingOfTheLocalUniversity).ToInt()
                        + (ranges.AreThereAnyHeritageSites == null || ranges.AreThereAnyHeritageSites == n.Attributes.AreThereAnyHeritageSites).ToInt()
                        + ranges.SpecificArchitecturalStyles.Intersect(n.Attributes.SpecificArchitecturalStyles).Count();

                    return n;
                })
                .OrderByDescending(n => n.Beans)
                .ToList();
        }
    }
}
