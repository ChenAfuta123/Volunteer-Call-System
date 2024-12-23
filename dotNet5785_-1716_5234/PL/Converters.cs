using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL;

public class ConvertUpdateToVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string? buttonText = value as string;

        // אם מדובר במצב "Update", השדה יהיה גלוי, אחרת מוסתר
        return buttonText == "Update" ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertUpdateToTrue : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string? buttonText = value as string;

        // אם מדובר במצב "Update", שדה ה-Id יהיה לקריאה בלבד
        return buttonText == "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
