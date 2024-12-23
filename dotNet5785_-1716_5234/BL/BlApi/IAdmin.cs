namespace BlApi;

/// <summary>
/// Administrative operations interface.
/// </summary>
public interface IAdmin
{
    /// <summary>Get the current clock time.</summary>
    public DateTime getClockTime();

    /// <summary>Advance the clock by a given time unit.</summary>
    public void AdvanceClock(BO.TimeUnit timeUnit);

    /// <summary>Get the risk time range.</summary>
    public TimeSpan getRiskTimeRange();

    /// <summary>Set the risk time range.</summary>
    public void setRiskTimeRange(TimeSpan riskTimeRange);

    /// <summary>Reset the database to its initial state.</summary>
    public void resetDatabase();

    /// <summary>Initialize the database.</summary>
    public void setDatabase();
}
