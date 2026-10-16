using CueVault.Components;
using CueVault.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UiState>();
builder.Services.AddScoped<CartService>();       // per browser session
builder.Services.AddSingleton<StoreService>();   // shared in-memory store

var app = builder.Build();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
