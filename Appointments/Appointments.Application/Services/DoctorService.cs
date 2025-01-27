using Appointments.Application.Exceptions;
using Appointments.Application.Models;
using Appointments.Application.Services.Abstractions;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;

namespace Appointments.Application.Services
{
    public class DoctorService(
            IUnitOfWork unitOfWork,
            IMapper mapper) : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<DoctorModel> GetByAccountId(Guid accountId, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.DoctorRepository.GetByAccountIdAsync(accountId, cancellationToken)
                ?? throw new NotFoundException($"Doctor with account id: {accountId} is not found.");

            return _mapper.Map<DoctorModel>(doctor);
        }
    }
}
