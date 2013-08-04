using System;
using System.Collections.Generic;

namespace Grain.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts a Flagged Enum to a collection of int values for each flag that the input parameter has set.
        /// 0 (default) is only returned if it is the only flag that is set.
        /// </summary>
        /// <param name="input">Enum: the enum with flags to export</param>
        /// <returns>A collection of integers that represent the flags that are set on the given enum</returns>        
        public static IEnumerable<int> AsEnumerableInts(this Enum input)
        {
            return input.AsEnumerableInts(true);
        }

        /// <summary>
        /// Converts a Flagged Enum to a collection of int values for each flag that the input parameter has set.
        /// </summary>
        /// <param name="input">Enum: the enum with flags to export</param>
        /// <param name="omitZeroFlag">This method will return 0 in addition to any flags that are set, unless omitZeroFlag is true.</param>
        /// <returns>A collection of integers that represent the flags that are set on the given enum</returns>
        public static IEnumerable<int> AsEnumerableInts(this Enum input, bool omitZeroFlag)
        {
            if (input.GetTypeCode().ToString() != "Int32")
                throw new NotImplementedException("AsEnumerable does not support Enums with an underlying type code other than Int32.");

            // If the value is default and only default, return the input
            if (((int)(object)input) == 0)
            {
                yield return ((int)(object)input);
                yield break;
            }

            foreach (Enum val in Enum.GetValues(input.GetType()))
            {
                if (omitZeroFlag)
                {
                    var _int = ((int)(object)val);
                    if (input.HasFlag(val) && _int != 0)
                        yield return _int;
                }
                else
                {
                    if (input.HasFlag(val))
                        yield return ((int)(object)val);
                }
            }
        }

        /// <summary>
        /// Converts a Flagged Enum to a collection of string values for each flag that the input parameter has set.
        /// 0 (default) is only returned if it is the only flag that is set.
        /// </summary>
        /// <typeparam name="T">The Enum type is required to be able to get the name</typeparam>
        /// <param name="input">Enum: the enum with flags to export</param>
        /// <returns>A collection of enums that represent the flags that are set on the given enum</returns>  
        public static IEnumerable<string> AsEnumerableNames<T>(this Enum input)
        {
            if (((int)(object)input) == 0)
            {
                yield return Enum.GetName(typeof(T), input);
                yield break;
            }

            var _output = input.ToString().ToList(',');
            foreach (var item in _output)
            {
                yield return item.Trim();
            }
        }

        /// <summary>
        /// Converts a Flagged Enum to a collection of Enum values for each flag that the input parameter has set.
        /// 0 (default) is only returned if it is the only flag that is set.
        /// </summary>
        /// <param name="input">Enum: the enum with flags to export</param>
        /// <returns>A collection of enums that represent the flags that are set on the given enum</returns>  
        public static IEnumerable<Enum> AsEnumerable(this Enum input)
        {
            return input.AsEnumerable(true);
        }

        /// <summary>
        /// Converts a Flagged Enum to a collection of Enum values for each flag that the input parameter has set.
        /// 0 (default) is only returned if it is the only flag that is set.
        /// </summary>
        /// <param name="input">Enum: the enum with flags to export</param>
        /// <param name="omitZeroFlag">This method will return 0 in addition to any flags that are set, unless omitZeroFlag is true.</param>
        /// <returns>A collection of enums that represent the flags that are set on the given enum</returns>  
        public static IEnumerable<Enum> AsEnumerable(this Enum input, bool omitZeroFlag)
        {
            if (input.GetTypeCode().ToString() != "Int32")
                throw new NotImplementedException("AsEnumerable does not support Enums with an underlying type code other than Int32.");

            // If the value is default and only default, return the input
            if (((int)(object)input) == 0)
            {
                yield return input;
                yield break;
            }

            foreach (Enum val in Enum.GetValues(input.GetType()))
            {
                if (omitZeroFlag)
                {
                    //var _int = (int)Convert.ChangeType(val, input.GetTypeCode()); // another means of casting the enum to an int
                    var _int = ((int)(object)val);
                    if (input.HasFlag(val) && _int != 0)
                        yield return val;
                }
                else
                {
                    if (input.HasFlag(val))
                        yield return val;
                }
            }
        }

        /// <summary>
        /// Compares the int
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public static bool EnumsAreEqual<T1, T2>()
        {
            if (Enum.GetUnderlyingType(typeof(T1)) != typeof(int))
                throw new NotImplementedException("AsEnumerable does not support Enums with an underlying type code other than Int32.");

            if (Enum.GetUnderlyingType(typeof(T2)) != typeof(int))
                throw new NotImplementedException("AsEnumerable does not support Enums with an underlying type code other than Int32.");

            // Enum.GetNames and Enum.GetValues return arrays sorted by value.
            var xValues = Enum.GetValues(typeof(T1));
            var yValues = Enum.GetValues(typeof(T2));
            bool _intsAreEqual = true;
            bool _namesAreEqual = true;

            for (int i = 0; i < xValues.Length; i++)
            {
                var xValue = xValues.GetValue(i);
                var yValue = yValues.GetValue(i);

                _intsAreEqual = _intsAreEqual && (int)xValue == (int)yValue;                                            // make sure the ints are the same
                _namesAreEqual = _namesAreEqual && Enum.GetName(typeof(T1), xValue) == Enum.GetName(typeof(T2), yValue);// make sure the strings are the same   
            }

            return xValues.Length == yValues.Length && _intsAreEqual && _namesAreEqual;
        }
    }
}
