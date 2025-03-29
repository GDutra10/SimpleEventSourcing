using Example.Domain;
using Example.InMemory.DifferentDictionary.WebApi.Helpers;
using SimpleEventSourcing;
using Example.InMemory.DifferentDictionary.WebApi.Services;
using Example.Domain.Entities;
using Example.Domain.Events;
using Example.InMemory.DifferentDictionary.WebApi.Handlers;
using SimpleEventSourcing.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.TypeInfoResolver = DefaultJsonTypeInfoResolverHelper.GetEventResolver();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.UseEventSourcing(config => { config.UseInMemory = true; });
builder.Services.AddExampleHandlers();
builder.Services.AddTransient<IEventHandler<OrderEvent, Order>, OrderEventHandler>();
builder.Services.AddTransient<IEventHandler<UserEvent, User>, UserEventHandler>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
