using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grain.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Creates an IEnumberable object that can be used to iterate of each day between two dates
        /// </summary>
        /// <param name="from">DateTime: the start date</param>
        /// <param name="thru">DateTime: then end date</param>
        /// <returns>IEnumberable: of type DateTime</returns>
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        /// <summary>
        /// Returns true if the dateTime is before now.
        /// </summary>
        /// <param name="dateTime">The DateTime to compare to now</param>
        /// <returns>Returns true if the dateTime is before now.</returns>
        public static bool IsInThePast(this DateTime dateTime) 
        {
            return dateTime < DateTime.Now;
        }

        /// <summary>
        /// Returns true if the dateTime is atleast the amount of the timeSpan before now
        /// </summary>
        /// <param name="dateTime">The DateTime to compare to now</param>
        /// <param name="timeSpan">the duration of time before now that is being validated</param>
        /// <returns>Returns true if the dateTime is atleast the amount of the timeSpan before now</returns>
        public static bool IsInThePast(this DateTime dateTime, TimeSpan timeSpan)
        {
            return dateTime < (DateTime.Now - timeSpan);
        }

        /// <summary>
        /// Returns true if the dateTime is after now.
        /// </summary>
        /// <param name="dateTime">The DateTime to compare to now</param>
        /// <returns>Returns true if the dateTime is after now.</returns>
        public static bool IsInTheFuture(this DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }

        /// <summary>
        /// Returns true if the dateTime is atleast the amount of the timeSpan after now
        /// </summary>
        /// <param name="dateTime">The DateTime to compare to now</param>
        /// <param name="timeSpan">the duration of time after now that is being validated</param>
        /// <returns>Returns true if the dateTime is atleast the amount of the timeSpan after now</returns>
        public static bool IsInTheFuture(this DateTime dateTime, TimeSpan timeSpan)
        {
            return dateTime > (DateTime.Now + timeSpan);
        }
    }
}
