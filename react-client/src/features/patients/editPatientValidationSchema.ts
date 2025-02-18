import * as Yup from "yup";

export const editPatientValidationSchema = Yup.object({
  firstName: Yup.string().required('First name is required'),
  lastName: Yup.string().required('Last name is required'),
  middleName: Yup.string().notRequired(),
  dateOfBirth: Yup.string().required('Date of birth is required')
});
