
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Todo.Model;
using Todo.TodoDb;
using System.ComponentModel;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// add the data base connection
builder.Services.AddDbContext<TodoDb>(options => options.UseInMemoryDatabase("TodoList"));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

WebApplication app = builder.Build();




//builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddOpenApiDocument(config =>
// {
//     config.DocumentName = "TodoAPI";
//     config.Title = "TodoAPI v1";
//     config.Version = "v1";
// });



app.MapGet("/", () => "Hello World!");

app.Run();





app.MapGet("/todo", Results<Ok<List<TodoModel>>, NotFound> (TodoModel todo, TodoDb db) =>
{
    List<TodoModel> result = db.todos.ToList();

    return result is null ?
    TypedResults.NotFound() :
    TypedResults.Ok<List<TodoModel>>(result);

});

app.MapGet("/todo/{id}", Results<Ok<TodoModel>, NotFound> (int id, TodoDb db) =>
{
    TodoModel result = db.todos.Where<TodoModel>(model => model.id == id).First();


    Console.WriteLine(result);
    // await db.SaveChangesAsync();

    return result is null ?
    TypedResults.NotFound() :
    TypedResults.Ok(result);

});


app.MapPost("/todo", async (TodoModel todo, TodoDb db) =>
{
    db.todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todo/{todo.id}", todo);

});



app.MapDelete("/todo/{id}", Results<Ok, NotFound> (int id, TodoDb db) =>
{

    var result = db.todos.Where<TodoModel>(model => model.id == id).First();

    if (result is null)
    {
        return TypedResults.NotFound();
    }
    else
    {
        db.todos.Remove(result);
        db.SaveChanges();
        return TypedResults.Ok();
    }
});




