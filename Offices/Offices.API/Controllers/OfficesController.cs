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
        private readonly ILogger<OfficesController> _logger;
        private readonly IMapper _mapper;

        public OfficesController(IOfficeService officeService, ILogger<OfficesController> logger, IMapper mapper)
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

        // POST api/Offices
        [HttpPost]
        public async void Post([FromBody] CreateOfficeDto createOfficeDto, CancellationToken cancellationToken)
        {
            var officeModel = _mapper.Map<CreateOfficeModel>(createOfficeDto);
            await _officeService.Create(officeModel, cancellationToken);
            _logger.LogInformation("New office was successfully created");
        }

        // PUT api/Offices
        [HttpPut]
        public async void Put([FromBody] UpdateOfficeDto updateOfficeDto, CancellationToken cancellationToken)
        {
            var officeModel = _mapper.Map<UpdateOfficeModel>(updateOfficeDto);
            await _officeService.Update(officeModel, cancellationToken);
            _logger.LogInformation("Office with id {id} was successfully updated", updateOfficeDto.Id);
        }

        // DELETE api/Offices/5
        [HttpDelete("{id}")]
        public async void Delete(string id, CancellationToken cancellationToken)
        {
            await _officeService.Delete(id, cancellationToken);
            _logger.LogInformation("Office with id {id} was successfully deleted", id);
        }
    }
}
