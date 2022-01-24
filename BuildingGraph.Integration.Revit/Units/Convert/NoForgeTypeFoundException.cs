using System;

namespace BuildingGraph.Integration.Revit
{

    public class NoForgeTypeFoundException : Exception
    {
        public NoForgeTypeFoundException()
        {
        }

        public NoForgeTypeFoundException(string message)
            : base(message)
        {
        }

        public NoForgeTypeFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
