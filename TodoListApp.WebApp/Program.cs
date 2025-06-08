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

builder.Services.AddHttpClient<IToDoListService, ToDoListWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7181/");
});

builder.Services.AddHttpClient<ITaskItemService, TaskItemWebApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7181/");
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
