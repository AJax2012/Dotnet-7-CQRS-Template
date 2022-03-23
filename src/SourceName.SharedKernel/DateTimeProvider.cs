namespace SourceName.SharedKernel;

/// <summary>
/// In a Micro Services project, this would be moved to another nuget package.
/// </summary>
public class DateTimeProvider : IDisposable
{
    private static readonly AsyncLocal<DateTime?> InjectedDateTime = new AsyncLocal<DateTime?>(); 
 
    private DateTimeProvider() 
    { 
    } 
 
    public static DateTime Now => InjectedDateTime.Value ?? DateTime.Now;
 
    public static IDisposable InjectActualDateTime(DateTime actualDateTime) 
    { 
        InjectedDateTime.Value = actualDateTime; 
 
        return new DateTimeProvider(); 
    }

    public void Dispose()
    {
        InjectedDateTime.Value = null;
    }
}