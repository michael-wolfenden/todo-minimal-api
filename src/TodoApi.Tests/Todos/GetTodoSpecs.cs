using System.Net.Http.Json;
using TodoApi.Todos.Features;

namespace TodoApi.Tests.Todos;

public class GetTodoSpecs : TestBase
{
    public GetTodoSpecs(TextFixture textFixture) : base(textFixture)
    {
    }

    [Fact]
    public async Task validates_user_is_authenticated()
    {
        var result = await UnauthorizedApi().GetAsync("/todos/1");

        await Verify(result, DefaultSettings);
    }

    [Fact]
    public async Task validates_todo_exists()
    {
        var result = await AuthorizedApi().GetAsync("/todos/1");

        await Verify(result, DefaultSettings);
    }

    [Fact]
    public async Task returns_persisted_todo_for_id()
    {
        await AuthorizedApi().PostAsJsonAsync("/todos", new CreateTodo.Request("a todo"));

        var result = await AuthorizedApi().GetAsync("/todos/1");

        await Verify(result, DefaultSettings);
    }
}