using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CryptoLink.WebUI.Client.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddTransient<CookieHandler>();

builder.Services.AddHttpClient("API", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddHttpClient<AuthService>(client =>
     client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
     .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddHttpClient<BookWordService>(client =>
     client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
     .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddHttpClient<LinksService>(client =>
     client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
     .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();