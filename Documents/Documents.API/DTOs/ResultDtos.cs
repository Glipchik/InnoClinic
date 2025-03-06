namespace Documents.API.DTOs
{
    public record ResultDto(
        Guid Id,
        Guid AppointmentId,
        string Complaints,
        string Conclusion,
        string Recommendations
    );

    public record CreateResultDto(
        Guid AppointmentId,
        string Complaints,
        string Conclusion,
        string Recommendations
    );

    public record UpdateResultDto(
        Guid Id,
        string Complaints,
        string Conclusion,
        string Recommendations
    );
}
