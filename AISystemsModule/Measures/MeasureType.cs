using System;
using System.Collections.Generic;
using System.Text;

namespace AISystemsModule.Measures
{
    enum MeasureType
    {
        /// <summary> Евклидово расстояние. </summary>
        EuclideanDistance,
        /// <summary> Манхэттенское расстояние. </summary>
        ManhattanDistance,
        /// <summary> Расстояние между узлами дерева. </summary>
        TreeDistance,
        /// <summary>
        /// Линейный коэффициент корреляции (коэффициент корреляции Пирсона).
        /// </summary>
        Correlation
    }
}
