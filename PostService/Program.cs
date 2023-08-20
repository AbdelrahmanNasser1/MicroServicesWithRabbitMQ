using Microsoft.EntityFrameworkCore;
using PostService.Consumer;
using PostService.Context;

var builder = WebApplication.CreateBuilder(args);



// run consumer
Events eve = new Events();
eve.ListenForIntegrationEvents();
// Add services to the container.


builder.Services.AddDbContext<PostDbContext>(ops => ops.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
