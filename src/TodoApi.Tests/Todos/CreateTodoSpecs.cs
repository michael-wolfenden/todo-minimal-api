using System.Net.Http.Json;
using TodoApi.Todos.Features;

namespace TodoApi.Tests.Todos;

public class CreateTodoSpecs : TestBase
{
    public CreateTodoSpecs(TextFixture textFixture) : base(textFixture)
    {
    }

    [Fact]
    public async Task validates_user_is_authenticated()
    {
        var request = new CreateTodo.Request("some todo");
        var result = await UnauthorizedApi().PostAsJsonAsync("/todos", request);

        await Verify(result, DefaultSettings);
    }

    [Fact]
    public async Task persists_todo_and_returns_it()
    {
        var request = new CreateTodo.Request("some todo");
        var result = await AuthorizedApi().PostAsJsonAsync("/todos", request);

        await Verify(result, DefaultSettings);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task validates_title_is_not_null_or_blank(string title)
    {
        var request = new CreateTodo.Request(title);
        var result = await AuthorizedApi().PostAsJsonAsync("/todos", request);

        await Verify(result, DefaultSettings.WithUseParameters(title));
    }

    [Fact]
    public async Task validates_titles_is_less_than_200_characters()
    {
        var request = new CreateTodo.Request(new string('*', 201));
        var result = await AuthorizedApi().PostAsJsonAsync("/todos", request);

        await Verify(result, DefaultSettings);
    }
}