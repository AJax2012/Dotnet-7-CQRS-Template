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

    /// <summary>
    /// Gets testable current DateTime.
    /// </summary>
    public static DateTime Now => InjectedDateTime.Value ?? DateTime.Now;

    /// <summary>
    /// Create Disposable wrapper for injecting DateTime into tests.
    /// </summary>
    /// <param name="actualDateTime">DateTime to inject.</param>
    /// <returns>DateTime Provider implementation.</returns>
    public static IDisposable InjectActualDateTime(DateTime actualDateTime)
    {
        InjectedDateTime.Value = actualDateTime;

        return new DateTimeProvider();
    }

    /// <summary>
    /// Dispose method for IDisposable interface.
    /// </summary>
    public void Dispose()
    {
        InjectedDateTime.Value = null;
    }
}