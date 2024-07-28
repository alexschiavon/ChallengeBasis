using Arch.Domain.Adapters.Helper;
using AutoMapper;
using BookApi.Helper;
using BookApi.Models;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{

    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public BookController(IBookService service, ILogger logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] Metadata<Book, BasicFilter> metadata)
        {
            var model = await _service.FindByFilter(metadata);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var book = await _service.FindById(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if (book.BookId != null && book.BookId != Guid.Empty)
                return BadRequest("Crie um objeto sem enviar o ID");

            var model = await _service.SaveOrUpdate(book);
            return CreatedAtAction(nameof(GetBookById), new { id = model.BookId }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book updatedBook)
        {
            var book = await _service.FindById(updatedBook.BookId);
            if (book == null)
                return NotFound();

            book.Title = updatedBook.Title;
            // Update other properties accordingly
            await _service.SaveOrUpdate(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _service.FindById(id);
            if (book == null)
                return NotFound();

            await _service.Delete(book);
            return NoContent();
        }
    }
}
