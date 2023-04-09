using BeatSaber_FakeMultiplay.Client;
using BeatSaber_FakeMultiplay.Client.Models;
using BeatSaber_FakeMultiplay.Client.Services;
using BeatSaber_FakeMultiplay.Client.Services.BeatSaber;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#if DEBUG
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
#else
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://bs-multiplay.azurewebsites.net") });
#endif

builder.Services.AddSingleton<BeatSaberSocketResolver>()
    .AddTransient<IBeatSaberSocket, BsPlusSocket>()
    .AddTransient<IBeatSaberSocket, DataPullerSocket>()
    .AddTransient<IBeatSaberSocket, HttpStatusSocket>()
;

builder.Services.AddLocalization();

var host = builder.Build();

CultureInfo culture;
var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");

if (result != null)
{
    culture = new CultureInfo(result);
}
else
{
    culture = new CultureInfo(LocalizerSettings.NeutralCulture);
    await js.InvokeVoidAsync("blazorCulture.set", LocalizerSettings.NeutralCulture);
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
