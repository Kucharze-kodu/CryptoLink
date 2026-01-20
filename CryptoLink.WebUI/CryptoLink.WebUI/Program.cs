using CryptoLink.Application;
using CryptoLink.Architecture;
using CryptoLink.Architecture.Utils.Extensions;
using CryptoLink.WebUI.Client.Pages;
using CryptoLink.WebUI.Components;
using CryptoLink.WebUI.Controllers.Modules;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
    builder.Services.AddControllers();


    //CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

    builder.Services
    .AddApplication()
    .AddArchitecture(builder.Configuration);
}

// Swagger (opcjonalnie, przydatne do test�w)
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddScoped(sp => new HttpClient
{
    // Ustawiamy adres bazowy na adres samej aplikacji serwerowej
    BaseAddress = new Uri(sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>().BaseUri)
});


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

// Mapowanie Endpoint�w
// ENDPOINTS
app.AddLinkExtendedModule();
app.AddLinkWordModule();
app.AddAuthenticationEndpoints();
app.AddAuthStateProvider();
// END ENDPOINTS



// Apply database migrations
app.ApplyMigration();
app.ApplySeeder();

// Mapowanie komponent�w Blazor
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CryptoLink.WebUI.Client._Imports).Assembly);

app.Run();