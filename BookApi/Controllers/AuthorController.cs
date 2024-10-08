﻿using Arch.Domain.Adapters.Helper;
using AutoMapper;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookApi.Controllers
{

    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;
        private readonly IBookService _bookService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public AuthorController(IAuthorService service, ILogger logger, IMapper mapper, IBookService bookService)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
            _bookService = bookService;
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
            try
            {
                if (author.AuthorId != null && author.AuthorId != Guid.Empty)
                    return BadRequest("Crie um objeto sem enviar o ID");

                var model = await _service.SaveOrUpdate(author);
                return CreatedAtAction(nameof(GetAuthorById), new { id = model.AuthorId }, model);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] Author updatedAuthor)
        {
            try
            {
                var author = await _service.FindById(updatedAuthor.AuthorId);
                if (author == null)
                    return NotFound();

                author.Name = updatedAuthor.Name;
                // Update other properties accordingly
                await _service.SaveOrUpdate(author);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                var author = await _service.FindById(id);
                if (author == null)
                    return NotFound();

                await _service.Delete(author);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{authorId}/books")]
        public async Task<IActionResult> GetAuthorBooks(Guid authorId)
        {
            Metadata<Book, BookFilter> metadataBooks = await _bookService.FindByFilter(new Metadata<Book, BookFilter> { Custom = new BookFilter { AuthorId = authorId.ToString() } });
            return Ok(metadataBooks);
        }
    }
}
