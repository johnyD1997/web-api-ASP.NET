//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ToDoAPI.Models;

//namespace ToDoAPI.Controllers
//{
//    [Route("api/TodoItems")]
//    [ApiController]
//    public class TodoItemsController : ControllerBase
//    {
//        private readonly TodoContext _context;

//        public TodoItemsController(TodoContext context)
//        {
//            _context = context;
//        }

//        // GET: api/TodoItems
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ToDoItemDTO>>> GetTodoItems()
//        {
//          if (_context.TodoItems == null)
//          {
//              return NotFound();
//          }
//            return await _context.TodoItems.ToListAsync();
//        }

//        // GET: api/TodoItems/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<ToDoItemDTO>> GetToDoItem(long id)
//        {
//          if (_context.TodoItems == null)
//          {
//              return NotFound();
//          }
//            var toDoItem = await _context.TodoItems.FindAsync(id);

//            if (toDoItem == null)
//            {
//                return NotFound();
//            }

//            return toDoItem;
//        }

//        // PUT: api/TodoItems/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutToDoItem(long id, ToDoItemDTO toDoItemDTO)
//        {
//            if (id != toDoItemDTO.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(toDoItemDTO).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ToDoItemExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/TodoItems
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<ToDoItemDTO>> PostToDoItem(ToDoItemDTO toDoItem)
//        {
//          if (_context.TodoItems == null)
//          {
//              return Problem("Entity set 'TodoContext.TodoItems'  is null.");
//          }
//            _context.TodoItems.Add(toDoItem);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetToDoItem), new { id = toDoItem.Id }, toDoItem);
//        }

//        // DELETE: api/TodoItems/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteToDoItem(long id)
//        {
//            if (_context.TodoItems == null)
//            {
//                return NotFound();
//            }
//            var toDoItem = await _context.TodoItems.FindAsync(id);
//            if (toDoItem == null)
//            {
//                return NotFound();
//            }

//            _context.TodoItems.Remove(toDoItem);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool ToDoItemExists(long id)
//        {
//            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemDTO>>> GetTodoItems()
    {
        return await _context.TodoItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, ToDoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<ToDoItemDTO>> PostTodoItem(ToDoItemDTO todoDTO)
    {
        var todoItem = new ToDoItem
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
    private static ToDoItemDTO ItemToDTO(ToDoItem todoItem) =>
    new ToDoItemDTO
    {
        Id = todoItem.Id,
           Name = todoItem.Name,
           IsComplete = todoItem.IsComplete
       };
}