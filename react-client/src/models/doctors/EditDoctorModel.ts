import { DoctorStatus } from "../../entities/enums/doctorStatus";

export default interface EditDoctorModel {
  id: string;
  firstName: string,
  lastName: string,
  middleName: string | null,
  specializationId: string,
  officeId: string,
  dateOfBirth: string,
  careerStartYear: string,
  status: DoctorStatus,
  photo: File | null
}