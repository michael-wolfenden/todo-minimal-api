using TodoApi.Persistence;

namespace TodoApi.Todos;

public class Todo : BaseEntity
{
    public Todo(string title)
    {
        Title = title;
        IsCompleted = false;
    }

    public bool IsCompleted { get; set; }
    public string Title { get; set; }
}
