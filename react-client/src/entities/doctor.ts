import { DoctorStatus } from './enums/doctorStatus'

export default interface Doctor {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  specializationId: string,
  accountId: string,
  officeId: string,
  careerStartYear: Date,
  dateOfBirth: Date,
  Status: DoctorStatus
}