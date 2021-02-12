using AISystemsModule.Helpers;
using System.Collections.Generic;

namespace AISystemsModule.Models
{
    public class NodeAttributes : Observable
    {
        // Количественные параметры.
        private double avgPricePerSquareMeter;
        private double maxDistanceToTheMetroStation;
        private double schoolsNumberPerCapita;
        private int bestRatingOfTheLocalUniversity;

        // Бинарный параметр.
        private bool areThereAnyHeritageSitesInTheArea;

        // Параметр, не переводимый в количественные.
        private HashSet<ArchitecturalStyle> specificArchitecturalStyles = new HashSet<ArchitecturalStyle>();

        /// <summary> Средняя цена квадратного метра в рублях. </summary>
        public double AvgPricePerSquareMeter
        {
            get => avgPricePerSquareMeter;
            set => Set(ref avgPricePerSquareMeter, value);
        }

        /// <summary> Максимальная удалённость до станции метро в метрах. </summary>
        public double MaxDistanceToTheMetroStation
        {
            get => maxDistanceToTheMetroStation;
            set => Set(ref maxDistanceToTheMetroStation, value);
        }

        /// <summary> Количество школ на душу населения. </summary>
        public double SchoolsNumberPerCapita
        {
            get => schoolsNumberPerCapita;
            set => Set(ref schoolsNumberPerCapita, value);
        }

        /// <summary> Лучший рейтинг районного университета (чем меньше, тем лучше). </summary>
        public int BestRatingOfTheLocalUniversity
        {
            get => bestRatingOfTheLocalUniversity;
            set => Set(ref bestRatingOfTheLocalUniversity, value);
        }

        /// <summary> Есть ли в районе объекты культурного наследия. </summary>
        public bool AreThereAnyHeritageSites
        {
            get => areThereAnyHeritageSitesInTheArea;
            set => Set(ref areThereAnyHeritageSitesInTheArea, value);
        }

        /// <summary> Характерные для района архитектурные стили. </summary>
        public HashSet<ArchitecturalStyle> SpecificArchitecturalStyles
        {
            get => specificArchitecturalStyles;
            set => Set(ref specificArchitecturalStyles, value);
        }
    }
}
