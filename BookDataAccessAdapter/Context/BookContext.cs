using BookDomain.Models;
using BookDomain.Models.Report;
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
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<BookSubject> BooksSubjects { get; set; }
        public DbSet<PurchaseType> PurchaseTypes { get; set; }
        public DbSet<BookDetails> BookDetails { get; set; }


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
                entity.HasMany(e => e.BookAuthors)
                  .WithOne()
                  .HasForeignKey(ba => ba.BookCodl)
                  .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.BookSubjects)
                      .WithOne()
                      .HasForeignKey(bs => bs.BookCodl)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.BookPrices)
                      .WithOne()
                      .HasForeignKey(bp => bp.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity<BookAuthor>(
                entity =>
                {
                    entity.HasKey(la => new { la.BookCodl, la.AuthorCodAu });
                    entity.ToTable("livro_autor");
                    entity.Property(e => e.BookCodl).HasColumnName("livro_codl");
                    entity.Property(e => e.AuthorCodAu).HasColumnName("autor_codau");
                });

            modelBuilder.Entity<BookSubject>(
               entity =>
               {
                   entity.HasKey(la => new { la.BookCodl, la.SubjectCodAs });
                   entity.ToTable("livro_assunto");
                   entity.Property(e => e.BookCodl).HasColumnName("livro_codl");
                   entity.Property(e => e.SubjectCodAs).HasColumnName("assunto_codas");
               });

            modelBuilder.Entity<PurchaseType>(entity =>
            {
                entity.ToTable("tipo_compra");
                entity.HasKey(e => e.PurchaseTypeId);
                entity.Property(e => e.PurchaseTypeId).HasColumnName("codc");
                entity.Property(e => e.Name).HasColumnName("nome").IsRequired().HasMaxLength(40);
            });

            modelBuilder.Entity<BookPrice>(entity =>
            {
                entity.ToTable("preco_livro");
                entity.HasKey(e => new { e.BookId, e.PurchaseTypeId });
                entity.Property(e => e.BookId).HasColumnName("livro_codl");
                entity.Property(e => e.PurchaseTypeId).HasColumnName("tipo_codc");
                entity.Property(e => e.Price).HasColumnName("preco").HasColumnType("decimal(18, 2)");

            });

            modelBuilder.Entity<BookDetails>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_livrodetalhes");
                entity.Property(e => e.BookId).HasColumnName("livro_id");
                entity.Property(e => e.BookTitle).HasColumnName("livro_titulo");
                entity.Property(e => e.Publisher).HasColumnName("editora");
                entity.Property(e => e.Edition).HasColumnName("edicao");
                entity.Property(e => e.PublicationYear).HasColumnName("ano_publicacao");
                entity.Property(e => e.Authors).HasColumnName("autores");
                entity.Property(e => e.Subjects).HasColumnName("assuntos");
                entity.Property(e => e.Prices).HasColumnName("precos");
            });

        }
    }
}
