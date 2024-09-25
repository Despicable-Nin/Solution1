using System.Reflection;

namespace BlazorApp2.Services
{
    public static class GenericHelper
    {
        public static string[] GetProperties<T>()
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToArray();
        }

        public static int CountProperties<T>() => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Length;
    }
}
