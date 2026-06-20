namespace AIChatBotMicroService.DTOs
{
    public class ReturnedPatientDetailsDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = null!;

        public string IdentityUserId { get; set; }
            = null!;

        public List<MedicalRecordDto>
            MedicalRecords
        { get; set; }
                = new();

        public List<AllergyDto>
            Allergies
        { get; set; }
                = new();
    }
}
