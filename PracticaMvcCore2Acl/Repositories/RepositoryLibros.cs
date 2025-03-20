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

        public async Task<List<Libro>> FindLibrosCarritoAsync(List<int> ids)
        {
            var libros = this.context.Libros.Where(l => ids.Contains(l.IdLibro));
                    
              return await libros.ToListAsync();
        }

        public async Task<int> GetMaxIdPedidosAsync()
        {
            var pedidos = await this.context.Pedidos.ToListAsync();
            var max = pedidos.Count + 1;
            return max;
        }

        public async Task<int> GetMaxIdFacturaAsync()
        {
            var max = await this.context.Pedidos
                        .MaxAsync(f => f.IdFactura);
            return max + 1;
        }

        public async Task FinalizarCompraLibrosAsync(List<int> ids, int idusuario)
        {
            var libros = this.context.Libros.Where(l => ids.Contains(l.IdLibro));
            int nuevoId = await this.GetMaxIdPedidosAsync();
            int nuevoIdFactura = await this.GetMaxIdFacturaAsync();

            foreach (var item in libros)
            {  
                Pedido ped = new Pedido
                {
                    IdPedido = nuevoId,
                    IdFactura = nuevoIdFactura,
                    Fecha = DateTime.Now,
                    IdLibro = item.IdLibro,
                    IdUsuario = idusuario,
                    Cantidad = 1
                };

                this.context.Pedidos.Add(ped);

                nuevoId++;
            }

            this.context.SaveChangesAsync();
        }

        public async Task<List<VistaPedido>> GetComprasUsuarioAsync(int idusuario)
        {
            var pedidos = await this.context.VistaPedidos
                        .Where(vp => vp.IdUsuario == idusuario)
                        .ToListAsync();
            return pedidos;
        }
    }
}
