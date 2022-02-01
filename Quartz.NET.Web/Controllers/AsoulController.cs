using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Quartz.NET.Web.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Controllers
{
    public class AsoulController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public AsoulController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        protected string GetPath()
        {
            var path = Directory.GetCurrentDirectory();//获取当前的项目文件所在的目录。当使用命令启动时为执行dotnet命令所在的目录
            dynamic type = (new Program()).GetType();
            string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);//打包后地址
            string dataDirectory = currentDirectory + Path.DirectorySeparatorChar + "asoul";
            return dataDirectory;
        }

        protected string GetFilename(string filename,string id)
        {
            return $"{filename}-{id}.txt";
        }
        public IActionResult GetUperList(string offset="")
        {
            string filename = "jiaran";
            string url;

            while (true)
            {
                if (string.IsNullOrEmpty(offset))
                {
                    url = $"https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/space_history?visitor_uid=0&host_uid=672328094&offset_dynamic_id=0&need_top=1&platform=web";
                }
                else
                {
                    url = $"https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/space_history?visitor_uid=0&host_uid=672328094&offset_dynamic_id={offset}&need_top=1&platform=web";
                }
                _logger.LogInformation($"url:{url}");
                string html = HttpManager.GetHtmlSource2(url);

                if (string.IsNullOrEmpty(html))
                {
                    _logger.LogInformation("失败 空");
                    break;
                }
                else
                {
                    _logger.LogInformation("成功");
                    FileHelper.WriteFile(GetPath(), GetFilename(filename, offset), html);
                    JObject jObj = JObject.Parse(html);
                    offset = jObj["data"]["next_offset"].ToString();
                    if (!string.IsNullOrEmpty(offset) && offset != "0")
                    {
                        Thread.Sleep(200);
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return Ok(new{title=1 });
        }


        public IActionResult GetFileString()
        {
            string filename = "jiaran";
            string begin = "0";
            string datafilename = GetPath() + Path.DirectorySeparatorChar + GetFilename(filename, begin);
            JObject jObj = JObject.Parse(FileHelper.ReadFile(datafilename));
            var next_offset = jObj["data"]["next_offset"];
            return Ok(new { title = 1 });
        }

        public IActionResult MyThread3()
        {
            return Ok(new { title = 1 });
        }


        public async Task<string> MyThread1()
        {
            var info = string.Format("api执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var infoTask = TaskCaller().Result;//使用Result

            var infoTaskFinished = string.Format("api执行线程（task调用完成后）:{0}", Thread.CurrentThread.ManagedThreadId);
            return string.Format("{0},{1},{2}", info, infoTask, infoTaskFinished);
        }

        public async Task<string> MyThread2()
        {
            var info = string.Format("api执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var infoTask = TaskCaller().Result;//使用Result

            var infoTaskFinished = string.Format("api执行线程（task调用完成后）:{0}", Thread.CurrentThread.ManagedThreadId);
            return string.Format("{0},{1},{2}", info, infoTask, infoTaskFinished);
        }
        private async Task<string> TaskCaller(int i=0)
        {
            await Task.Delay(5000);
            _logger.LogInformation($"TaskCaller thread:{i};Thread.CurrentThread.ManagedThreadId:" + Thread.CurrentThread.ManagedThreadId + DateTime.Now.ToString());
            return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        public async Task<string> MyThread4()
        {
            string res = "";
            for (int i = 0; i < 3; i++)
            {
                _logger.LogInformation($"thread:{i};" + DateTime.Now.ToString());
                res += $"{i}";
                res += TaskCaller(i);
            }

            return string.Format(res);
        }
        public async Task<string> MyThread5()
        {
            string res = "";
            Task[] tasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                _logger.LogInformation($"MyThread5 thread:{i};" + DateTime.Now.ToString());
                res += $"{i}";
                res += TaskCaller(i);

                tasks[i] = Task.Factory.StartNew(() => TaskCaller(i));
            }

            Task.WaitAll(tasks);
            for (int i = 0; i < 5; i++)
            {
                res += $"res:{tasks[i]}";
            }

            return string.Format(res);
        }
        public string MyThread6()
        {
            string res = "";
            Task[] tasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                _logger.LogInformation($"MyThread6 thread:{i};" + DateTime.Now.ToString());
                tasks[i] = TaskCaller(i);
            }

            Task.WaitAll(tasks.ToArray());
            return string.Format("finish");
        }
    }
}
