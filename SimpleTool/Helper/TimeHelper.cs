using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSqlSugar
{
    /// <summary>
    /// 时间戳辅助类
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// 获取当前的时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetNowTimeStamp()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            return (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;
        }

        /// <summary>
        /// 获取当前毫秒时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetNowTimeStampMill()
        {
            DateTime dt1970 = new DateTime(1970, 1, 1);
            DateTime current = DateTime.Now;
            return (long)(current - dt1970).TotalMilliseconds;
        }

        /// <summary>
        /// 将c# Unix时间戳格式转换为DateTime时间格式
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <param name="isMinSeconds"></param>
        /// <returns></returns>
        public static DateTime StampToDatetime(this long TimeStamp, bool isMinSeconds = false)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));//当地时区
            //返回转换后的日期
            if (isMinSeconds)
                return startTime.AddMilliseconds(TimeStamp);
            else
                return startTime.AddSeconds(TimeStamp);
        }

        /// <summary>
        /// 获取当前年的时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetYearTimeStamp()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            DateTime yearStamp = new DateTime(DateTime.Now.Year, 1, 1);
            return (yearStamp.Ticks - timeStamp.Ticks) / 10000000;
        }

        /// <summary>
        /// 获取某一年的时间戳
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public static long GetSomeYearTimeStamp(int Year)
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            DateTime yearStamp = new DateTime(Year, 1, 1);
            return (yearStamp.Ticks - timeStamp.Ticks) / 10000000;
        }

        /// <summary>
        /// 获取当前天的时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetDayStartTimeStamp()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            DateTime yearStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            return (yearStamp.Ticks - timeStamp.Ticks) / 10000000;
        }

        /// <summary>  
        /// 时间戳转为C#格式时间  
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>C#格式时间</returns>  
        public static DateTime GetDateTimeFromUnix(this long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timeStamp * 10000000);
            return dtStart.Add(toNow);
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToLong(this DateTime time)
        {
#pragma warning disable CS0618 // 类型或成员已过时
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
#pragma warning restore CS0618 // 类型或成员已过时
            return (long)(time - startTime).TotalSeconds; // 相差秒数

        }

        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string GetDateDiff(this DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = string.Empty;

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff += ts.Days > 0 ? ts.Days.ToString() + "天" : string.Empty;
            dateDiff += ts.Hours > 0 ? ts.Hours.ToString() + "小时" : string.Empty;
            dateDiff += ts.Minutes > 0 ? ts.Minutes.ToString() + "分钟" : string.Empty;
            dateDiff += ts.Seconds > 0 ? ts.Seconds.ToString() + "秒" : string.Empty;
            return dateDiff;
        }

    }
}
