using Arch.Domain.Adapters.Helper;
using AutoMapper;
using BookApi.Helper;
using BookApi.Models;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookApi.Controllers
{

    [Route("api/purchase-types")]
    [ApiController]
    public class PurchaseTypeController : ControllerBase
    {
        private readonly IPurchaseTypeService _service;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public PurchaseTypeController(IPurchaseTypeService service, ILogger logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseTypes([FromQuery] Metadata<PurchaseType, BasicFilter> metadata)
        {
            var model = await _service.FindByFilter(metadata);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseTypeById(Guid id)
        {
            var PurchaseType = await _service.FindById(id);
            if (PurchaseType == null)
                return NotFound();

            return Ok(PurchaseType);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseType([FromBody] PurchaseType PurchaseType)
        {
            try
            {
                if (PurchaseType.PurchaseTypeId != null && PurchaseType.PurchaseTypeId != Guid.Empty)
                    return BadRequest("Crie um objeto sem enviar o ID");

                var model = await _service.SaveOrUpdate(PurchaseType);
                return CreatedAtAction(nameof(GetPurchaseTypeById), new { id = model.PurchaseTypeId }, model);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseType(Guid id, [FromBody] PurchaseType updatedPurchaseType)
        {
            try
            {
                await _service.SaveOrUpdate(updatedPurchaseType);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseType(Guid id)
        {
            var PurchaseType = await _service.FindById(id);
            if (PurchaseType == null)
                return NotFound();

            await _service.Delete(PurchaseType);
            return NoContent();
        }
    }
}
