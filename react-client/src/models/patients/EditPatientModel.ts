export default interface EditPatientModel {
  id: string;
  firstName: string,
  lastName: string,
  middleName: string | null,
  dateOfBirth: string,
  photo: File | null
}