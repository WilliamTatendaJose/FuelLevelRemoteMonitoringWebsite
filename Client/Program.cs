using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using DeanRemoteMonitoringWeb.Client;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddMudServices();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<DeanRemoteMonitoringWeb.Client.RAZDENService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("DeanRemoteMonitoringWeb.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DeanRemoteMonitoringWeb.Server"));
builder.Services.AddScoped<DeanRemoteMonitoringWeb.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, DeanRemoteMonitoringWeb.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
await host.RunAsync();