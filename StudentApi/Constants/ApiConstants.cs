namespace StudentApi.Constants;

public class ApiConstants
{
    public static readonly HashSet<string> ValidFileTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        ".xlsx", ".xls"
    };

    #region Redis
    public const int CachedTimeOutInSec = 60;
    public const string RedisInstanceName = "StudentApi";
    #endregion

    #region Rate Limiter
    public const string UnknownIpAddress = "Unknown";
    public const int PermitLimitPerWindow = 10;
    public const int WindowTimeSpanInSec = 30;
    public const int WindowQueueLimit = 0;
    #endregion
}
