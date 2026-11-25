using BookingApp.Api.Repositories;
using BookingApp.Api.Repositories.Interfaces;
using BookingApp.Api.Services;
using BookingApp.Api.Services.Interfaces;
using BookingApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddBookingApi("BookingApp Rooms API");
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

var app = builder.Build();
app.MapBookingApi();
app.Run();
