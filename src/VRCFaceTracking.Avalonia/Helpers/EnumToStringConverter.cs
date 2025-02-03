using System;
using System.Globalization;
using Avalonia.Data.Converters;
using VRCFaceTracking.Core.Models;

namespace VRCFaceTracking.Helpers;

public class EnumToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is InstallState state)
            if (state == InstallState.NotInstalled)
                return "";
        return $"({value})";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
