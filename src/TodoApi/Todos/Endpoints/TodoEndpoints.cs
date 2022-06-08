using System.Security.Claims;
using TodoApi.Todos.Features;
using TodoApi.Todos.Features.Common;

namespace TodoApi.Todos.Endpoints;

public static class TodoEndpoints
{
    private const string Tag = "Todos";
    private const string GetTodoRoute = nameof(GetTodoRoute);

    public static WebApplication MapTodoEndpoints(this WebApplication app)
    {
        app
            .MapGet("/todos", GetAllTodos)
            .WithTags(Tag)
            .Produces<TodoResponse[]>()
            .ProducesProblem(Status401Unauthorized)
            .WithDisplayName("Get all todos");

        app
            .MapGet("/todos/{id}", GetTodo)
            .WithTags(Tag)
            .Produces<TodoResponse>()
            .ProducesProblem(Status401Unauthorized)
            .ProducesProblem(Status404NotFound)
            .WithName(GetTodoRoute)
            .WithDisplayName("Get a todo by id");

        app
            .MapDelete("/todos/{id}", DeleteTodo)
            .WithTags(Tag)
            .Produces<TodoResponse>()
            .ProducesProblem(Status401Unauthorized)
            .ProducesProblem(Status404NotFound)
            .WithDisplayName("Delete a todo by id");

        app
            .MapPut("/todos/{id}", UpdateTodo)
            .WithTags(Tag)
            .Produces<TodoResponse>()
            .ProducesProblem(Status401Unauthorized)
            .ProducesProblem(Status404NotFound)
            .ProducesValidationProblem(Status422UnprocessableEntity)
            .WithDisplayName("Update a todo by id");

        app
            .MapPost("/todos", CreateTodo)
            .WithTags(Tag)
            .Produces<TodoResponse>(Status201Created)
            .ProducesProblem(Status401Unauthorized)
            .ProducesValidationProblem(Status422UnprocessableEntity)
            .WithDisplayName("Creates a todo");

        return app;
    }

    private static async Task<IResult> GetAllTodos(IMediator mediator) =>
        Ok(await mediator.Send(new GetAllTodos.Request()));

    private static async Task<IResult> GetTodo(IMediator mediator, long id) =>
        Ok(await mediator.Send(new GetTodo.Request(id)));

    private static async Task<IResult> DeleteTodo(IMediator mediator, long id) =>
        Ok(await mediator.Send(new DeleteTodo.Request(id)));

    private static async Task<IResult> UpdateTodo(IMediator mediator, long id, UpdateTodo.Request request) =>
        Ok(await mediator.Send(request with { Id = id }));

    private static async Task<IResult> CreateTodo(IMediator mediator, CreateTodo.Request request)
    {
        var createdTodo = await mediator.Send(request);
        return CreatedAtRoute(GetTodoRoute, new { createdTodo.Id }, createdTodo);
    }
}
