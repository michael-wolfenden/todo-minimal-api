using TodoApi.Infrastructure.Exceptions;
using TodoApi.Infrastructure.Swagger;
using TodoApi.Persistence;
using TodoApi.Todos.Features.Common;

namespace TodoApi.Todos.Features;

public class UpdateTodo
{
    public record Request([property: SwaggerIgnoreProperty] long Id, string Title, bool IsCompleted) : IRequest<TodoResponse>;
    
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200)
                .NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Request, TodoResponse>
    {
        private readonly TodoDbContext _db;

        public Handler(TodoDbContext db)
        {
            _db = db;
        }

        public async Task<TodoResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            var todo = await _db.Todos
                .FindAsync(new object[]
                {
                    request.Id
                }, cancellationToken);

            if (todo is null)
            {
                throw new NotFoundException();
            }

            todo.Title = request.Title;
            todo.IsCompleted = request.IsCompleted;

            await _db.SaveChangesAsync(cancellationToken);

            return TodoResponse.FromTodo(todo);
        }
    }
}
