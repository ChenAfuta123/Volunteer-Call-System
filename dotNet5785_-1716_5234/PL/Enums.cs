using System;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    internal class CallsCollection : IEnumerable
    {
        static readonly IEnumerable<BO.CallInListField> s_enums =
            (Enum.GetValues(typeof(BO.CallInListField)) as IEnumerable<BO.CallInListField>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class VolunteersCollection : IEnumerable
    {
        static readonly IEnumerable<BO.VolunteerInListFields> s_enums =
            (Enum.GetValues(typeof(BO.VolunteerInListFields)) as IEnumerable<BO.VolunteerInListFields>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class DistanceTypeCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum DistanceType
        static readonly IEnumerable<Enums.DistanceType> s_enums =
            Enum.GetValues(typeof(Enums.DistanceType)).Cast<Enums.DistanceType>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class RoleCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum Role
        static readonly IEnumerable<Enums.Role> s_enums =
            Enum.GetValues(typeof(Enums.Role)).Cast<Enums.Role>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class IsActiveFilterCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum Role
        static readonly IEnumerable<BO.IsActiveFilter> s_enums =
            Enum.GetValues(typeof(BO.IsActiveFilter)).Cast<BO.IsActiveFilter>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class ClosedCallInListFieldCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum Role
        static readonly IEnumerable<BO.ClosedCallInListField> s_enums =
            Enum.GetValues(typeof(BO.ClosedCallInListField)).Cast<BO.ClosedCallInListField>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class OpenCallsCollection : IEnumerable
    {
        static readonly IEnumerable<BO.OpenCallInListField> s_enums =
            (Enum.GetValues(typeof(BO.OpenCallInListField)) as IEnumerable<BO.OpenCallInListField>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    public class Enums
    {

        public enum Role
        {
            volunteer,
            manager
        }
        public enum DistanceType
        {
            AirDistance,
            WalkingDistance,
            DrivingDistance
        }
        public enum ClosedCallInListField
        {
            Id,
            CallType,
            Address,
            OpeningTime,
            EntryTime,
            EndTime,
            EndTimeType
        }

    }
}