
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SaborGregoNew.Pages.Usuario
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            Console.WriteLine("--- O usuário está fazendo logout! ---");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Console.WriteLine("--- O usuário fez logout! ---");
            return RedirectToPage("/Index");
        }
    }
}