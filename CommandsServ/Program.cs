using CommandsServ.AsyncDS;
using CommandsServ.Data;
using CommandsServ.EventProc;
using CommandsServ.SyncDS.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddControllers();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddScoped<IPlatformDC, PlatformDC>();
builder.Services.AddHostedService<MessageBusSub>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

PrepDB.PrepPopulation(app);

app.MapControllers();

app.Run();
