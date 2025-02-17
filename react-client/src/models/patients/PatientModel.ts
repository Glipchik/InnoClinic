import Account from "../../entities/account";

export default interface Patient {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  dateOfBirth: Date,
  account: Account,
}