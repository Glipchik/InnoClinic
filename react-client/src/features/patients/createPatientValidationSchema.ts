import * as Yup from "yup";

export const createPatientValidationSchema = Yup.object({
  firstName: Yup.string().required('First name is required'),
  lastName: Yup.string().required('Last name is required'),
  middleName: Yup.string(),
  dateOfBirth: Yup.string().required('Date of birth is required'),
  account: Yup.object({
    email: Yup.string().email('Invalid email address').required('Email is required'),
    phoneNumber: Yup.string().required('Phone number is required'),
  }),
});
