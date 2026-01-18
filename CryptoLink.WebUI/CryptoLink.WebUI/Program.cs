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

// Swagger (opcjonalnie, przydatne do testów)
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();




// Apply database migrations
app.ApplyMigration();


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

// Mapowanie Endpointów
// ENDPOINTS
app.AddLinkExtendedModule();
app.AddLinkWordModule();
app.AddAuthenticationEndpoints();
app.AddAuthStateProvider();
// END ENDPOINTS


// Mapowanie komponentów Blazor
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CryptoLink.WebUI.Client._Imports).Assembly);

app.Run();