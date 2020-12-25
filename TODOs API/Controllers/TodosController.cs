using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TODOs_API.Models;
using TODOs_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TODOs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly TodoAPIcontext _db;

        public TodosController(TodoAPIcontext db)
        {
            _db = db;
        }

        //GET: api/Todos/GetTasks
        [HttpGet("GetTasks")]
        public ActionResult<List<Task>> GetTasks([FromQuery(Name = "dateSort")] string dateSort, [FromQuery(Name = "sortBy")] string sortBy)
        {
            List<Task> tasks = _db.Tasks.ToList();
            if (!string.IsNullOrEmpty(dateSort))
            {
                if (dateSort.Equals("CreatedAt"))
                {
                    tasks = tasks.OrderByDescending(q => q.CreatedAt).ToList();
                }
                else if (dateSort.Equals("Deadline"))
                {
                    tasks = tasks.OrderByDescending(q => q.Deadline).ToList();
                }
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("Desc"))
                {
                    tasks = tasks.OrderByDescending(q => q.Id).ToList();
                }
                else if (sortBy.Equals("Asc"))
                {
                    tasks = tasks.OrderBy(q => q.Id).ToList();
                }
            }
            return tasks;
        }

        //GET: api/Todos/GetTask/id
        [HttpGet("GetTask/{id}")]
        public ActionResult<Task> GetTask(int id)
        {
            Task task = _db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            return task;
        }

        //GET: api/Todos/GetIncomplete
        [HttpGet("GetIncomplete")]
        public ActionResult<List<Task>> GetIncomplete()
        {
            List<Task> tasks = _db.Tasks.ToList();
            tasks.RemoveAll(q => q.IsCompleted == true);
            return tasks;
        }

        //GET: api/Todos/GetCompleted
        [HttpGet("GetCompleted")]
        public ActionResult<List<Task>> GetCompleted()
        {
            List<Task> tasks = _db.Tasks.ToList();
            tasks.RemoveAll(q => q.IsCompleted == false);
            return tasks;
        }

        //POST: api/Todos/CreateTask
        [HttpPost("CreateTask")]
        public ActionResult CreateTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            task.CreatedAt = DateTime.Now;
            _db.Tasks.Add(task);
            int result = _db.SaveChanges();
            if (result == 1)
            {
                return RedirectToAction(nameof(GetTasks));
            }
            else
            {
                return NoContent();
            }
        }

        //PUT: api/Todos/UpdateTask
        [HttpPut("UpdateTask/{id}")]
        public ActionResult UpdateTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != task.Id)
            {
                return BadRequest();
            }
            try
            {
                _db.Entry(task).State = EntityState.Modified;

                _db.Entry(task).Property(q => q.UserId).IsModified = false;
                _db.Entry(task).Property(q => q.Id).IsModified = false;

                int result = _db.SaveChanges();
                if (result == 1)
                {
                    return RedirectToAction(nameof(GetTasks));
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch(DbUpdateConcurrencyException)
            {
                if (DoesExists(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }

        //DELETE: api/Todos/DeleteTask/id
        [HttpDelete("DeleteTask/{id}")]
        public ActionResult DeleteTask(int id)
        {
            Task taskToDelete = DoesExists(id);
            if (taskToDelete == null)
            {
                return NotFound();
            }
            _db.Tasks.Remove(taskToDelete);
            int result = _db.SaveChanges();
            if (result == 1)
            {
                return RedirectToAction(nameof(GetTasks));
            }
            else
            {
                return StatusCode(500);
            }
        }

        //DELETE: api/Todos/DeleteAll
        [HttpDelete("DeleteAll")]
        public ActionResult DeleteAll()
        {
            List<Task> tasks = _db.Tasks.ToList();
            _db.Tasks.RemoveRange(tasks);
            _db.SaveChanges();
            return Ok("Deleted");
        }

        private Task DoesExists(int id)
        {
            Task task = _db.Tasks.Find(id);
            return task;
        }
    }
}
