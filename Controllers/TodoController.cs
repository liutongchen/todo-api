using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

using TodoApi.Models;

namespace TodoApi.Controllers 
{
    [Route("api/[controller]")] //TODO: WHAT DOES "[controller]" DO HERE?
    [ApiController]
    public class TodoController : ControllerBase 
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.TodoItems.Find(id); //TODO: WHY USE VAR INSTEAD OF RETURN TYPE?
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult<TodoItem> Create(TodoItem item) 
        {
            //TODO: WHERE IS THE DEFAULT VALUE OF ITEM'S FIELD DEFINED?
            _context.TodoItems.Add(item);
            _context.SaveChanges();
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public ActionResult<TodoItem> Update(long id, TodoItem item) // TODO: WHY ARE ALL METHODS INITIALLY-CAPITALIZED
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;
            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}