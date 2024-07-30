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

    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public ReportController(IBookService service, ILogger logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportBooks()
        {
            var model = await _service.Report();
            return Ok(model);
        }
    }
}
