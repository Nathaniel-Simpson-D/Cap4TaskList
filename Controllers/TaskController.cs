using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cap4TaskList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cap4TaskList.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly Cap4DbContext _context;

        public TaskController(Cap4DbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("ListTasks");
        }
        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTask(UserTask newTask)
        {
            newTask.Complete = false;
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            newTask.UserId = thisUser.Id;
            newTask.User = thisUser;

            if(newTask.DueDate == null || newTask.DueDate.ToString() == "")
            {
                newTask.DueDate = DateTime.Now.AddDays(14);
            }

            if(ModelState.IsValid)
            {
                _context.UserTask.Add(newTask);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(newTask);
            }
           
        }
        public IActionResult ListTasks()
        {
            List<UserTask> dBTasks = _context.UserTask.ToList();
            List<UserTask> tasks = new List<UserTask>();
            foreach (var task in dBTasks)
            {
                tasks.Add(task);
            }
            return View(tasks);
        }
        public IActionResult MarkComplete(int Id)
        {
            foreach (var task in _context.UserTask.ToList())
            {
                if (task.TaskId == Id)
                {
                    task.Complete = true;
                    _context.UserTask.Update(task);
                }
                    
            }
            _context.SaveChanges();
            return RedirectToAction("ListTasks");
        }
        public IActionResult RemoveTask(int Id)
        {
            foreach (var task in _context.UserTask.ToList())
            {
                if (task.TaskId == Id)
                {
                    _context.UserTask.Remove(task);
                }

            }
            _context.SaveChanges();
            return RedirectToAction("ListTasks");
        }


    }
}