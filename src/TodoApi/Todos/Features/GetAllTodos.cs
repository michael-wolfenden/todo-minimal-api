using Microsoft.EntityFrameworkCore;
using TodoApi.Persistence;
using TodoApi.Todos.Features.Common;

namespace TodoApi.Todos.Features;

public class GetAllTodos
{
    public record Request : IRequest<TodoResponse[]>;

    public class Handler : IRequestHandler<Request, TodoResponse[]>
    {
        private readonly TodoDbContext _db;

        public Handler(TodoDbContext db)
        {
            _db = db;
        }

        public async Task<TodoResponse[]> Handle(Request request, CancellationToken cancellationToken)
        {
            var todos = await _db.Todos
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return todos.Select(TodoResponse.FromTodo).ToArray();
        }
    }
}
