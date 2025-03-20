using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2Acl.Models;
using PracticaMvcCore2Acl.Repositories;

namespace PracticaMvcCore2Acl.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;

        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string pass)
        {
            Usuario usuario = await this.repo.LoginUsuarioAsync(email, pass);

            if (usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                Claim claimID = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                Claim claimEmail = new Claim(ClaimTypes.Name, usuario.Email);
                Claim claimNombre = new Claim("Nombre", usuario.Nombre);
                Claim claimApellidos = new Claim("Apellidos", usuario.Apellidos);
                Claim claimFoto = new Claim("Foto", usuario.Foto);

                identity.AddClaim(claimID);
                identity.AddClaim(claimEmail);
                identity.AddClaim(claimNombre);
                identity.AddClaim(claimApellidos);
                identity.AddClaim(claimFoto);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                string controller = TempData["controller"]?.ToString() ?? "Libros";
                string action = TempData["action"]?.ToString() ?? "Index";

                return RedirectToAction(action, controller);
            }
            else
            {
                ViewData["Mensaje"] = "Credenciales incorrectas";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Managed");
        }
    }
}
