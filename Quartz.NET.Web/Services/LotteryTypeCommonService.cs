using Quartz.NET.Web.Models.MysqlLottery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Services
{
    public class LotteryTypeCommonService<T> where T:LotteryTypeCommon
    {
        public void UpdateLotteryTypeData(T l)
        {
            using (var _context = new MysqlDbContext())
            {
                var tb = _context.Set<T>();
                T res = tb.Find(l.issue);
                Console.WriteLine(res);
                if (res == null)
                {
                    tb.Add(l);
                }
                else
                {
                    res.open_date = l.open_date;
                    res.save_time = l.save_time;
                    res.lottery_data = l.lottery_data;
                    res.issue = l.issue;
                    res.front_num = l.front_num;
                    res.back_num = l.back_num;
                }
                _context.SaveChanges();
            }

        }
    }
}
