
namespace Todo.Model;
//public record TodoRecord(int id, string name, DateTime dueDate, bool isCompleted);


public class TodoModel
{
    public int id { get; set; }
    public required string name { get; set; }

    public DateTime dueDate { get; set; }

    public bool isCompleted { get; set; }

    public static implicit operator List<object>(TodoModel v)
    {
        throw new NotImplementedException();
    }
}