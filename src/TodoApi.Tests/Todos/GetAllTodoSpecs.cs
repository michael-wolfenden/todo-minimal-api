using System.Net.Http.Json;
using TodoApi.Todos.Features;

namespace TodoApi.Tests.Todos;

public class GetAllTodoSpecs : TestBase
{
    public GetAllTodoSpecs(TextFixture textFixture) : base(textFixture)
    {
    }

    [Fact]
    public async Task validates_user_is_authenticated()
    {
        var result = await UnauthorizedApi().GetAsync("/todos");

        await Verify(result, DefaultSettings);
    }

    [Fact]
    public async Task returns_all_persisted_todos()
    {
        await AuthorizedApi().PostAsJsonAsync("/todos", new CreateTodo.Request("a todo"));
        await AuthorizedApi().PostAsJsonAsync("/todos", new CreateTodo.Request("another todo"));

        var result = await AuthorizedApi().GetAsync("/todos");

        await Verify(result, DefaultSettings);
    }
}