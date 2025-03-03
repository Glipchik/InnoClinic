import { DoctorStatus } from "../../entities/enums/doctorStatus";

export default interface DoctorModel {
  id: string,
  firstName: string,
  lastName: string,
  middleName?: string,
  specializationId: string,
  accountId: string,
  officeId: string,
  careerStartYear: Date,
  dateOfBirth: Date,
  Status: DoctorStatus
}