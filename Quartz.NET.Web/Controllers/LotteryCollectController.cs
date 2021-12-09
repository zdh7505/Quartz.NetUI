using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Quartz.NET.Web.Models.MysqlLottery;
using Quartz.NET.Web.Models.OtherModels;
using Quartz.NET.Web.Services;
using Quartz.NET.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Controllers
{

    [AllowAnonymous]
    public class LotteryCollectController : Controller
    {
        private readonly MysqlDbContext _context;
        readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor httpContextAccessor;
        public LotteryCollectController(MysqlDbContext context, IHttpClientFactory httpClientFactory, IHttpContextAccessor accessor, ILogger<HomeController> logger)
        {
            _context = context;
            this.httpClientFactory = httpClientFactory;
            _logger = logger;
            _logger.LogDebug(1, "初始化");
            httpContextAccessor = accessor;
        }
        //public async Task Ssq()
        //{
        //    //var res = HttpManager.GetHtmlSource2("https://www.zhcw.com/kjxx/");
        //    //var res = _context.LotterySsq.First();

        //    //string html = HttpManager.GetHtmlSource2("https://www.zhcw.com/kjxx/");

        //    //var htmlDoc = new HtmlDocument();
        //    //htmlDoc.LoadHtml(html);
        //    //var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

        //    //var url = "http://html-agility-pack.net/";
        //    //var web = new HtmlWeb();
        //    //var doc = web.Load(url);

        //    // With XPath 
        //    //var value = doc.DocumentNode.SelectNodes("//td/input").First().Attributes["value"].Value;
        //    //var postitemsNodes = post_listnode.SelectNodes("//div[@class='post_item']");

        //    // With LINQ 
        //    //var nodes = doc.DocumentNode.Descendants("input").Select(y => y.Descendants().Where(x => x.Attributes["class"].Value == "box")).ToList();

        //    //var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='kjbg']");


        //    //string httpMessage = "";
        //    //Dictionary<string, string> header = new Dictionary<string, string>();
        //    //header["Origin"] = "https://www.zhcw.com";
        //    //header["Referer"] = "https://www.zhcw.com/";

        //    //httpMessage = await httpClientFactory.HttpSendAsync(
        //    //    HttpMethod.Get,
        //    //    "https://jc.zhcw.com/port/client_json.php?transactionType=10001008&type=FC&tt=0.5139950695920534&_=1635780250928",
        //    //    header);

        //    //JObject jObj = JObject.Parse(httpMessage);
        //    //var t = jObj["lotteryResults"].ToString();

        //    //var lottery_list = JsonHelper.DeserializeJsonToList<LotteryCollect>(t);
        //    //foreach(LotteryCollect l in lottery_list)
        //    //{

        //    //}

        //    //LotterySsq ssq = new LotterySsq() { };
        //    //LotteryCollect ssq_collect = lottery_list[0];
        //    //ssq.issue = ssq_collect.issue;

        //    //var res = _context.LotterySsq.Find(ssq.issue);
        //    //if (res == null)
        //    //{
        //    //    ssq.open_date = ssq_collect.openTime;
        //    //    ssq.front_num = ssq_collect.frontWinningNum;
        //    //    ssq.back_num = ssq_collect.backWinningNum;
        //    //    ssq.save_time = DateTime.Now;
        //    //    _context.LotterySsq.Add(ssq);
        //    //    _context.SaveChanges();
        //    //}


        //    _logger.LogDebug(1, "发生了一个Bug2");
        //    return;
        //}
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllData()
        {
            //string url = "http://kaijiang.zhcw.com/zhcw/html/ssq/list.html";
            //Dictionary<string, string> header = new Dictionary<string, string>();
            //string httpMessage = await httpClientFactory.HttpSendAsync(HttpMethod.Get, url, header);

            //string html = HttpManager.GetHtmlSource2("http://kaijiang.zhcw.com/zhcw/html/ssq/list_1.html");
            //string html = HttpManager.GetHtmlSource2("http://kaijiang.zhcw.com/zhcw/inc/ssq/ssq_wqhg.jsp?pageNum=9999");

            int page_start = 20;
            int all_page_num = 9999;
            LotteryTypeCommonService<LotterySsq> cc = new LotteryTypeCommonService<LotterySsq>() { };
            for (; page_start <= all_page_num; page_start++)
            {
                string url = $"http://kaijiang.zhcw.com/zhcw/inc/ssq/ssq_wqhg.jsp?pageNum={page_start}";
                //string url = $"http://kaijiang.zhcw.com/zhcw/html/ssq/list_{page_start}.html";

                _logger.LogInformation($"url:{url}");
                string html = "";
                int load_times = 0;
                bool next_flag = false;
                do
                {
                    if (load_times > 200)
                    {
                        next_flag = true;
                        break;
                    }
                    try
                    {
                        html = HttpManager.GetHtmlSource2(url);
                        if (string.IsNullOrEmpty(html))
                        {
                            _logger.LogInformation("失败 空");
                            load_times++;
                            Thread.Sleep(3000);
                            continue;
                        }
                        else
                        {
                            _logger.LogInformation("成功");
                        }
                        
                        break;
                    }
                    catch (Exception)
                    {
                        _logger.LogInformation("失败 异常");
                        load_times++;
                        Thread.Sleep(3000);
                    }
                } while (true);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(Regex.Replace(html.Trim().Replace("\n", "").Replace("\r", ""), @">\s+<", "><"));
                var htmlBody = htmlDoc.DocumentNode.SelectNodes("//table[@class='wqhgt']/tr");

                if (all_page_num == 9999)
                {
                    //分析总页数
                    string temp_page_num = htmlBody[htmlBody.Count - 1].ChildNodes[0].InnerText;
                    string pattern = @"[\d]+";
                    all_page_num = Convert.ToInt32(Regex.Match(temp_page_num, pattern).Value);
                }
                if(next_flag == true)
                {
                    continue;
                }
                foreach (var a in htmlBody)
                {
                    //分析处理每一行
                    if (a.ChildNodes.Count != 7)
                    {
                        continue;
                    }
                    int ele_count = 0;
                    string data_date = "";
                    string data_issue = "";
                    string data_f = "";//front
                    string data_b = "";//back
                    foreach (var b in a.ChildNodes)
                    {
                        switch (ele_count)
                        {
                            case 0:
                                //日期
                                data_date = b.InnerText;
                                break;
                            case 1:
                                //期号
                                data_issue = b.InnerText;
                                break;
                            case 2:
                                //开奖数据分析
                                var data_fb = b.ChildNodes;
                                List<string> temp_ls_f = new List<string> { };
                                List<string> temp_ls_b = new List<string> { };
                                for (int i = 0; i < 6; i++)
                                {
                                    temp_ls_f.Add(data_fb[i].InnerText);
                                }
                                temp_ls_f.Sort((x, y) => { return String.Compare(x, y); });
                                temp_ls_b.Add(data_fb[6].InnerText);
                                data_f = string.Join(" ", temp_ls_f.ToArray());
                                data_b = string.Join("", temp_ls_b.ToArray());
                                break;
                            default:
                                break;
                        }
                        ele_count += 1;
                    }

                    if (!string.IsNullOrEmpty(data_date) && !string.IsNullOrEmpty(data_issue) && !string.IsNullOrEmpty(data_f) && !string.IsNullOrEmpty(data_b))
                    {
                        cc.UpdateLotteryTypeData(new LotterySsq { issue = data_issue, open_date = Convert.ToDateTime(data_date), save_time = Convert.ToDateTime(data_date), front_num = data_f, back_num = data_b, lottery_data = "" });
                    }
                }
                Thread.Sleep(10000);
            }
            return Json(new { status = true, msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
        }

        /// 返回网页源文件
        public async Task<IActionResult> GetNewSsq()
        {
            //"https://jc.zhcw.com/port/client_json.php?transactionType=10001008&type=FC&tt=0.5139950695920534&_=1635780250928",

            Random random = new Random();
            var tt = Math.Round(random.NextDouble(), 15);
            var ts = TimeHelper.Timestamp();
            string url = $"https://jc.zhcw.com/port/client_json.php?transactionType=10001008&type=FC&tt={tt}&_={ts}";

            string httpMessage = "";
            Dictionary<string, string> header = new Dictionary<string, string>();
            header["Origin"] = "https://www.zhcw.com";
            header["Referer"] = "https://www.zhcw.com/";

            httpMessage = await httpClientFactory.HttpSendAsync(HttpMethod.Get,url,header);

            JObject jObj = JObject.Parse(httpMessage);
            var t = jObj["lotteryResults"].ToString();
            var lottery_list = JsonHelper.DeserializeJsonToList<LotteryCollect>(t);

            LotterySsq ssq = new LotterySsq() { };
            LotteryCollect ssq_collect = lottery_list[0];
            ssq.issue = ssq_collect.issue;

            //var res = _context.LotterySsq.Find(ssq.issue);
            //if (res == null)
            //{
            ssq.open_date = ssq_collect.openTime;
            ssq.front_num = ssq_collect.frontWinningNum;
            ssq.back_num = ssq_collect.backWinningNum;
            ssq.save_time = DateTime.Now;
            //    _context.LotterySsq.Add(ssq);
            //    _context.SaveChanges();
            //}

            LotteryDataService s = new LotteryDataService();
            s.UpdateNewIssueData("ssq", ssq);

            _logger.LogTrace("记录LogTrace");
            _logger.LogDebug("记录LogDebug");
            _logger.LogInformation("记录LogInformation");
            return Json(new { status = true, msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")});
        }
    }
}
