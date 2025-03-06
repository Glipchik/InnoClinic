namespace Results.MessageBroking.Mapper
{
    public partial class ConsumersMapping
    {
        partial void AddDoctorMapping();
        partial void AddServiceMapping();
        partial void AddSpecializationMapping();
        partial void AddServiceCategoryMapping();
        partial void AddPatientMapping();
        partial void AddAccountMapping();
        partial void AddAppointmentMapping();

        public ConsumersMapping()
        {
            AddDoctorMapping();
            AddServiceMapping();
            AddSpecializationMapping();
            AddServiceCategoryMapping();
            AddPatientMapping();
            AddAccountMapping();
            AddAppointmentMapping();
        }
    }
}
