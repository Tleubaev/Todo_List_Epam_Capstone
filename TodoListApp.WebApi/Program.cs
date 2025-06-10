using TodoListApp.Services;
using TodoListApp.Services.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<TodoDbContext>(options =>
//    options.UseInMemoryDatabase("TodoListDb"));

builder.Services.AddScoped<ITodoListService, TodoListDatabaseService>();
builder.Services.AddScoped<ITaskItemService, TaskItemDatabaseService>();

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
