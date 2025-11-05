using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SaborGregoNew.Pages;

public class IndexModel : PageModel
{
    public string Texto { get; set; } = "Page Index";

    public void OnGet()
    {

    }
    
    public async Task<IActionResult> OnPostLogoutAsync()
    {
            Console.WriteLine("--- MÉTODO OnPostLogoutAsync FOI CHAMADO ---");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Console.WriteLine("--- USUÁRIO DESLOGADO, REDIRECIONANDO... ---");
            return RedirectToPage("/Index");
    }
}