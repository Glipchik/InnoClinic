using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Offices.API.DTOs;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;

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

        // GET: api/Offices
        [HttpGet]
        public async Task<IEnumerable<OfficeDto>> Get(CancellationToken cancellationToken)
        {
            var offices = await _officeService.GetAll(cancellationToken);
            _logger.LogInformation("Requested offices list");
            return _mapper.Map<IEnumerable<OfficeDto>>(offices);
        }

        // GET api/Offices/5
        [HttpGet("{id}")]
        public async Task<OfficeDto> Get(string id, CancellationToken cancellationToken)
        {
            var office = await _officeService.Get(id, cancellationToken);
            _logger.LogInformation("Requested office with id {id}", id);
            return _mapper.Map<OfficeDto>(office);
        }

        //// POST api/<ValuesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
