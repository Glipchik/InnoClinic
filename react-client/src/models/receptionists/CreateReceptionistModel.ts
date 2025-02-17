export default interface CreateReceptionistModel {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  accountId: string,
  officeId: string,
}