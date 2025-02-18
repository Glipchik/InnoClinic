import * as Yup from "yup";

export const editDoctorByDoctorValidationSchema = Yup.object({
  firstName: Yup.string().required('First name is required'),
  lastName: Yup.string().required('Last name is required'),
  middleName: Yup.string().notRequired(),
  dateOfBirth: Yup.string().required('Date of birth is required'),
  careerStartYear: Yup.string().required('Career start year is required')
});
