using System;

namespace PrestoSharp
{
    public class PrestoHelper
    {
        public static Type MapType(string Type)
        {
            return Type.GetType();
        }
    }
}
