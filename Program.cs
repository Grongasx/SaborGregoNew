using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Models;
using SaborGregoNew.Repositories;
using SaborGregoNew.Repository;
using SaborGregoNew.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDistributedMemoryCache(); // Requerido para a sessão
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "SaborGrego.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=SaborGrego.db"));

builder.Services.AddHttpContextAccessor();

// Registro dos Serviços e Repositórios
builder.Services.AddScoped<CarrinhoRepository>();
builder.Services.AddScoped<CarrinhoService>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<EnderecoRepository>();
builder.Services.AddScoped<EnderecoService>();


// Configuração de Autenticação via Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login"; // Redirecionar para login se não autenticado
        options.AccessDeniedPath = "/AcessoNegado"; // Onde ir se não tiver permissão
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tempo de vida do cookie
    });

// Configuração do Razor Pages
builder.Services.AddRazorPages()
    .WithRazorPagesRoot("/Pages"); // Usar a pasta Pages como root (se não for a pasta padrão)

var app = builder.Build();

// --- 2. Pipeline de Middleware (ORDEM CRÍTICA) ---

// Tratamento de Erro
if (app.Environment.IsDevelopment())
{
    // Em Dev, usamos a página detalhada de exceção
    app.UseDeveloperExceptionPage();
}
else
{
    // Em Produção, usamos uma página de erro genérica e HSTS
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseStaticFiles();
// Opcional: Força o uso de HTTPS
app.UseHttpsRedirection();

// A Ordem Correta é: Roteamento -> Autenticação/Autorização -> Mapeamento de Endpoints

// 1. ATIVAR O ROTEAMENTO (Necessário para saber para onde ir)
app.UseRouting();

app.UseSession();

// 2. HABILITAR SEGURANÇA (Necessário para identificar o usuário antes de checar as regras)
app.UseAuthentication();
app.UseAuthorization();

// 3. MAPEAR AS PÁGINAS RAZOR (Finalmente diz para onde o roteamento deve ir)
app.MapRazorPages();
app.MapPost("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
});

app.Run();
