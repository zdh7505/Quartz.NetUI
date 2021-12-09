using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Models.MysqlLottery
{
    public class MysqlDbContext : DbContext
    {
        //public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=42.192.93.119;user id=root;password=zdh888999;database=lottery;charset=utf8;sslMode=None");
            //optionsBuilder.UseMySql("server=mysql1;user id=root;password=123456;database=lottery;charset=utf8;sslMode=None");
        }
        public DbSet<LotterySsq> LotterySsq { get; set; }
        public DbSet<LotteryDlt> LotteryDlt { get; set; }
        public DbSet<LotteryData> LotteryData { get; set; }

    }
}
