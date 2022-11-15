using BeatSaber_FakeMultiplay.Server.Hubs;
using BeatSaber_FakeMultiplay.Shared.Models.Socket;
using Microsoft.AspNetCore.ResponseCompression;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("https://bi-sei.sakura.live/");
            policy.WithOrigins("https://beatsaver.com/");
        });
});

builder.Services.AddSignalR(conf =>
{
    conf.MaximumReceiveMessageSize = null;
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(myAllowSpecificOrigins);

app.MapRazorPages();
app.MapControllers();
app.MapHub<BsHub>(SocketUri.Lobby);
app.MapFallbackToFile("index.html");

app.Run();
