using System.Net.Http.Json;
using TodoApi.Todos.Features;

namespace TodoApi.Tests.Todos;

public class DeleteTodoSpecs : TestBase
{
    public DeleteTodoSpecs(TextFixture textFixture) : base(textFixture)
    {
    }

    [Fact]
    public async Task validates_user_is_authenticated()
    {
        var result = await UnauthorizedApi().DeleteAsync("/todos/1");

        await Verify(result, DefaultSettings);
    }

    [Fact]
    public async Task validates_todo_exists()
    {
        var result = await AuthorizedApi().DeleteAsync("/todos/1");

        await Verify(result, DefaultSettings);
    }

    [Fact]
    public async Task returns_deleted_todo_for_id_and_removes_from_persistence()
    {
        await AuthorizedApi().PostAsJsonAsync("/todos", new CreateTodo.Request("a todo"));

        var deleteResult = await AuthorizedApi().DeleteAsync("/todos/1");
        await Verify(deleteResult, DefaultSettings.WithUseTextForParameters("delete"));

        var getResult = await AuthorizedApi().GetAsync("/todos/1");
        await Verify(getResult, DefaultSettings.WithUseTextForParameters("get"));
    }
}