using System;
using System.Linq;

namespace EightBot.MonoDroid.CheatCode
{
    public static class ArrayExtensions
    {
        public static Boolean EndSequenceEqual<T>(this T[] first, T[] second)
        {
            return first.SequenceEqual(second.Skip(second.Length - first.Length));
        }
    }
}