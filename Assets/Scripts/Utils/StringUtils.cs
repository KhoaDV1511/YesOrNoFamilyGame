using System.Globalization;

public class StringUtils
{
    public static string FormatMoney(double value)
    {
        return value.ToString("N0", CultureInfo.GetCultureInfo("de"));
    }
}