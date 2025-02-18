import * as Yup from "yup";

export const editDoctorValidationSchema = Yup.object({
  firstName: Yup.string().required('First name is required'),
  lastName: Yup.string().required('Last name is required'),
  middleName: Yup.string().notRequired(),
  specializationId: Yup.string().required('Specialization is required'),
  officeId: Yup.string().required('Office is required'),
  dateOfBirth: Yup.string().required('Date of birth is required'),
  careerStartYear: Yup.string().required('Career start year is required'),
  status: Yup.string().required('Status is required')
});
