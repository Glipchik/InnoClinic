namespace Appointments.Consumers.Mapper
{
    public partial class ConsumersMapping
    {
        partial void AddDoctorMapping();
        partial void AddServiceMapping();
        partial void AddSpecializationMapping();
        partial void AddServiceCategoryMapping();
        partial void AddPatientMapping();

        public ConsumersMapping()
        {
            AddDoctorMapping();
            AddServiceMapping();
            AddSpecializationMapping();
            AddServiceCategoryMapping();
            AddPatientMapping();
        }
    }
}
