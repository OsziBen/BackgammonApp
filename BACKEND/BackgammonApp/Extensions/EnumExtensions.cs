namespace WebAPI.Extensions
{
    public static class EnumExtensions
    {
        public static T ParseEnum<T>(this string value) where T : struct
        {
            if (!Enum.TryParse<T>(value, true, out var result))
            {
                throw new ArgumentException($"Invalid value for {typeof(T).Name}: {value}");
            }

            return result;
        }
    }
}
