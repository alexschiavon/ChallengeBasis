using BookDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookDataAccessAdapter.Context
{

    public class BookContext : DbContext
    {

        public BookContext(DbContextOptions<BookContext> options)
           : base(options)
        {
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Author> Authors { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseInMemoryDatabase("BookDatabase");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("livro");
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.BookId).HasColumnName("codl");
                entity.Property(e => e.Title).HasColumnName("titulo").HasMaxLength(40).IsRequired();
                entity.Property(e => e.Publisher).HasColumnName("editora").HasMaxLength(40).IsRequired();
                entity.Property(e => e.Edition).HasColumnName("edicao").IsRequired();
                entity.Property(e => e.PublicationYear).HasColumnName("anopublicacao").HasMaxLength(4).IsRequired();
                entity.HasData(
                   new Book { BookId = Guid.NewGuid(), Title = "Book One", Publisher = "Publisher One", Edition = 1, PublicationYear = "2020" },
                   new Book { BookId = Guid.NewGuid(), Title = "Book Two", Publisher = "Publisher Two", Edition = 2, PublicationYear = "2021" },
                   new Book { BookId = Guid.NewGuid(), Title = "Book Three", Publisher = "Publisher Three", Edition = 3, PublicationYear = "2022" }
               );
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("assunto");
                entity.HasKey(e => e.SubjectId);
                entity.Property(e => e.SubjectId).HasColumnName("codas");
                entity.Property(e => e.Description).HasColumnName("descricao").HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("autor");
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.AuthorId).HasColumnName("codau");
                entity.Property(e => e.Name).HasColumnName("nome").HasMaxLength(40).IsRequired();
            });
        }
    }
}
