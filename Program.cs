using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Repository;
using SaborGregoNew.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// --- 1. Configuração dos Serviços (Dependency Injection) ---

// Configuração do DbContext com SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=SaborGrego.db"));
    

// Registro dos Serviços e Repositórios
builder.Services.AddHttpContextAccessor(); // Necessário para CarrinhoSessionRepository
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ProdutoSqliteRepository>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<EnderecoSqliteRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardSqliteRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoSqliteRepository>();
builder.Services.AddScoped<ICarrinhoRepository, CarrinhoSessionRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioSqliteRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoSqliteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoSqliteRepository>();
builder.Services.AddSession();


// Configuração de Autenticação via Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.AccessDeniedPath = "/AcessoNegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

// Configuração do Razor Pages
builder.Services.AddRazorPages()
    .WithRazorPagesRoot("/Pages"); // Usar a pasta Pages como root (se não for a pasta padrão)

var app = builder.Build();

// Inicializar o banco de dados
DatabaseInitializer.Initialize(app.Services);

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

// 0.5 ATIVAR ARQUIVOS ESTÁTICOS (Necessário para imagens, css, js)
app.UseStaticFiles();

// 1. ATIVAR O ROTEAMENTO (Necessário para saber para onde ir)
app.UseRouting();

// 1.5 ATIVAR SESSÃO (Necessário para carrinho)
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
