 using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace WebApiAutores.Entities
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("security");

            builder.Entity<IdentityUser>().ToTable("users");
            builder.Entity<IdentityRole>().ToTable("roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("users_roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("users_claims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("users_logins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("roles_claims");
            builder.Entity<IdentityUserToken<string>>().ToTable("users_tokens");

            builder.Entity<Book>().HasIndex(x => x.ISBN).IsUnique(true);

            builder.Entity<Comentario>().HasOne(c => c.Usuario).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Comentario>().HasOne(c => c.Review).WithMany(r => r.Comentarios).HasForeignKey(c => c.ReviewId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Review>().HasOne(r => r.Book).WithMany().HasForeignKey(r => r.BookId);

            builder.Entity<RespuestaComentario>().HasOne(rc => rc.Usuario).WithMany().HasForeignKey(rc => rc.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<RespuestaComentario>().HasOne(rc => rc.Comentario).WithMany(c => c.Respuestas).HasForeignKey(rc => rc.ComentarioId).OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<RespuestaComentario> RespuestaComentarios { get; set; }
    }
}
