
namespace Todo.TodoDb;
using Microsoft.EntityFrameworkCore;
using Todo.Model;

public class TodoDb : DbContext
{

    public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

        

    public DbSet<TodoModel> todos => Set<TodoModel>();
    // public required DbSet<TodoModel> todos { get; set; }
}