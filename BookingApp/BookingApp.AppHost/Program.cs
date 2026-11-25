var builder = DistributedApplication.CreateBuilder(args);

var postgres = string.IsNullOrWhiteSpace(builder.Configuration["ConnectionStrings:bookingdb"])
    ? builder.AddPostgres("postgres").WithDataVolume()
    : null;
IResourceBuilder<IResourceWithConnectionString> database = postgres is null
    ? builder.AddConnectionString("bookingdb")
    : postgres.AddDatabase("bookingdb");
var jwtKey = builder.AddParameter("jwt-key", secret: true);
var adminEmail = builder.AddParameter("admin-email");
var adminPassword = builder.AddParameter("admin-password", secret: true);

var identity = Configure(builder.AddProject<Projects.BookingApp_Identity>("identity"), 5023)
    .WithEnvironment("SeedAdmin__Email", adminEmail)
    .WithEnvironment("SeedAdmin__Password", adminPassword)
    .WithHttpHealthCheck("/health");

if (postgres is not null)
    identity.WaitFor(postgres);

var rooms = Configure(builder.AddProject<Projects.BookingApp_Rooms>("rooms"), 5022)
    .WaitFor(identity)
    .WithHttpHealthCheck("/health");

Configure(builder.AddProject<Projects.BookingApp_Api>("bookings"), 5021)
    .WaitFor(identity)
    .WithReference(rooms)
    .WithHttpHealthCheck("/health");

builder.Build().Run();

IResourceBuilder<ProjectResource> Configure(IResourceBuilder<ProjectResource> project, int port) =>
    project.WithHttpEndpoint(port: port, name: "http")
        .WithReference(database)
        .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
        .WithEnvironment("Jwt__Issuer", "BookingApp.Identity")
        .WithEnvironment("Jwt__Audience", "BookingApp")
        .WithEnvironment("Jwt__ExpiresMinutes", "60")
        .WithEnvironment("Jwt__Key", jwtKey);
