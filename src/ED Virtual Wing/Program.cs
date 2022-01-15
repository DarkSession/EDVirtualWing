using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "edvwuser";
    options.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

string httpOrigin = Environment.GetEnvironmentVariable("EDVW_HTTP_ORIGIN") ?? string.Empty;

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(httpOrigin)
                    .AllowCredentials()
                    .WithMethods(HttpMethods.Post, HttpMethods.Get, HttpMethods.Options)
                    .WithHeaders("content-type", "content-length")
                    ;
        });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

WebSocketOptions webSocketOptions = new()
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
};
webSocketOptions.AllowedOrigins.Add(httpOrigin);
app.UseWebSockets(webSocketOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.Use((ctx, next) =>
{
    ctx.Response.Headers.Add("Content-Security-Policy", string.Empty);
    ctx.Response.Headers.Add("X-Frame-Options", "deny");
    ctx.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    ctx.Response.Headers.Add("Referrer-Policy", "strict-origin");
    ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    ctx.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate, max-age=0");
    ctx.Response.Headers.Add("Pragma", "no-cache");
    return next();
});
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});

app.MapFallbackToFile("index.html"); ;

app.Run();
