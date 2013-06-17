using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDtMM.KingOfPing
{
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">DT whose TimeOfDay we are trying to determine if is 
        /// between start and end time</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static bool TimeInRange(DateTime time, DateTime startTime, DateTime endTime)
        {
            return TimeInRange(time.TimeOfDay, startTime.TimeOfDay, endTime.TimeOfDay);
        }

        public static bool TimeInRange(TimeSpan time, TimeSpan start, TimeSpan end)
        {
            if (start < end)
            {
                return (time >= start) && (time <= end);
            }
            else if (start > end)
            {
                return (time >= start) || (time <= end);
            }
            else
            {
                return (time == start);
            }
        }

        /// <summary>
        /// Gets the next Date and Time relative to the time of day.  So if time's TimeOfDay
        /// &gt; Now's TimeOfDay then it would be later today, otherwise tommorow.
        /// </summary>
        /// <param name="timeOfDay">A DateTime object meant only for time of day.</param>
        public static DateTime NextTimeDate(TimeSpan timeOfDay)
        {
            DateTime now = DateTime.Now;
            DateTime nextDT = now.Date + timeOfDay;
            if (timeOfDay < now.TimeOfDay) nextDT = nextDT.AddDays(1);
            return nextDT;
        }
    }
}