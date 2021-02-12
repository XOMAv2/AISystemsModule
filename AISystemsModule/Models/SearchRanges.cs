using System;
using System.Collections.Generic;
using System.Text;

namespace AISystemsModule.Models
{
    class SearchRanges
    {
        public Range AvgPricePerSquareMeter { get; }
        public Range MaxDistanceToTheMetroStation { get; }
        public Range SchoolsNumberPerCapita { get; }
        public Range BestRatingOfTheLocalUniversity { get; }
        public bool? AreThereAnyHeritageSites { get; }
        public HashSet<ArchitecturalStyle> SpecificArchitecturalStyles { get; }

        public SearchRanges(Range? avgPricePerSquareMeter = null,
                            Range? maxDistanceToTheMetroStation = null,
                            Range? schoolsNumberPerCapita = null,
                            Range? bestRatingOfTheLocalUniversity = null,
                            bool? areThereAnyHeritageSites = null,
                            HashSet<ArchitecturalStyle>? specificArchitecturalStyles = null)
        {
            AvgPricePerSquareMeter = avgPricePerSquareMeter ?? new Range();
            MaxDistanceToTheMetroStation = maxDistanceToTheMetroStation ?? new Range();
            SchoolsNumberPerCapita = schoolsNumberPerCapita ?? new Range();
            BestRatingOfTheLocalUniversity = bestRatingOfTheLocalUniversity ?? new Range();
            AreThereAnyHeritageSites = areThereAnyHeritageSites;
            SpecificArchitecturalStyles = specificArchitecturalStyles ?? new HashSet<ArchitecturalStyle>();
        }
    }
}
