using Arch.Domain.Adapters.Helper;
using AutoMapper;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{

    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public AuthorController(IAuthorService service, ILogger logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery] Metadata<Author, BasicFilter> metadata)
        {
            var model = await _service.FindByFilter(metadata);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            var author = await _service.FindById(id);
            if (author == null)
                return NotFound();

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] Author author)
        {
            if (author.AuthorId != null && author.AuthorId != Guid.Empty)
                return BadRequest("Crie um objeto sem enviar o ID");

            var model = await _service.SaveOrUpdate(author);
            return CreatedAtAction(nameof(GetAuthorById), new { id = model.AuthorId }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] Author updatedAuthor)
        {
            var author = await _service.FindById(updatedAuthor.AuthorId);
            if (author == null)
                return NotFound();

            author.Name = updatedAuthor.Name;
            // Update other properties accordingly
            await _service.SaveOrUpdate(author);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var author = await _service.FindById(id);
            if (author == null)
                return NotFound();

            await _service.Delete(author);
            return NoContent();
        }
    }
}
