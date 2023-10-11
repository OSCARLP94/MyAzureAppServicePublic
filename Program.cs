using MyAzureAppService.Services;
using MyAzureAppService.Entities;
using MyAzureAppService.DataAccess.Repositories;
using MyAzureAppService.DataAccess;
using MyAzureAppService.Hubs;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

#region Microsoft Identity AZ
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});
#endregion

#region Inyeccion dependencias
builder.Services.AddSignalR();

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["AZApplicationInsight:Key"]);

string environment = builder.Configuration["Environment"];

//Blob storage
var configAZblob = builder.Configuration.GetSection("AZBlob");

builder.Services.AddSingleton<IBackupService>(
    new BackupService(configAZblob["AccountName"], configAZblob["ContainerName"],
    environment == "development" ? configAZblob["ConnectionString"] : null)
);

//Azure cosmos
var configAZcosmos = builder.Configuration.GetSection("AZCosmos");

builder.Services.AddSingleton<IMyCosmosDataAccess>(
    new MyCosmosDataAccess(configAZcosmos["EndPoint"], nameof(MyCosmosDataAccess),
        configAZcosmos["Key"] )
    );

//Repositorios
builder.Services.AddSingleton<ICosmoRepository<Post>>(provider =>
{
    var cosmoDataAccess = provider.GetRequiredService<IMyCosmosDataAccess>();
    return new CosmoRepository<Post>(cosmoDataAccess, nameof(Post), $"/{nameof(Post.IdUser)}");
});

//Servicios
builder.Services.AddSingleton<IPostService, PostService>();

#endregion

var app = builder.Build();

//Hubs
app.MapHub<BlobNotifyHub>("hubs/BlobNotification4");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

