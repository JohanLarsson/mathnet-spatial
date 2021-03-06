﻿namespace MathNet.Spatial.Units
{
    using System;
    using System.Text.RegularExpressions;

    public static class UnitParser
    {
        public static readonly string UnitValuePattern = string.Format(@"^(?: *)(?<Value>{0}) *(?<Unit>.+) *$", Parser.DoublePattern);

        public static T Parse<T>(string s, Func<double, IAngleUnit, T> creator)
        {
            Match match = Regex.Match(s, UnitValuePattern);
            double d = Parser.ParseDouble(match.Groups["Value"]);
            var unit = ParseUnit(match.Groups["Unit"].Value);
            return creator(d, (IAngleUnit)unit);
        }

        public static object ParseUnit(string s)
        {
            var trim = s.Trim();
            switch (trim)
            {
                case Degrees.Name:
                    return AngleUnit.Degrees;
                case Radians.Name:
                    return AngleUnit.Radians;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
