using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLApps.Revit.Parameters
{
    public interface IHLPeramiter
    {

        string Name { get; }

        object Value { get; set; }

        Type ExpectedType { get; }
    }
}
