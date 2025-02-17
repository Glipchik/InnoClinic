export default interface EditDoctorModelByDoctor {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  careerStartYear: Date,
  dateOfBirth: Date
}