using System.Collections.Immutable;
using System.Reflection;

namespace BlazorApp2.Services
{
    public static class GenericHelper
    {
        public static string[] GetProperties<T>()
        {
            return typeof(T).GetProperties()
                .OrderBy(i => i.Name)
                .Select(p => p.Name)
                .ToArray();
        }

        public static int CountProperties<T>() => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Length;

        public const string CSVFields = "Crime ID,Crime Type,Date,Time,Address,Severity,Description,Weapon Used,Victim Count,Suspect Description,Arrest Made,Arrest Date,Response Time (min),Police District,Weather Condition,Crime Motive,Nearby Landmarks,Recurring Incident (Y/N),Population Density (per sq km),Unemployment Rate (%),Median Income (PHP),Proximity to Police Station (km),Street Light Present (Y/N),CCTV Coverage (Y/N),Alcohol/Drug Involvement (Y/N)";

        private static readonly ImmutableArray<string> _csvFieldsArray = [.. CSVFields.Split(",")];
        public static ImmutableArray<string> CSVFieldsArray => _csvFieldsArray;

        public static bool ArePropertiesAssigned(object obj)
        {
            // Get the type of the object
            Type type = obj.GetType();

            var properties = type.GetProperties();

            // Iterate over all the properties
            foreach (PropertyInfo property in properties)
            {
                // Check if the property is writable and readable
                if (property.CanRead)
                {
                    // Get the value of the property
                    object? value = property.GetValue(obj);

                    // Check if the value is null or the default value for its type
                    if (value == null || IsDefaultValue(value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsDefaultValue(object value)
        {
            return value.Equals(Activator.CreateInstance(value.GetType()));
        }
    }
}
