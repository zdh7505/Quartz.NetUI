using Quartz.NET.Web.Models.MysqlLottery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Services
{
    public class LotteryDataService
    {
        //private readonly MysqlDbContext _context;
        //public LotteryDataService(MysqlDbContext context)
        //{
        //    _context = context;
        //}

        public bool InitLotteryData(string lottery_type)
        {
            using(var _context = new MysqlDbContext())
            {
                LotteryData lottery = new LotteryData() { type = lottery_type, issue_last = "0", issue_now = "0" };
                _context.LotteryData.Add(lottery);
                int res = _context.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateNewIssueData(string lottery_type,LotterySsq lottery)
        {
            using (var _context = new MysqlDbContext())
            {
                LotteryData ld = _context.LotteryData.Find(lottery_type);

                if (ld == null)
                {
                    //初始化
                    InitLotteryData(lottery_type);
                    ld = _context.LotteryData.Find(lottery_type);
                }

                if (Convert.ToInt32(ld.issue_now) < Convert.ToInt32(lottery.issue))
                {
                    //更新
                    ld.issue_last = ld.issue_now;
                    ld.issue_now = lottery.issue;
                    _context.LotteryData.Update(ld);
                    _context.LotterySsq.Add(lottery);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
