using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using saborGregoNew.Repository;
using SaborGregoNew.Data;
using SaborGregoNew.Repositories;
using SaborGregoNew.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "SaborGrego.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=SaborGrego.db"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<ICarrinhoRepository, CarrinhoSessionRepository>();
builder.Services.AddSingleton<IUsuarioRepository, UsuarioSqliteRepository>();
builder.Services.AddSingleton<IProdutoRepository, ProdutoSqliteRepository>();
builder.Services.AddSingleton<IPedidoRepository, PedidoSqliteRepository>();
builder.Services.AddSingleton<IEnderecoRepository, EnderecoSqliteRepository>();

// Configuração de Autenticação via Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login/Login"; // Redirecionar para login se não autenticado
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
