import Account from "../../entities/account";
import Office from "../../entities/office";

export default interface Receptionist {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  account: Account,
  office: Office,
}