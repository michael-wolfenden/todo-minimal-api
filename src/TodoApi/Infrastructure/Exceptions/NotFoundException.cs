namespace TodoApi.Infrastructure.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("The specified resource was not found.") { }
}
