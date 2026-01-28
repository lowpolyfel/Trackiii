using Trackii.Application;
using Trackii.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// =====================
// MVC
// =====================
builder.Services.AddControllersWithViews();

// =====================
// Infrastructure → Application (ORDEN CRÍTICO)
// =====================
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// =====================
// Pipeline
// =====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
