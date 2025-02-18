export default interface EditReceptionistModel {
  id: string,
  firstName: string,
  lastName: string,
  middleName: string | null,
  officeId: string,
  photo: File | null
}