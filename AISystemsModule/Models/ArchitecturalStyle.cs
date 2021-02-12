using System.ComponentModel;

namespace AISystemsModule.Models
{
    public enum ArchitecturalStyle
    {
        [Description("Нарышкинское барокко")]
        NaryshkinBaroque,
        [Description("Московский ампир")]
        MoscowEmpire,
        [Description("Московский модерн")]
        MoscowModern,
        [Description("Конструктивизм")]
        Constructivism,
        [Description("Сталинский ампир")]
        StalinsEmpire,
        [Description("Позднесоветский модернизм")]
        LateSovietModernism,
        [Description("Лужковский постмодернизм")]
        LuzhkovsPostmodernism
    }
}
