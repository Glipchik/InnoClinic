export default interface CreatePatientModel {
  firstName: string,
  lastName: string,
  middleName: string | null,
  accountId: string,
  dateOfBirth: Date
}