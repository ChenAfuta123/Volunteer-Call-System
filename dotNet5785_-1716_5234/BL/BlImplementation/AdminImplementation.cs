
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
    
        var currentTime = AdminManager.Now;

     
        var newTime = timeUnit switch
        {
            TimeUnit.MINUTE => currentTime.AddMinutes(1),
            TimeUnit.HOUR => currentTime.AddHours(1),
            TimeUnit.DAY => currentTime.AddDays(1),
            TimeUnit.MONTH => currentTime.AddMonths(1),
            TimeUnit.YEAR => currentTime.AddYears(1),
            _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), $"Unsupported time unit: {timeUnit}")
        };


        AdminManager.UpdateClock(newTime);
    }

    public  DateTime getClockTime()
    {
        return AdminManager.Now;
    }

    public TimeSpan getRiskTimeRange()
    {
        var riskRange = AdminManager.RiskRange;
        return riskRange;
    }

    public void resetDatabase()
    {
        _dal.ResetDB();
        AdminManager.UpdateClock(AdminManager.Now);
        AdminManager.RiskRange = AdminManager.RiskRange;
    }

    public void setDatabase()
    {
        _dal.ResetDB();
        Initialization.Do();
        AdminManager.UpdateClock(AdminManager.Now);
        AdminManager.RiskRange = AdminManager.RiskRange;

    }

    public void setRiskTimeRange(TimeSpan riskTimeRange)
    {
        AdminManager.RiskRange = riskTimeRange;
    }
    
    public void AddClockObserver(Action clockObserver) =>
    AdminManager.ClockUpdatedObservers += clockObserver;
    public void RemoveClockObserver(Action clockObserver) =>
    AdminManager.ClockUpdatedObservers -= clockObserver;
    public void AddConfigObserver(Action configObserver) =>
   AdminManager.ConfigUpdatedObservers += configObserver;
    public void RemoveConfigObserver(Action configObserver) =>
    AdminManager.ConfigUpdatedObservers -= configObserver;
    
}
