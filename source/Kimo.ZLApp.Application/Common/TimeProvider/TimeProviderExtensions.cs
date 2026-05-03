namespace Kimo.ZLApp.Application.Common.TimeProvider;

public static class TimeProviderExtensions
{
    public static DateOnly GetLocalToday(this System.TimeProvider provider)
    {
        return DateOnly.FromDateTime(provider.GetLocalNow().DateTime);
    }
}