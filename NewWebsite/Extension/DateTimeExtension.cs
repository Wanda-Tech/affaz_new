namespace NewWebsite.Extension;

public static class DateTimeExtension
{

    public static string ToHumanAgoString(this DateTime dateTime)
    {
        TimeSpan diff = DateTime.UtcNow - dateTime;
        if (diff.TotalSeconds < 60)
            return $"{(int)diff.TotalSeconds} seconds ago";
        if (diff.TotalMinutes < 60)
            return $"{(int)diff.TotalMinutes} minutes ago";
        if (diff.TotalHours < 24)
            return $"{(int)diff.TotalHours} hours ago";
        if (diff.TotalDays < 30)
            return $"{(int)diff.TotalDays} days ago";
        if (diff.TotalDays < 365)
            return $"{(int)(diff.TotalDays / 30)} months ago";  
            
        return $"{(int)(diff.TotalDays / 365)} years ago";
    }
    
    public static string ToLocalTimeString(this DateTime dateTime)
    {
        return dateTime.ToLocalTime().ToString("MMMM dd, HH:mm:ss tt zz");
    }
}