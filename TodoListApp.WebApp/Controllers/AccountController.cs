using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("WebApi");
            var response = await client.PostAsJsonAsync("api/users/register", new
            {
                userName = model.UserName,
                email = model.Email,
                password = model.Password
            });

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            // Получить ошибку сервера и показать пользователю
            ModelState.AddModelError("", "Registration failed. Try another email or username.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Пример: запрос в API для аутентификации
            var client = _httpClientFactory.CreateClient("WebApi");
            var response = await client.PostAsJsonAsync("api/users/login", new
            {
                userName = model.UserName,
                password = model.Password
            });

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<User>();
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("sub", user.Id.ToString())
        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return LocalRedirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("WebApi");
            var response = await client.PostAsJsonAsync("api/users/forgot-password", new { email = model.Email });

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Check your email for recovery instructions.";
            }
            else
            {
                ModelState.AddModelError("", "No user with that email found.");
            }

            return View();
        }
    }
}
