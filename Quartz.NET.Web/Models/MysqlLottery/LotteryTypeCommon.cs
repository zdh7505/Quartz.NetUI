using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Models.MysqlLottery
{
    public class LotteryTypeCommon
    {
        [Key]
        public string issue { get; set; }
        public string lottery_data { get; set; }
        public string front_num { get; set; }
        public string back_num { get; set; }
        public DateTime save_time { get; set; }
        public DateTime open_date { get; set; }
    }
}
