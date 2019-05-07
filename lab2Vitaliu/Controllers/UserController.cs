using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lab2Vitaliu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab2Vitaliu.Controllers
{
    [Route("user")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {

        private MyContext ApplicationContext;

        public UserController(MyContext applicationContext)
        {
            this.ApplicationContext = applicationContext;
        }

        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            return View(ApplicationContext.users.Include(u => u.Role));
        }

        // GET: User/Create
        [Route("add")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public ActionResult Create(User user)
        {
            ApplicationContext.users.Add(user);
            ApplicationContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: User/Edit/5
        [Route("update/{id}")]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            User user = ApplicationContext.users.Where(u => u.Id == id).First();
            return View(user);
        }

        // POST: User/Edit/5
        [Route("update/{id}")]
        [HttpPost]
        public ActionResult Edit(User user)
        {
            foreach (Role role in ApplicationContext.roles)
            {
                if (role.Id == user.RoleId)
                {
                    user.Role = role;
                    break;
                }
            }
            User oldUser = ApplicationContext.users.Where(u => u.Id == user.Id).First();
            oldUser.Email = user.Email;
            ApplicationContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: User/Delete/5
        [Route("delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            User user = ApplicationContext.users.Where(u => u.Id == id).First();
            ApplicationContext.users.Remove(user);
            ApplicationContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}