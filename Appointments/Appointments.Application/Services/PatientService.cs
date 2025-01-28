using Appointments.Application.Exceptions;
using Appointments.Application.Models;
using Appointments.Application.Services.Abstractions;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;

namespace Appointments.Application.Services
{
    public class PatientService(
            IUnitOfWork unitOfWork,
            IMapper mapper) : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PatientModel> GetByAccountId(Guid accountId, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.PatientRepository.GetByAccountIdAsync(accountId, cancellationToken)
                ?? throw new NotFoundException($"Patient with account id: {accountId} is not found.");

            return _mapper.Map<PatientModel>(patient);
        }
    }
}
