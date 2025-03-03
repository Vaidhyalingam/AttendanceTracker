using AttendanceTracker;
using AttendanceTracker.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Set the base address for GitHub Pages
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Adjust for GitHub Pages URL (replace "AttendanceTracker" with your repo name)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://vaidhyalingam.github.io/AttendanceTracker/")
});

builder.Services.AddScoped<AttendanceService>();


await builder.Build().RunAsync();