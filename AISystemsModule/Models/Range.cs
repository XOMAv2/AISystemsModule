using System;
using System.Collections.Generic;
using System.Text;

namespace AISystemsModule.Models
{
    class Range
    {
        /// <summary> Нижняя граница диапазона (включительно). </summary>
        public double? From { get; }

        /// <summary> Верхняя граница диапазона (включительно). </summary>
        public double? To { get; }

        /// <summary>
        /// Аргуме нты from и to могут быть равны.
        /// При равенстве аргумента null'у он будет воспринят как + или - бесконечность.
        /// </summary>
        public Range(double? from, double? to)
        {
            if (from != null && to != null && from > to)
            {
                throw new ArgumentException("Нижняя граница диапазона больше верхней.");
            }

            From = from;
            To = to;
        }

        /// <summary>
        /// Конструктор для создания диапазона от - бесконечности до + бесконечности.
        /// </summary>
        public Range() => (From, To) = (null, null);

        /// <summary>
        /// Проверка принадлежности x диапазону.
        /// </summary>
        public bool Contains(double x)
        {
            return (From, To) switch
            {
                (null, null) => true,
                (null, _) => x <= To,
                (_, null) => x >= From,
                _ => (x >= From) && (x <= To)
            };
        }
    }
}
