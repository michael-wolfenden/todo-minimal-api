using System.Net.Http.Json;
using TodoApi.Todos.Features;

namespace TodoApi.Tests.Todos;

public class UpdateTodoSpecs : TestBase
{
    public UpdateTodoSpecs(TextFixture textFixture) : base(textFixture)
    {
    }

    [Fact]
    public async Task validates_user_is_authenticated()
    {
        var request = new UpdateTodo.Request(-1, "updated todo", false);
        var result = await UnauthorizedApi().PutAsJsonAsync("/todos/1", request);

        await Verify(result, DefaultSettings);
    }

    [Fact]
    public async Task validates_todo_exists()
    {
        var request = new UpdateTodo.Request(-1, "updated todo", false);
        var result = await AuthorizedApi().PutAsJsonAsync("/todos/1", request);

        await Verify(result, DefaultSettings);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task validates_title_is_not_null_or_blank(string title)
    {
        var request = new UpdateTodo.Request(-1, title, false);
        var result = await AuthorizedApi().PutAsJsonAsync("/todos/1", request);

        await Verify(result, DefaultSettings.WithUseParameters(title));
    }

    [Fact]
    public async Task validates_titles_is_less_than_200_characters()
    {
        var request = new UpdateTodo.Request(-1, new string('*', 201), false);
        var result = await AuthorizedApi().PutAsJsonAsync("/todos/1", request);

        await Verify(result, DefaultSettings);
    }
    
    [Fact]
    public async Task persists_updated_todo_and_returns_it()
    {
        await AuthorizedApi().PostAsJsonAsync("/todos", new CreateTodo.Request("some todo"));
        
        var updateRequest = new UpdateTodo.Request(-1, "updated todo", true);
        var updateResult = await AuthorizedApi().PutAsJsonAsync("/todos/1", updateRequest);
        await Verify(updateResult, DefaultSettings.WithUseTextForParameters("update"));
        
        var getResult = await AuthorizedApi().GetAsync("/todos/1");
        await Verify(getResult, DefaultSettings.WithUseTextForParameters("get"));
    }
}