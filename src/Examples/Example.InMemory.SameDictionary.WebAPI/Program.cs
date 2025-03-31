using Example.Domain;
using Example.Domain.Entities;
using Example.Domain.Handlers;
using Example.InMemory.SameDictionary.WebAPI;
using Example.InMemory.SameDictionary.WebAPI.Handlers;
using SimpleEventSourcing;
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
builder.Services.UseEventSourcing(config => 
{ 
    config.UseInMemory = true;
    config.RetryDelayMs = 1000;
    config.RetryTimes = 2;
});
builder.Services.AddExampleHandlers();
builder.Services.AddTransient<IEventHandler<Event, Order>, OrderEventHandler>();
builder.Services.AddTransient<IEventHandler<Event, User>, UserEventHandler>();
builder.Services.AddScoped<StorageService>();

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
