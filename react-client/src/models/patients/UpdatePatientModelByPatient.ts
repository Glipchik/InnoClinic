export default interface Patient {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  dateOfBirth: Date
}