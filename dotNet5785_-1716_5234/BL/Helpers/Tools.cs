
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Helpers;

internal static class Tools
{
    public static string ToStringProperty<T>(this T obj)
    {
        if (obj == null)
            return "null";

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sb = new StringBuilder();

        sb.AppendLine($"Type: {type.Name}");
        foreach (var prop in properties)
        {
            try
            {
                var value = prop.GetValue(obj);

                if (value is IEnumerable enumerable && value.GetType() != typeof(string))
                {
                    sb.AppendLine($"  {prop.Name}: [");
                    foreach (var item in enumerable)
                    {
                        sb.AppendLine($"    {item?.ToStringProperty()}");
                    }
                    sb.AppendLine($"  ]");
                }
                else
                {
                    sb.AppendLine($"  {prop.Name}: {value ?? "null"}");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"  {prop.Name}: Error retrieving value ({ex.Message})");
            }
        }

        return sb.ToString();
    }
    public static class DistanceCalculator
    {
        public static double CalculateDistance(string address1, string address2, DistanceType distanceType)
        {
            if (string.IsNullOrWhiteSpace(address1) || string.IsNullOrWhiteSpace(address2))
            {
                throw new ArgumentException("Addresses cannot be null or empty.");
            }

            switch (distanceType)
            {
                case DistanceType.AirDistance:
                    return CalculateAirDistance(address1, address2);

                case DistanceType.WalkingDistance:
                    return CalculateWalkingDistance(address1, address2);

                case DistanceType.DrivingDistance:
                    return CalculateDrivingDistance(address1, address2);

                default:
                    throw new ArgumentOutOfRangeException(nameof(distanceType), "Invalid distance type.");
            }
        }

        /// <summary>
        /// calulate the air distance with the coordinates
        /// </summary>
        private static double CalculateAirDistance(string address1, string address2)
        {
            var (latitude1, longitude1) = GetAddressCoordinates(address1);
            var (latitude2, longitude2) = GetAddressCoordinates(address2);

            return CalculateDistanceBetweenCoordinates(latitude1, longitude1, latitude2, longitude2);
        }


        private static double CalculateWalkingDistance(string address1, string address2)
        {
            return CalculateTravelDistance(address1, address2, "foot");
        }

        private static double CalculateDrivingDistance(string address1, string address2)
        {
            return CalculateTravelDistance(address1, address2, "driving");
        }

        /// <summary>
        /// calculate driving and waliking distance
        /// </summary>=
        private static double CalculateTravelDistance(string address1, string address2, string mode)
        {
            const string LocationIqApiKey = "pk.ddce0bbd11edfee17d07cb35922321f7";
            const string BaseUrl = "https://us1.locationiq.com/v1/directions/";

            var (latitude1, longitude1) = GetAddressCoordinates(address1);
            var (latitude2, longitude2) = GetAddressCoordinates(address2);

            string requestUrl = $"{BaseUrl}{mode}/{longitude1},{latitude1};{longitude2},{latitude2}?key={LocationIqApiKey}&overview=false";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(requestUrl).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error fetching route data: {response.ReasonPhrase}");
                }

                string responseContent = response.Content.ReadAsStringAsync().Result;

                var routeData = System.Text.Json.JsonSerializer.Deserialize<LocationIqDirectionsResponse>(responseContent);

                if (routeData == null || routeData.Routes == null || routeData.Routes.Length == 0)
                {
                    throw new Exception("No route data found for the provided addresses.");
                }

                return routeData.Routes[0].Distance / 1000.0;
            }
        }

        /// <summary>
        /// calculate the distances between coordinates
        /// </summary>
        private static double CalculateDistanceBetweenCoordinates(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            const double EarthRadiusKm = 6371.0;

            double latitude1Rad = DegreesToRadians(latitude1);
            double longitude1Rad = DegreesToRadians(longitude1);
            double latitude2Rad = DegreesToRadians(latitude2);
            double longitude2Rad = DegreesToRadians(longitude2);

            double deltaLatitude = latitude2Rad - latitude1Rad;
            double deltaLongitude = longitude2Rad - longitude1Rad;

            double a = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) +
                       Math.Cos(latitude1Rad) * Math.Cos(latitude2Rad) *
                       Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }



        private class LocationIqDirectionsResponse
        {
            public Route[] Routes { get; set; }
        }

        private class Route
        {
            public double Distance { get; set; }
        }

        private class LocationIqResponse
        {
            public string Lat { get; set; }
            public string Lon { get; set; }
        }
    }

}
