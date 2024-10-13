using System.Reflection;

namespace BlazorApp2.Services.Crimes.Extensions
{
    public static class CrimesDashboartDtoExtension
    {
        public static bool HasRequiredValues(this CrimeDashboardDto dto)
        {
            // Get the type of the object
            Type type = dto.GetType();

            var properties = type.GetProperties();

            string[] exceptions = ["BatchID","SeverityId","Latitude","Longitude","RawDate","RawTime","PoliceDistrictId","PrecinctId","CrimeTypeId","CrimeMotiveId","WeatherConditionId","WeatherId","Description","NearbyLandmarks"];

            // Iterate over all the properties
            foreach (PropertyInfo property in properties)
            {

                if (exceptions.Contains(property.Name)) continue;

                // Check if the property is writable and readable
                if (property.CanRead)
                {
                    // Get the value of the property
                    object? value = property.GetValue(dto);

                    // Check if the value is null or the default value for its type
                    if (value == null || IsDefaultValue(value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static bool IsDefaultValue(object value)
        {
            return value.Equals(Activator.CreateInstance(value.GetType()));
        }
    }
}
