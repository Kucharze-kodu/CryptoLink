using CryptoLink.Application;
using CryptoLink.Architecture;
using CryptoLink.WebUI.Client.Pages;
using CryptoLink.WebUI.Components;
using CryptoLink.WebUI.Controllers.Modules;
using CryptoLink.Architecture.Utils.Extensions;

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

    // Add health checks
    builder.Services.AddHealthChecks();
}

// Swagger (opcjonalnie, przydatne do test�w)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

// app.UseHttpsRedirection(); // WYŁĄCZAMY NA CZAS TESTÓW HTTP
app.UseStaticFiles();
app.UseAntiforgery();
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

// Mapowanie Endpoint�w
// ENDPOINTS
app.AddAuthenticationEndpoints();
// Health check endpoint
app.MapHealthChecks("/health");
// END ENDPOINTS



// Apply database migrations
app.ApplyMigration();
app.ApplySeeder();

// Mapowanie komponent�w Blazor
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CryptoLink.WebUI.Client._Imports).Assembly);

app.Run();