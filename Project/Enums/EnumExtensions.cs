using System;
using System.Collections.Generic;

namespace MatrixMultipling.Project.Enums
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return (IEnumerable<T>)Enum.GetValues(typeof(T));
        }
    }
}