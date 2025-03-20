using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2Acl.Extensions;
using PracticaMvcCore2Acl.Filters;
using PracticaMvcCore2Acl.Models;
using PracticaMvcCore2Acl.Repositories;

namespace PracticaMvcCore2Acl.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;

        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Libro> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }

        public async Task<IActionResult> GeneroLibros(int idgenero)
        {
            List<Libro> libros = await this.repo.GetLibrosGeneroAsync(idgenero);
            return View(libros);
        }

        public async Task<IActionResult> DetalleLibro(int idlibro)
        {
            Libro libro = await this.repo.FindLibroAsync(idlibro);
            return View(libro);
        }

        public async Task<IActionResult> AddCarrito(int idlibro)
        {
            var carritoIds = HttpContext.Session.GetObject<List<int>>("CarritoIds");
            if (carritoIds == null)
            {
                carritoIds = new List<int>();
            }

            carritoIds.Add(idlibro);
            HttpContext.Session.SetObject("CarritoIds", carritoIds);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveCarrito(int idlibro)
        {
            var carritoIds = HttpContext.Session.GetObject<List<int>>("CarritoIds");

            if (carritoIds.Contains(idlibro))
            {
                carritoIds.Remove(idlibro);
            }

            if (carritoIds.Count > 0)
            {
                HttpContext.Session.SetObject("CarritoIds", carritoIds);
            }
            else
            {
                HttpContext.Session.Remove("CarritoIds");
            }

            return RedirectToAction("Carrito");
        }

        public async Task<IActionResult> Carrito()
        {
            var carritoIds = HttpContext.Session.GetObject<List<int>>("CarritoIds");
            if (carritoIds == null)
            {
                carritoIds = new List<int>();
            }

            List<Libro> libros = await this.repo.FindLibrosCarrito(carritoIds);

            return View(libros);
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> Finalizar()
        {
            var carritoIds = HttpContext.Session.GetObject<List<int>>("CarritoIds");
            if (carritoIds == null)
            {
                carritoIds = new List<int>();
            }

            List<Libro> libros = await this.repo.FindLibrosCarrito(carritoIds);

            return View("Carrito");
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> Perfil()
        {
            return View();
        }
    }
}
