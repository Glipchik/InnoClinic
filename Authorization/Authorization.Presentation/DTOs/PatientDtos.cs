namespace Authorization.Presentation.DTOs
{
    public record CreatePatientForProfilesServerDto(
         CreateAccountForProfilesDto CreateAccountFromAuthDto,
         string FirstName,
         string LastName,
         string MiddleName,
         DateTime DateOfBirth);
}