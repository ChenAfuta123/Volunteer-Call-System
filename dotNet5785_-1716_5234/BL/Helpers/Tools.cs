using BO;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Helpers;

internal static class Tools
{

    public static bool IsValidID(int Id)
    {
        string id = Id.ToString();

        if (id.Length != 9 || !id.All(char.IsDigit))
            return false;

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            int digit = int.Parse(id[i].ToString());
            sum += (i % 2 == 0) ? digit : digit * 2;
        }
        int checkDigit = (10 - (sum % 10)) % 10;
        return checkDigit == int.Parse(id[8].ToString());
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

            const string LocationIqApiKey = "pk.ddce0bbd11edfee17d07cb35922321f7";
            const string BaseUrl = "https://us1.locationiq.com/v1/search.php";
            Console.WriteLine();

            string url = $"{BaseUrl}?key={LocationIqApiKey}&q={Uri.EscapeDataString(address)}&format=json";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result; // ניתן להשתמש ב-await במקום
                if (!response.IsSuccessStatusCode)
                    return false;

                string jsonResponse = response.Content.ReadAsStringAsync().Result;


                var results = System.Text.Json.JsonSerializer.Deserialize<LocationIqResponse[]>(jsonResponse);

                if (results == null || results.Length == 0)
                    return false;

                if (latitude.HasValue && longitude.HasValue)
                {
                    foreach (var result in results)
                    {
                        if (double.TryParse(result.Lat, out double resultLat) && double.TryParse(result.Lon, out double resultLon))
                        {
                            if (Math.Abs(resultLat - latitude.Value) < 0.01 && Math.Abs(resultLon - longitude.Value) < 0.01)
                                return true;
                        }
                    }

                    return false;
                }

                return true;
            }
        }


        public static double CalculateDistance(string? address1, string? address2, DO.DistanceType distanceType)
        {
            if (string.IsNullOrWhiteSpace(address1) || string.IsNullOrWhiteSpace(address2))
            {
                throw new BlNullPropertyException("Addresses cannot be null or empty.");
            }

            switch (distanceType)
            {
                case DO.DistanceType.AirDistance:
                    return CalculateAirDistance(address1, address2);

                case DO.DistanceType.WalkingDistance:
                    return CalculateWalkingDistance(address1, address2);

                case DO.DistanceType.DrivingDistance:
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
            (double? latitude1, double? longitude1) = GetAddressCoordinates(address1);
            (double? latitude2, double? longitude2) = GetAddressCoordinates(address2);
            double lat1 = latitude1 ?? throw new BO.BlNullPropertyException("Latitude1 is null.");
            double lon1 = longitude1 ?? throw new BO.BlNullPropertyException("Longitude1 is null.");
            double lat2 = latitude2 ?? throw new BO.BlNullPropertyException("Latitude2 is null.");
            double lon2 = longitude2 ?? throw new BO.BlNullPropertyException("Longitude2 is null.");

            return CalculateDistanceBetweenCoordinates(lat1, lon1, lat2, lon2);
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
        public static (double? Latitude, double? Longitude) GetAddressCoordinates(string? address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return (null, null);
            }
            const string LocationIqApiKey = "pk.ddce0bbd11edfee17d07cb35922321f7";
            const string BaseUrl = "https://us1.locationiq.com/v1/search.php";

            string requestUrl = $"{BaseUrl}?key={LocationIqApiKey}&q={Uri.EscapeDataString(address)}&format=json";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error fetching coordinates for address: {response.ReasonPhrase}");
                }

                string responseContent = response.Content.ReadAsStringAsync().Result;


                var locationData = System.Text.Json.JsonSerializer.Deserialize<LocationIqResponse[]>(responseContent);

                if (locationData == null || locationData.Length == 0)
                {
                    throw new Exception($"No coordinates found for address: {address}");
                }

                double latitude = double.Parse(locationData[0].Lat);
                double longitude = double.Parse(locationData[0].Lon);

                return (latitude, longitude);
            }
        }
        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
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
            public required Route[] Routes { get; set; }
        }

        private class Route
        {
            public double Distance { get; set; }
        }

        private class LocationIqResponse
        {
            public required string Lat { get; set; }
            public required string Lon { get; set; }
        }
    }

}