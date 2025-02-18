export default interface EditDoctorModelByDoctor {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  careerStartYear: string,
  dateOfBirth: string,
  photo: File | null
}