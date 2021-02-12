namespace AISystemsModule.Extensions
{
    static class BooleanExtension
    {
        public static int ToInt(this bool val) => val ? 1 : 0;
    }
}