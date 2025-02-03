using AutoMapper;
using Documents.Application.Exceptions;
using Documents.Application.Models;
using Documents.Application.Services.Abstractions;
using Documents.Domain.Repositories.Abstractions;

namespace Documents.Application.Services
{
    public class DoctorService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<DoctorModel> GetByAccountId(Guid accountId, CancellationToken cancellationToken)
        {
            return _mapper.Map<DoctorModel>(
                await _unitOfWork.DoctorRepository.GetByAccountIdAsync(accountId, cancellationToken)
                ?? throw new NotFoundException($"Doctor with account id {accountId} not found."));
        }
    }
}
