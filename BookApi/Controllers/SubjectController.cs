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
            if (subject.SubjectId != null && subject.SubjectId != Guid.Empty)
                return BadRequest("Crie um objeto sem enviar o ID");

            var model = await _service.SaveOrUpdate(subject);
            return CreatedAtAction(nameof(GetSubjectById), new { id = model.SubjectId }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(Guid id, [FromBody] Subject updatedSubject)
        {
            var subject = await _service.FindById(updatedSubject.SubjectId);
            if (subject == null)
                return NotFound();

            subject.Description = updatedSubject.Description;
            // Update other properties accordingly
            await _service.SaveOrUpdate(subject);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var subject = await _service.FindById(id);
            if (subject == null)
                return NotFound();

            await _service.Delete(subject);
            return NoContent();
        }
    }
}
