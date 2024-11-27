
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


}
