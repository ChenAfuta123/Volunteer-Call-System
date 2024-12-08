using BlApi;
using BO;
using DalApi;
using DO;
using System.Net;
using System.Xml.Linq;
namespace Helpers;

internal static class CallManager
{
    private static IDal s_dal = Factory.Get;
    public static BO.Call DOtoBO(DO.Call call)
    {
        // שלב 1: שליפת כל ה-Assignments הרלוונטיים לאותו ID
        var assignments = s_dal.Assignment.ReadAll()
            .Where(assignment => assignment.CallId == call.Id)
            .ToList();

        // שלב 2: יצירת רשימה של BO.CallAssignInList
        var callAssignInList = assignments.Select(assignment => new BO.CallAssignInList
        {
            VolunteerId = assignment.VolunteerId,
            Name = assignment.Name,
            EntryTime = assignment.EntryTime,
            EndTime = assignment.EndTime,
            EndTimeType = assignment.EndTimeType
        }).ToList();
            return new BO.Call
        {
            Id = call.Id,
            callType = call.callType,
            Address = call.Address,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpeningTime = call.OpeningTime,
            description = call.Description,
            maxEndingTime = call.maxEndingTime,
            callStatus = Tools.Status(call.Id)
            CallAssignList= callAssignInList
            };
    }
}
