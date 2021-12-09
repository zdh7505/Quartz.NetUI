using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Models.OtherModels
{
    public class LotteryCollect
    {
        /*
         "lotteryId": "1",
            "lotteryName": "双色球",
            "issue": "2021124",
            "openTime": "2021-10-31",
            "week": "日",
            "frontWinningNum": "01 03 18 22 29 32",
            "backWinningNum": "02",
            "saleMoney": "394848530"
         */

        public string lotteryId { get; set; }
        public string lotteryName { get; set; }
        public string issue { get; set; }
        public DateTime openTime { get; set; }
        public string week { get; set; }
        public string frontWinningNum { get; set; }
        public string backWinningNum { get; set; }
        public string saleMoney { get; set; }
    }
}
