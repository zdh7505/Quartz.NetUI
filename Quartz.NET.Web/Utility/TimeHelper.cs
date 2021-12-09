using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Utility
{
    public static class TimeHelper
    {  /// <summary>
       /// 获取当前的时间戳
       /// </summary>
       /// <returns></returns>
        public static string Timestamp()
        {
            long ts = ConvertDateTimeToInt(DateTime.Now);
            return ts.ToString();
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            long t = (time.Ticks - 621356256000000000) / 10000;
            return t;
        }
    }
}
