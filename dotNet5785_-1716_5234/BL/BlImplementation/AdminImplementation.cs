using BlApi;
using BO;
using Helpers;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;
internal class AdminImplementation : IAdmin
{
    public void AdvanceClock(TimeUnit timeUnit)
    {
        throw new NotImplementedException();
    }

    public DateTime getClockTime()
    {
        throw new NotImplementedException();
    }

    public TimeSpan getRiskTimeRange()
    {
        throw new NotImplementedException();
    }

    public void resetDatabase()
    {
        throw new NotImplementedException();
    }

    public void setDatabase()
    {
        throw new NotImplementedException();
    }

    public void setRiskTimeRange(TimeSpan riskTimeRange)
    {
        throw new NotImplementedException();
    }
}
