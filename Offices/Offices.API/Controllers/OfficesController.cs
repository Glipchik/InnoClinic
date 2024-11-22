using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Offices.API.DTOs;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;

namespace Offices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OfficesController(IOfficeService officeService, ILogger logger, IMapper mapper)
        {
            _officeService = officeService;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/OfficesController
        [HttpGet]
        public async Task<IEnumerable<OfficeDto>> Get(CancellationToken cancellationToken)
        {
            var offices = await _officeService.GetAll(cancellationToken);
            _logger.LogInformation("GET: api/OfficesController. Offices count: {OfficesCount}", offices.Count());
            return _mapper.Map<IEnumerable<OfficeDto>>(offices);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
