import { DoctorStatus } from "../../entities/enums/doctorStatus";
import CreateAccountModelForProfile from "../accounts/CreateAccountModelForProfile";

export default interface CreateDoctorModel {
  account: CreateAccountModelForProfile,
  firstName: string,
  lastName: string,
  middleName: string | null,
  specializationId: string,
  officeId: string,
  dateOfBirth: string,
  careerStartYear: string,
  Status: DoctorStatus
}