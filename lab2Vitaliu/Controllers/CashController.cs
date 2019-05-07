using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lab2Vitaliu.Filters;
using lab2Vitaliu.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace lab2Vitaliu.Controllers
{
    [Route("cashes")]
    [ServiceFilter(typeof(LoggerFilter))]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class CashController : Controller
    {
        MyContext MyContext;
        private IMemoryCache MemoryCache;
        public CashController(MyContext MyContext, IMemoryCache memoryCache)
        {
            this.MyContext = MyContext;
            this.MemoryCache = memoryCache;
        }

        [ResponseCache(Duration = 2 * 11 + 240, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            object obj;
            if (!MemoryCache.TryGetValue("cash", out obj))
            {
                List<Cash> cash = MyContext.cashes.ToList();
                if (cash != null)
                {
                    MemoryCache.Remove("cash");
                    MemoryCache.Set("cash", cash, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 11 + 240)));
                }
                return View(MemoryCache.Get("cash"));
            }
            else
            {
                return View(MemoryCache.Get("cash"));
            }
        }

        [HttpDelete, Route("/cashes/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Cash cash = MyContext.cashes.First(t => t.Id == id);
            MyContext.cashes.Remove(cash);

            int n = MyContext.SaveChanges();
            if (n > 0)
            {
                MemoryCache.Remove(cash);
                MemoryCache.Set("cash", cash,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 11 + 240)));
            }
            return View();
        }
        [HttpGet, Route("Edit")]
        public IActionResult EditPage(int id)
        {
            ViewBag.cashes = MyContext.cashes.ToList();
            Cash cash = MyContext.cashes.First(t => t.Id == id);
            return View(cash);
        }
        [HttpPost, Route("Edit"), ActionName("EditPage")]
        public IActionResult Edit(Cash cashCol)
        {
            foreach (Cash cash in MyContext.cashes)
            {
                if (cash.Id == cashCol.Id)
                {
                    cash.Name = cashCol.Name;
                    break;
                }
            }
            MyContext.SaveChanges();
            return Redirect("/cashes");
        }
        [HttpGet, Route("Details")]
        public IActionResult DetailsPage(int id)
        {
            Cash cash = MyContext.cashes.First(t => t.Id == id);
            return View(cash);
        }
        [HttpGet, Route("Add")]
        public IActionResult AddPage()
        {
            ViewBag.cashes = MyContext.cashes.ToList();
            if (HttpContext.Request.Cookies.TryGetValue("cash", out var content))
            {
                Cash cash = JsonConvert.DeserializeObject<Cash>(content);
                return View(cash);
            }
            return View();
        }
        [HttpPost, Route("Add"), ActionName("AddPage")]
        public IActionResult Add(Cash cash)
        {
            MyContext.cashes.Add(cash);
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(60),
                IsEssential = true
            };
            HttpContext.Response.Cookies.Append("cash", JsonConvert.SerializeObject(cash), options);

            int n = MyContext.SaveChanges();
            if (n > 0)
            {
                MemoryCache.Set(cash.Id, cash, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2 * 11 + 240)
                });
            }
            return Redirect("/cashes");
        }
    }
}