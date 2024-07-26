using BookDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookDataAccessAdapter.Context
{

    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("BookDatabase");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Livro");
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.BookId).HasColumnName("codl");
                entity.Property(e => e.Title).HasColumnName("titulo").HasMaxLength(40).IsRequired();
                entity.Property(e => e.Publisher).HasColumnName("editora").HasMaxLength(40).IsRequired();
                entity.Property(e => e.EditionId).HasColumnName("edicao").IsRequired();
                entity.Property(e => e.PublicationYear).HasColumnName("anoPublicacao").HasMaxLength(4).IsRequired();
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Assunto");
                entity.HasKey(e => e.SubjectId);
                entity.Property(e => e.SubjectId).HasColumnName("codAs");
                entity.Property(e => e.Description).HasColumnName("descricao").HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Autor");
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.AuthorId).HasColumnName("codAu");
                entity.Property(e => e.Name).HasColumnName("nome").HasMaxLength(40).IsRequired();
            });
        }
    }
}
