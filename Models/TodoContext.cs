using System;
using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models
{
	public class TodoContext : DbContext
	{

		public TodoContext(DbContextOptions<TodoContext> options)
			: base(options)
		{
		}

		public DbSet<ToDoItem> TodoItems { get; set; } = null;
	}
}

