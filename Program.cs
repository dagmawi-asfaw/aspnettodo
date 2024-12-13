
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Todo.Model;
using Todo.TodoDb;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


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

/// redirects to the todo
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todo/$1"));

// custom middler ware
app.Use((context, next) => next(context));





app.MapGet("/todo", Results<Ok<List<TodoModel>>, NotFound> ([FromServices] TodoDb db) =>
{
    List<TodoModel> result = db.todos.OrderBy(todo => todo.id).ToList();

    return result is null ?
    TypedResults.NotFound() :
    TypedResults.Ok<List<TodoModel>>(result);

});

app.MapGet("/todo/{id}", Results<Ok<TodoModel>, NotFound> ([FromRoute] int id, [FromServices] TodoDb db) =>
{
    TodoModel result = db.todos.Where<TodoModel>(model => model.id == id).First();


    Console.WriteLine(result);
    // await db.SaveChangesAsync();

    return result is null ?
    TypedResults.NotFound() :
    TypedResults.Ok(result);

}).AddEndpointFilter(async (context,next)=>{
    var arg = context.GetArgument<int>(0);
     

    return next(context);
});


app.MapPost("/todo", Results<Ok<TodoModel>, NotFound> ([FromBody] TodoModel todo, [FromServices] TodoDb db) =>
{
    db.todos.Add(todo);
    db.SaveChanges();

    return TypedResults.Ok<TodoModel>(todo);

});

app.MapDelete("/todo/{id}", Results<Ok, NotFound> ([FromRoute] int id, [FromServices] TodoDb db) =>
{

    var result = db.todos.Where<TodoModel>(model => model.id == id).First();
 
    if (result is null)
    {
        return TypedResults.NotFound();
    }

    db.todos.Remove(result);
    db.SaveChanges();
    return TypedResults.Ok();

});


app.Run();


