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
    }
}
