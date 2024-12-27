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

    }
}
