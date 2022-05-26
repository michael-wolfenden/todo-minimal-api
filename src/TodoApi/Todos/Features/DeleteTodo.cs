using TodoApi.Infrastructure.Exceptions;
using TodoApi.Persistence;
using TodoApi.Todos.Features.Common;

namespace TodoApi.Todos.Features;

public class DeleteTodo
{
    public record Request(long Id) : IRequest<TodoResponse>;

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

            _db.Todos.Remove(todo);
            await _db.SaveChangesAsync(cancellationToken);

            return TodoResponse.FromTodo(todo);
        }
    }
}
