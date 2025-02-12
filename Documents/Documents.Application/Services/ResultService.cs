using AutoMapper;
using Documents.Application.Exceptions;
using Documents.Application.Models;
using Documents.Application.Services.Abstractions;
using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Documents.Application.Services
{
    public class ResultService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmailService emailService) : IResultService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;

        public async Task<ResultModel> Create(CreateResultModel createResultModel, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetAsync(createResultModel.AppointmentId, cancellationToken)
                ?? throw new RelatedObjectNotFoundException($"Appointment with id {createResultModel.AppointmentId} is not found.");

            if (appointment.IsApproved == false)
            {
                throw new BadRequestException("Appointment is not approved.");
            }

            if (appointment.Result != null)
            {
                throw new BadRequestException("Result is already created for this appointment.");
            }

            var resultToCreate = _mapper.Map<Result>(createResultModel);
            resultToCreate.Id = Guid.NewGuid();

            var resultModel = _mapper.Map<ResultModel>(await _unitOfWork.ResultRepository.CreateAsync(resultToCreate, cancellationToken));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return resultModel;
        }

        public async Task<ResultModel> Delete(Guid id, CancellationToken cancellationToken)
        {
            var resultToDelete = await _unitOfWork.ResultRepository.GetAsync(id, cancellationToken)
                ?? throw new NotFoundException($"Result with id {id} is not found.");

            await _unitOfWork.ResultRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ResultModel>(resultToDelete);
        }

        public async Task<byte[]> GeneratePdf(Guid resultId, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ResultRepository.GetAsync(resultId, cancellationToken)
                ?? throw new NotFoundException($"Result with id {resultId} is not found.");

            var text =
                $"Result: {result.Appointment.Service} + {result.Appointment.Date} + {result.Appointment.Time} \nComplaints: {result.Complaints}\nConclusion: {result.Conclusion}\nRecommendations: {result.Recommendations}";

            return await PdfGenerator.GenerateFile(text, cancellationToken); 
        }

        public async Task<ResultModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ResultModel>(await _unitOfWork.ResultRepository.GetAsync(id, cancellationToken)
                ?? throw new NotFoundException($"Result with id {id} is not found."));
        }

        public async Task<IEnumerable<ResultModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ResultModel>>(await _unitOfWork.ResultRepository.GetAllAsync(cancellationToken));
        }

        public async Task<IEnumerable<ResultModel>> GetAllByDoctorId(Guid doctorId, CancellationToken cancellationToken)
        {

            return _mapper.Map<IEnumerable<ResultModel>>(await _unitOfWork.ResultRepository.GetAllByDoctorIdAsync(doctorId, cancellationToken));
        }

        public async Task<IEnumerable<ResultModel>> GetAllByPatientId(Guid patientId, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ResultModel>>(await _unitOfWork.ResultRepository.GetAllByPatientIdAsync(patientId, cancellationToken));
        }

        public async Task<ResultModel> SendResultByEmail(Guid id, string email, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ResultRepository.GetAsync(id, cancellationToken)
                ?? throw new NotFoundException($"Result with id {id} is not found.");

            await _emailService.SendEmailAsync(email,
                "Result",
                $"Complaints: {result.Complaints}\nConclusion: {result.Conclusion}\nRecommendations: {result.Recommendations}", cancellationToken);

            return _mapper.Map<ResultModel>(result);
        }

        public async Task<ResultModel> Update(UpdateResultModel updateResultModel, CancellationToken cancellationToken)
        {
            _ = await _unitOfWork.ResultRepository.GetAsync(updateResultModel.Id, cancellationToken)
                ?? throw new NotFoundException($"Result with id {updateResultModel.Id} is not found.");

            var resultModel = _mapper.Map<ResultModel>(_unitOfWork.ResultRepository.Update(_mapper.Map<Result>(updateResultModel), cancellationToken));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return resultModel;
        }
    }
}
