using TodoApi.Persistence;
using TodoApi.Todos.Features.Common;

namespace TodoApi.Todos.Features;

public class CreateTodo
{
    public record Request(string Title) : IRequest<TodoResponse>;

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
            var todo = new Todo(request.Title);

            _db.Todos.Add(todo);
            await _db.SaveChangesAsync(cancellationToken);

            return TodoResponse.FromTodo(todo);
        }
    }
}
