using TodoListApp.Services;
using TodoListApp.Services.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddHttpContextAccessor();

// Именованный клиент для вызова любых WebApi ручек
builder.Services.AddHttpClient("WebApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7244/");
});

// Типизированные клиенты для сервисов (если используешь DI в других контроллерах)
builder.Services.AddHttpClient<ITodoListService, TodoListWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7244/");
});

builder.Services.AddHttpClient<ITaskItemService, TaskItemWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7244/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
