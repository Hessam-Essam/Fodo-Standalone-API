using Fodo.Web.ApiClients.Interfaces;
using Fodo.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fodo.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthApiClient _authApi;

        public AccountController(IAuthApiClient authApi)
        {
            _authApi = authApi;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var apiResult = await _authApi.LoginPortalAsync(model.Username, model.Password);

            if (apiResult.Code != 200 || apiResult.Body?.Success != true)
            {
                model.Error = apiResult.Body?.Message ?? "Login failed.";
                return View(model);
            }

            var user = apiResult.Body.User;

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username ?? ""),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("RoleId", user.RoleId.ToString()),
            new Claim("ClientId", user.ClientId.ToString()),
            new Claim("FullNameEn", user.FullNameEn ?? ""),
            new Claim("FullNameAr", user.FullNameAr ?? "")
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
