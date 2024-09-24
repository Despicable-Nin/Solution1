using System.Reflection;

namespace BlazorApp2.Helpers;

public static class GenericHelper
{
    public static int CountProperties<T>() => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Length;
}
