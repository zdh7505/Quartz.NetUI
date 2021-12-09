using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Models.MysqlLottery
{
    public class LotteryData
    {
        [Key]
        public string type { get; set; }
        public string issue_last { get; set; }
        public string issue_now { get; set; }
    }
}
