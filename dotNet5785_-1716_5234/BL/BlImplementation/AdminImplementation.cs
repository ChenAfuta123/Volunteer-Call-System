
using BlApi;
using BO;
using DalTest;
using Helpers;

namespace BlImplementation;

internal class AdminImplementation : IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void AdvanceClock(TimeUnit timeUnit)
    {
        // שלב 1: קבלת הזמן הנוכחי
        var currentTime = ClockManager.Now;

        // שלב 2: חישוב הזמן החדש לפי יחידת הזמן שנבחרה
        var newTime = timeUnit switch
        {
            TimeUnit.MINUTE => currentTime.AddMinutes(1),
            TimeUnit.HOUR => currentTime.AddHours(1),
            TimeUnit.DAY => currentTime.AddDays(1),
            TimeUnit.MONTH => currentTime.AddMonths(1),
            TimeUnit.YEAR => currentTime.AddYears(1),
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), $"Unsupported time unit: {timeUnit}")
        };

        // שלב 3: עדכון השעון עם הזמן החדש
        ClockManager.UpdateClock(newTime);
    }

    public  DateTime getClockTime()
    {
        return DateTime.Now;
    }

    public TimeSpan getRiskTimeRange()
    {
        var riskRange = _dal.Config.RiskRange;
        return riskRange;
    }

    public void resetDatabase()
    {
        _dal.ResetDB();
    }

    public void setDatabase()
    {
        _dal.ResetDB();
        Initialization.Do();
    }

    public void setRiskTimeRange(TimeSpan riskTimeRange)
    {
        _dal.Config.RiskRange= riskTimeRange;
    }
}
