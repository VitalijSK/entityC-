using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lab2Vitaliu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using lab2Vitaliu.Filters;
using Microsoft.AspNetCore.Authorization;

namespace lab2Vitaliu.Controllers
{
    [Route("deposits")]
    [ServiceFilter(typeof(LoggerFilter))]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    [Authorize(Roles = "admin, user")]
    public class DepositController : Controller
    {       
        MyContext MyContext;
        private IMemoryCache MemoryCache;
        public DepositController(MyContext MyContext, IMemoryCache memoryCache)
        {
            this.MyContext = MyContext;
            this.MemoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet, Route("/AllResult")]
        [ResponseCache(Duration = 2 * 11 + 240, Location = ResponseCacheLocation.Any)]
        public IActionResult AllResult()
        {
            ViewBag.cashes = MyContext.cashes.ToList();
            object obj;
            if (!MemoryCache.TryGetValue("deposit", out obj))
            {
                List<Deposit> deposits = MyContext.deposits.ToList();
                if (deposits != null)
                {
                    MemoryCache.Remove("deposit");
                    MemoryCache.Set("deposit", deposits, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 11 + 240)));
                }
                return View(MemoryCache.Get("deposit"));
            }
            else
            {
                return View(MemoryCache.Get("deposit"));
            }
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Deposit deposit = MyContext.deposits.First(t => t.Id == id);
            MyContext.deposits.Remove(deposit);

            int n = MyContext.SaveChanges();
            if (n > 0)
            {
                MemoryCache.Remove(deposit);
                MemoryCache.Set("deposit", deposit,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 11 + 240)));
            }
            return View();
        }
        [HttpGet, Route("/Edit")]
        public IActionResult EditPage(int id)
        {
            ViewBag.cashes = MyContext.cashes.ToList();
            Deposit deposit = MyContext.deposits.First(t => t.Id == id);
            return View(deposit);
        }
        [HttpPost, Route("/Edit"), ActionName("EditPage")]
        public IActionResult Edit(Deposit deposit)
        {
            foreach (Deposit depositCol in MyContext.deposits)
            {
                if (depositCol.Id == deposit.Id)
                {
                    depositCol.IdCash = deposit.IdCash;
                    depositCol.MinMoney = deposit.MinMoney;
                    depositCol.MinTime = deposit.MinTime;
                    depositCol.Name = deposit.Name;
                    depositCol.Prosent = deposit.Prosent;
                    break;
                }
            }
            MyContext.SaveChanges();
            return Redirect("/AllResult");
        }
        [HttpGet, Route("/Details")]
        public IActionResult DetailsPage(int id)
        {
            ViewBag.cashes = MyContext.cashes.ToList();
            Deposit deposit = MyContext.deposits.First(t => t.Id == id);
            return View(deposit);
        }
        [HttpGet, Route("/Add")]
        public IActionResult AddPage()
        {
            ViewBag.cashes = MyContext.cashes.ToList();
            if (HttpContext.Request.Cookies.TryGetValue("deposit", out var content))
            {
                Deposit deposit = JsonConvert.DeserializeObject<Deposit>(content);
                return View(deposit);
            }
            return View();
        }
        [HttpPost, Route("/Add"), ActionName("AddPage")]
        public IActionResult Add(Deposit deposit)
        {
            MyContext.deposits.Add(deposit);
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(60),
                IsEssential = true
            };
            HttpContext.Response.Cookies.Append("deposit", JsonConvert.SerializeObject(deposit), options);

            int n = MyContext.SaveChanges();
            if (n > 0)
            {
                MemoryCache.Set(deposit.Id, deposit, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2 * 11 + 240)
                });
            }
            return Redirect("/AllResult");
        }

        [Route("sort/{sort}")]
        [HttpGet]
        public ActionResult sort(string sort)
        {
            List<Deposit> deposits = MyContext.deposits.ToList();

            switch (sort)
            {
                case "Name":
                    {
                        deposits.Sort(delegate (Deposit x, Deposit y)
                        {
                            return x.Name.CompareTo(y.Name);
                        });
                        break;
                    }
                case "MinTime":
                    {
                        deposits.Sort(delegate (Deposit x, Deposit y)
                        {
                            return x.MinTime.CompareTo(y.MinTime);
                        });
                        break;
                    }
                case "Prosent":
                    {
                        deposits.Sort(delegate (Deposit x, Deposit y)
                        {
                            return x.Prosent.CompareTo(y.Prosent);
                        });
                        break;
                    }
                case "IdCash":
                    {
                        deposits.Sort(delegate (Deposit x, Deposit y)
                        {
                            List<Cash> cashes = MyContext.cashes.ToList();
                            Cash cashX = cashes.Find(cash => cash.Id == x.IdCash);
                            Cash cashY = cashes.Find(cash => cash.Id == y.IdCash);
                            return cashX.Name.CompareTo(cashY.Name);
                        });
                        break;
                    }
                case "MinMoney":
                    {
                        deposits.Sort(delegate (Deposit x, Deposit y)
                        {
                            return x.MinMoney.CompareTo(y.MinMoney);
                        });
                        break;
                    }
            }
            ViewBag.cashes = MyContext.cashes.ToList();
            return View("AllResult", deposits);
        }

        [Route("filter")]
        [HttpGet]
        public ActionResult filter(string NameDeposit)
        {
            List<Deposit> deposits = MyContext.deposits.ToList();
            if (NameDeposit != null && !NameDeposit.Equals(""))
            {
                deposits = deposits.Where(r => r.Name.Contains(NameDeposit)).ToList();
            }
            ViewBag.cashes = MyContext.cashes.ToList();
            return View("AllResult", deposits);
        }

        [Route("partial")]
        public ActionResult Partial()
        {
            ViewBag.Message = "Это частичное представление.";
            return PartialView();
        }
    }
}