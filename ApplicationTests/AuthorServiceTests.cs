using BookApplication.Services;
using BookDomain.Models;
using BookDomain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using BookDataAccessAdapter.Context;
namespace ApplicationTests
{
    public class TestLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // No-op
        }
    }

    public class TestLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider) { }

        public ILogger CreateLogger(string categoryName) => new TestLogger<object>();

        public void Dispose() { }
    }
    public class AuthorServiceTests
    {

        private DbContextOptions<BookContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        private AuthorService CreateService(out Mock<IAuthorRepository> repositoryMock, out Mock<IBookRepository> repositoryBookMock)
        {
            repositoryMock = new Mock<IAuthorRepository>();
            repositoryBookMock = new Mock<IBookRepository>();
            var loggerFactory = new TestLoggerFactory();

            return new AuthorService(repositoryMock.Object, loggerFactory, repositoryBookMock.Object);
        }

        [Fact]
        public async Task Delete_AuthorExists_ReturnsTrue()
        {
            // Arrange
            var author = new Author { AuthorId = Guid.NewGuid() };
            var service = CreateService(out var repositoryMock, out var repositoryBookMock);
            repositoryMock.Setup(r => r.FindById(author.AuthorId)).ReturnsAsync(author);

            // Act
            var result = await service.Delete(author);

            // Assert
            Assert.True(result);
            repositoryMock.Verify(r => r.Delete(author), Times.Once);
        }

        [Fact]
        public async Task Delete_AuthorDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var author = new Author { AuthorId = Guid.NewGuid() };
            var service = CreateService(out var repositoryMock, out var repositoryBookMock);
            repositoryMock.Setup(r => r.FindById(author.AuthorId)).ReturnsAsync((Author)null);

            // Act
            var result = await service.Delete(author);

            // Assert
            Assert.False(result);
            repositoryMock.Verify(r => r.Delete(It.IsAny<Author>()), Times.Never);
        }

        [Fact]
        public async Task FindById_AuthorExists_ReturnsAuthor()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { AuthorId = authorId };
            var service = CreateService(out var repositoryMock, out var repositoryBookMock);
            repositoryMock.Setup(r => r.FindById(authorId)).ReturnsAsync(author);

            // Act
            var result = await service.FindById(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorId, result.AuthorId);
        }

        [Fact]
        public async Task FindById_AuthorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var service = CreateService(out var repositoryMock, out var repositoryBookMock);
            repositoryMock.Setup(r => r.FindById(authorId)).ReturnsAsync((Author)null);

            // Act
            var result = await service.FindById(authorId);

            // Assert
            Assert.Null(result);
        }
    }
}