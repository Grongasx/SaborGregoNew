using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SaborGregoNew.Pages;

public class IndexModel : PageModel
{

    public void OnGet()
    {

    }
    
    public async Task<IActionResult> OnPostLogoutAsync()
    {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
    }
}