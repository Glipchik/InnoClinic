import Account from '../../entities/account'
import { DoctorStatus } from '../../entities/enums/doctorStatus'
import Office from '../../entities/office'
import Specialization from '../../entities/specialization'

export default interface DoctorModel {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  specialization: Specialization,
  accountId: Account,
  officeId: Office,
  careerStartYear: Date,
  dateOfBirth: Date,
  Status: DoctorStatus
}