import * as Yup from "yup";

export const editReceptionistValidationSchema = Yup.object({
  firstName: Yup.string().required('First name is required'),
  lastName: Yup.string().required('Last name is required'),
  middleName: Yup.string().notRequired(),
  officeId: Yup.string().required('Office is required')
});
