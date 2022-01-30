using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.FDevApi;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal;
using ED_Virtual_Wing.WebSockets;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseMySql(builder.Configuration["EDVW:ConnectionString"],
    new MariaDbServerVersion(new Version(10, 6, 0)),
    options =>
    {
        options.EnableRetryOnFailure();
        options.CommandTimeout(60 * 10 * 1000);
    })
#if DEBUG
                .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine)
#endif
    ;
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.User.AllowedUserNameCharacters += " ";
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.UseMemberCasing();
    });
builder.Services.AddRazorPages();
builder.Services.AddSingleton<WebSocketServer>();
builder.Services.AddSingleton<JournalProcessor>();
builder.Services.AddSingleton<FDevApi>();

string httpOrigin = builder.Configuration["EDVW:HttpOrigin"];
Uri httpOriginUri = new(httpOrigin);

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

app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

string contentSecurityPolicy =
    "default-src 'self'; " +
    $"connect-src 'self' wss://{httpOriginUri.Host}/; " +
    "style-src 'self' 'unsafe-inline'; " +
    "script-src 'self' 'unsafe-inline';"; // Once this is fixed: https://github.com/angular/angular-cli/issues/20864 we want to remove 'unsafe-inline' again. In Angular 13 the suggested workaround there doesn't seem to work anymore.

app.Use((ctx, next) =>
{
    ctx.Response.Headers.Add("Content-Security-Policy", contentSecurityPolicy);
    ctx.Response.Headers.Add("X-Frame-Options", "deny");
    ctx.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    ctx.Response.Headers.Add("Referrer-Policy", "strict-origin");
    ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    // ctx.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate, max-age=0");
    // ctx.Response.Headers.Add("Pragma", "no-cache");
    return next();
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapFallbackToFile("index.html");

app.Run();
