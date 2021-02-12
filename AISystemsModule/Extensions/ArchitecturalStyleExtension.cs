using AISystemsModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AISystemsModule.Extensions
{
    static class ArchitecturalStyleExtension
    {
        public static string GetDescription(this ArchitecturalStyle style)
        {
            FieldInfo fieldInfo = style.GetType().GetField(style.ToString()) ?? throw new Exception();
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0
                ? attributes[0].Description
                : style.ToString();
        }

        public static string[] GetAllDescriptions()
        {
            FieldInfo[] fieldInfos = typeof(ArchitecturalStyle).GetFields().Where(fi => fi.Name != "value__").ToArray();
            List<string> descriptions = new List<string>();

            foreach (FieldInfo fi in fieldInfos)
            {
                var attrs = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                descriptions.Add(attrs.Length > 0 ? attrs[0].Description : fi.Name);
            }

            return descriptions.ToArray();
        }

        public static ArchitecturalStyle? GetValueByDescription(string description)
        {
            FieldInfo[] fieldInfos = typeof(ArchitecturalStyle).GetFields().Where(fi => fi.Name != "value__").ToArray();

            foreach (FieldInfo fi in fieldInfos)
            {
                var attrs = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string fiDescription = attrs.Length > 0 ? attrs[0].Description : fi.Name;

                if (fiDescription == description)
                {
                    return (ArchitecturalStyle?)fi.GetValue(null);
                }
            }

            return null;
        }
    }
}
