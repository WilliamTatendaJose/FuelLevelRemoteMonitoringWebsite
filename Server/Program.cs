using Radzen;
using DeanRemoteMonitoringWeb.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using DeanRemoteMonitoringWeb.Server.Data;
using Microsoft.AspNetCore.Identity;
using DeanRemoteMonitoringWeb.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024).AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddHttpClient();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpfdnVTR2ldUENxXEE=");
builder.Services.AddScoped<DeanRemoteMonitoringWeb.Server.RAZDENService>();
builder.Services.AddDbContext<RAZDENContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RAZDENConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderRAZDEN = new ODataConventionModelBuilder();
    oDataBuilderRAZDEN.EntitySet<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank>("FuelTanks");
    oDataBuilderRAZDEN.EntitySet<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling>("FuelRefillings");
    opt.AddRouteComponents("odata/RAZDEN", oDataBuilderRAZDEN.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<DeanRemoteMonitoringWeb.Client.RAZDENService>();
builder.Services.AddHttpClient("DeanRemoteMonitoringWeb.Server").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false }).AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<DeanRemoteMonitoringWeb.Client.SecurityService>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RAZDENConnection"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<AuthenticationStateProvider, DeanRemoteMonitoringWeb.Client.ApplicationAuthenticationStateProvider>();
builder.Services.AddDbContext<DeanRemoteMonitoringWeb.Server.Data.RAZDENContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RAZDENConnection"));
});
builder.Services.AddDbContext<DeanRemoteMonitoringWeb.Server.Data.RAZDENContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(builder.Configuration.GetConnectionString("RAZDENConnection"));
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseHeaderPropagation();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(DeanRemoteMonitoringWeb.Client._Imports).Assembly);
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Run();