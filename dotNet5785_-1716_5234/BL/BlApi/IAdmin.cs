
namespace BlApi;

public interface IAdmin
{
    public DateTime getClockTime();
    public void AdvanceClock(BO.TimeUnit timeUnit);
    public TimeSpan getRiskTimeRange();
    public void setRiskTimeRange(TimeSpan riskTimeRange);
    public void resetDatabase();
    public void setDatabase();
}
