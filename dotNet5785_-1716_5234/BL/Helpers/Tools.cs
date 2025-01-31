using BO;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
//namespace Helpers;

internal static class Tools
{

    public static bool IsValidID(int Id)
    {
        string id = Id.ToString();

        if (id.Length != 9 || !id.All(char.IsDigit))
            return false;

        return true;
        //int sum = 0;
        //for (int i = 0; i < 8; i++)
        //{
        //    int digit = int.Parse(id[i].ToString());
        //    sum += (i % 2 == 0) ? digit : digit * 2;
        //}
        //int checkDigit = (10 - (sum % 10)) % 10;
        //return checkDigit == int.Parse(id[8].ToString());
    }
    public static void NullVal<T>(T? value) where T : struct
    {
        if (!value.HasValue)
            throw new BlNullPropertyException($"{nameof(value)} is null.");
    }
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
        public static bool IsValidAddress(string? address, double? longitude, double? latitude)
        {
            if (longitude.HasValue)
            {
                if (longitude < -180 || longitude > 180)
                    return false;
            }

            if (latitude.HasValue)
            {
                if (latitude < -90 || latitude > 90)
                    return false;
            }

            if (string.IsNullOrWhiteSpace(address))
                return false;

            const string LocationIqApiKey = "pk.67d4c5ff0db38922a88ae34ae0522e52";
            const string BaseUrl = "https://us1.locationiq.com/v1/search.php";

            string url = $"{BaseUrl}?key={LocationIqApiKey}&q={Uri.EscapeDataString(address)}&format=json";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                    return false;

                string jsonResponse = response.Content.ReadAsStringAsync().Result;

                // שימוש ב-Regex לחיפוש lat ו-lon
                var latMatch = System.Text.RegularExpressions.Regex.Match(jsonResponse, "\"lat\":\"(?<lat>-?\\d+(?:\\.\\d+)?)\"");
                var lonMatch = System.Text.RegularExpressions.Regex.Match(jsonResponse, "\"lon\":\"(?<lon>-?\\d+(?:\\.\\d+)?)\"");

                if (latMatch.Success && lonMatch.Success &&
                    double.TryParse(latMatch.Groups["lat"].Value, out double resultLat) &&
                    double.TryParse(lonMatch.Groups["lon"].Value, out double resultLon))
                {
                    if (latitude.HasValue && longitude.HasValue)
                    {
                        if (Math.Abs(resultLat - latitude.Value) < 0.01 && Math.Abs(resultLon - longitude.Value) < 0.01)
                            return true;

                        return false;
                    }

                    return true;
                }

                return false;
            }
        }


        public static async Task<(double? Latitude, double? Longitude)> GetAddressCoordinatesAsync(string? address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return (null, null);
            }

            const string LocationIqApiKey = "pk.67d4c5ff0db38922a88ae34ae0522e52";
            const string BaseUrl = "https://us1.locationiq.com/v1/search.php";

            string requestUrl = $"{BaseUrl}?key={LocationIqApiKey}&q={Uri.EscapeDataString(address)}&format=json";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error fetching coordinates for address: {response.ReasonPhrase}");
                }

                string responseContent = await response.Content.ReadAsStringAsync();

                // שימוש ב-Regex לחיפוש lat ו-lon
                var latMatch = System.Text.RegularExpressions.Regex.Match(responseContent, "\"lat\":\"(?<lat>-?\\d+(?:\\.\\d+)?)\"");
                var lonMatch = System.Text.RegularExpressions.Regex.Match(responseContent, "\"lon\":\"(?<lon>-?\\d+(?:\\.\\d+)?)\"");

                if (latMatch.Success && lonMatch.Success &&
                    double.TryParse(latMatch.Groups["lat"].Value, out double latitude) &&
                    double.TryParse(lonMatch.Groups["lon"].Value, out double longitude))
                {
                    return (latitude, longitude);
                }

                throw new Exception("Unable to parse coordinates from response.");
            }
        }

        public static double CalculateDistance(double? latitude1, double? longitude1, double? latitude2, double? longitude2, DO.DistanceType distanceType)
        {
            if (!latitude1.HasValue || !longitude1.HasValue || !latitude2.HasValue || !longitude2.HasValue)
            {
                throw new ArgumentNullException("Latitude and Longitude cannot be null.");
            }

            switch (distanceType)
            {
                case DO.DistanceType.AirDistance:
                    return CalculateAirDistance(latitude1.Value, longitude1.Value, latitude2.Value, longitude2.Value);

                case DO.DistanceType.WalkingDistance:
                    return CalculateWalkingDistance(latitude1.Value, longitude1.Value, latitude2.Value, longitude2.Value);

                case DO.DistanceType.DrivingDistance:
                    return CalculateDrivingDistance(latitude1.Value, longitude1.Value, latitude2.Value, longitude2.Value);



                default:
                    throw new ArgumentException("Invalid distance type.");
            }
        }

        private static double CalculateAirDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            return CalculateDistanceBetweenCoordinates(latitude1, longitude1, latitude2, longitude2);
        }

        private static double CalculateWalkingDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double airDistance = CalculateDistanceBetweenCoordinates(latitude1, longitude1, latitude2, longitude2);
            const double walkingMultiplier = 1.3; // מקדם למרחק הליכה
            return airDistance * walkingMultiplier;
        }

        private static double CalculateDrivingDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double airDistance = CalculateDistanceBetweenCoordinates(latitude1, longitude1, latitude2, longitude2);
            const double drivingMultiplier = 1.5; // מקדם למרחק נסיעה
            return airDistance * drivingMultiplier;
        }

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

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }
    }




}