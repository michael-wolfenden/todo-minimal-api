namespace TodoApi.Todos.Features.Common;

public record TodoResponse(long Id, string Title, bool IsCompleted, string CreatedBy, DateTime CreatedOn, string LastModifiedBy, DateTime? LastModifiedOn)
{
    public static TodoResponse FromTodo(Todo todo) =>
        new(todo.Id, todo.Title, todo.IsCompleted, todo.CreatedBy, todo.CreatedOn, todo.LastModifiedBy, todo.LastModifiedOn);
}
