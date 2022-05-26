# 📃Minimal APIs Todos

This repository is my attempt at implementing an API using the the new [Minimal APIs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis) feature of .NET 6.

I wanted to investigate

- [x] How to best organise the bootstrapping code in [Program.cs](src\TodoApi\Program.cs)
- [x] Use [Duende IdentityServer](https://duendesoftware.com/) for authentication
- [x] Using [MediatR](https://github.com/jbogard/MediatR) and custom pipeline behaviours to handle cross cutting concerns
- [x] Integration testing using Docker and [Verify.Http](https://github.com/VerifyTests/Verify) to generate snapshots of HTTP requests and response
- [x] Representing all error response as [Problem Details](https://datatracker.ietf.org/doc/html/rfc7807)
- [x] Using [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) for persistence
- [x] Using [OpenAPI](https://swagger.io/specification/) for API documentation

## Getting Started

This project is configured to reference a sql server database and a seq instance for logging.

By default, the database will be configured to run in a docker container and already has the connection
string configured in your launch settings.

To start the sql server container and seq instance, run the following from the root directory.

`docker-compose up`

### Running the API

Once you have your database(s) running, you can run the API either from your favourite IDE or by running the following command from the root directory.

`dotnet run --project src\TodoApi\TodoApi.csproj`

Once the API is running you can view the API documentation at [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html).

You will need to authorize with Duende IdentityServer with the `api` scope by clicking "Authorize" to be authorised to call the API endpoints.

### Running Integration Tests

To run the integration tests, you can either use your favourite IDE or the following command from the root directory

`dotnet test .\src\MinimalApiTodo.sln`

Before all the test's run, a sql server docker container will be started, then before each individual test [Respawn](https://github.com/jbogard/Respawn) is used to clear all database tables, to ensure a clean database before each test.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
