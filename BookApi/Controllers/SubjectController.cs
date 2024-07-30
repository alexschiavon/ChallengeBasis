using Arch.Domain.Adapters.Helper;
using AutoMapper;
using BookApi.Helper;
using BookApi.Models;
using BookDomain.Filters;
using BookDomain.Helper.Exceptions;
using BookDomain.Models;
using BookDomain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{

    [Route("api/subjects")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _service;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public SubjectController(ISubjectService service, ILogger logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjects([FromQuery] Metadata<Subject, BasicFilter> metadata)
        {
            var model = await _service.FindByFilter(metadata);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById(Guid id)
        {
            var subject = await _service.FindById(id);
            if (subject == null)
                return NotFound();

            return Ok(subject);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] Subject subject)
        {
            try
            {
                if (subject.SubjectId != null && subject.SubjectId != Guid.Empty)
                    return BadRequest("Crie um objeto sem enviar o ID");

                var model = await _service.SaveOrUpdate(subject);
                return CreatedAtAction(nameof(GetSubjectById), new { id = model.SubjectId }, model);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(Guid id, [FromBody] Subject updatedSubject)
        {
            try
            {
                var subject = await _service.FindById(updatedSubject.SubjectId);
                if (subject == null)
                    return NotFound();
                await _service.SaveOrUpdate(updatedSubject);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            try
            {
                var subject = await _service.FindById(id);
                if (subject == null)
                    return NotFound();

                await _service.Delete(subject);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
