using System;

/// <summary>
/// Datetime helper class.
/// </summary>
public static class DateTimeHelper
{
    public static DateTime DateTime1970 = new DateTime(1970, 1, 1).ToLocalTime();

    /// <summary>
    /// Get the number of milliseconds from 1970-01-01 to the present.
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStamp()
    {
        return (long) (DateTime.Now.ToLocalTime() - DateTime1970).TotalSeconds;
    }

    /// <summary>
    /// Calculate the number of milliseconds from 1970-01-01 to the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long GetTimeStamp(DateTime dateTime)
    {
        return (long) (dateTime.ToLocalTime() - DateTime1970).TotalSeconds * 1000;
    }
}