﻿using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2Acl.Models;

namespace PracticaMvcCore2Acl.Data
{
    public class LibrosContext: DbContext
    {
        public LibrosContext(DbContextOptions<LibrosContext> options)
            : base(options)
        { }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<VistaPedido> VistaPedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
