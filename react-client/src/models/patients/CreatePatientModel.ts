import CreateAccountModelForProfile from "../accounts/CreateAccountModelForProfile";

export default interface CreatePatientModel {
  firstName: string,
  lastName: string,
  middleName: string | null,
  account: CreateAccountModelForProfile,
  dateOfBirth: string,
  photo: File | null
}