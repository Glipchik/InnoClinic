import CreateAccountModelForProfile from "../accounts/CreateAccountModelForProfile";

export default interface CreateReceptionistModel {
  firstName: string,
  lastName: string,
  middleName: string | null,
  account: CreateAccountModelForProfile,
  officeId: string,
  photo: File | null
}