namespace Events.Account
{
    public class AccountCreated
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required bool IsEmailVerified { get; set; }
        public required string PhotoFileName { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required Guid CreatedBy { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public required Guid UpdatedBy { get; set; }
    }
}
