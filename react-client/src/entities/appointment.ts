export default interface Appointment {
  id: string,
  firstName: string,
  lastName: string,
  middleName?: string,
  specializationId: string,
  accountId: string,
  officeId: string,
  careerStartYear: Date,
  dateOfBirth: Date,
  isApproved: boolean
}