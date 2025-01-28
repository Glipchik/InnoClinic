using AutoMapper;
using Documents.Application.Exceptions;
using Documents.Application.Models;
using Documents.Application.Services.Abstractions;
using Documents.Domain.Repositories.Abstractions;

namespace Documents.Application.Services
{
    public class PatientService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PatientModel> GetByAccountId(Guid accountId, CancellationToken cancellationToken)
        {
            return _mapper.Map<PatientModel>(
                await _unitOfWork.PatientRepository.GetByAccountIdAsync(accountId, cancellationToken)
                ?? throw new NotFoundException($"Patient with account id {accountId} not found."));
        }
    }
}
