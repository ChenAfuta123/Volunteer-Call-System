using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;


internal class CallsCollection : IEnumerable
{
    static readonly IEnumerable<BO.CallInListField> s_enums =
    (Enum.GetValues(typeof(BO.CallInListField)) as IEnumerable<BO.CallInListField>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

class Enums
{
  
}

