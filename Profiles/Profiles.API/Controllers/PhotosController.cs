using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profiles.API.DTOs;
using Profiles.Application.Services;
using Profiles.Application.Services.Abstractions;

namespace Profiles.API.Controllers
{
    /// <summary>
    /// Controller for managing files.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IAccountService _accountService;
        private readonly ILogger<DoctorsController> _logger;

        public PhotosController(IFileService fileService, IAccountService accountService, ILogger<DoctorsController> logger)
        {
            _fileService = fileService;
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// Gets url of photo by account id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Photos/1
        ///
        /// </remarks>
        /// <returns>Returns an url of photo.</returns>
        /// <response code="200">Returns the photo</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        public async Task<string> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Requested photo");
            var accountModel = await _accountService.Get(Guid.Parse(id), cancellationToken);
            return await _fileService.GetFileUrl(accountModel.PhotoFileName);
        }
    }
}
