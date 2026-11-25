using BookingApp.Api.Repositories;
using BookingApp.Api.Repositories.Interfaces;
using BookingApp.Api.Services;
using BookingApp.Api.Services.Interfaces;
using BookingApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddBookingApi("BookingApp Bookings API");
builder.Services.AddServiceDiscovery();
builder.Services.AddHttpClient<RoomsClient>(client =>
{
    client.BaseAddress = new Uri("http://rooms");
    client.Timeout = TimeSpan.FromSeconds(5);
}).AddServiceDiscovery();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAdminService, AdminService>();

var app = builder.Build();
app.MapBookingApi();
app.Run();
