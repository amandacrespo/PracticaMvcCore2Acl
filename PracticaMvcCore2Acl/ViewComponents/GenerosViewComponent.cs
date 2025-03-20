using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PracticaMvcCore2Acl.Models;
using PracticaMvcCore2Acl.Repositories;

namespace PracticaMvcCore2Acl.ViewComponents
{
    public class GenerosViewComponent: ViewComponent
    {
        private RepositoryLibros repo;

        public GenerosViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task <IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGenerosAsync();
            return View("Default", generos);
        }
    }
}
