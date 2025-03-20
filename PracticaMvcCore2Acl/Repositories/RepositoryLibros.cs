using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2Acl.Data;
using PracticaMvcCore2Acl.Models;

namespace PracticaMvcCore2Acl.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;

        public RepositoryLibros(LibrosContext context) 
        {
            this.context = context;
        }

        public async Task<Usuario> LoginUsuarioAsync(string email, string pass)
        {
            return await this.context.Usuarios
                .Where(u => u.Email == email && u.Pass == pass)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        public async Task<List<Libro>> GetLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }

        public async Task<List<Libro>> GetLibrosGeneroAsync(int idgenero)
        {
            return await this.context.Libros
                .Where(x => x.IdGenero == idgenero)
                .ToListAsync();
        }

        public async Task<Libro> FindLibroAsync(int idlibro)
        {
            return await this.context.Libros
                    .Where(l => l.IdLibro == idlibro)
                    .FirstOrDefaultAsync();
        }

        public async Task<List<Libro>> FindLibrosCarrito(List<int> ids)
        {
            var libros = this.context.Libros.Where(l => ids.Contains(l.IdLibro));
                    
              return await libros.ToListAsync();
        }
    }
}
