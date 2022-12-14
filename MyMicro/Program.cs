using Microsoft.EntityFrameworkCore;
using MyMicro.AsyncDS;
using MyMicro.Data;
using MyMicro.SyncDS.Grpc;
using MyMicro.SyncDS.Http;


var builder = WebApplication.CreateBuilder(args);

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
var env = provider.GetRequiredService<IWebHostEnvironment>();

if(env.IsProduction())
{
    Console.WriteLine("--> Using SQLserver database");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(configuration.GetConnectionString("PlatformsConnection")));
}
else
{
    Console.WriteLine("--> Using internal memory database");
    builder.Services.AddDbContext<AppDbContext>(opt => 
        opt.UseInMemoryDatabase("InMem"));
}
// Add services to the container.

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddSingleton<IMessageBC, MessageBC>();
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddHttpClient<ICommandDataCli, HttpComDataCli>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine($"--> Service Endpoint {configuration["CommandsServ"]}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints => 
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<GrpcMyMicroService>();
    endpoints.MapGet("/Protobufs/platform.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protobufs/platform.protos"));
    });
});

PrepDb.PrepPopulation(app, env.IsProduction());

app.Run();