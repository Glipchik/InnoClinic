import * as Yup from "yup";

export const validationSchema = Yup.object({
  address: Yup.string()
    .required("Address is required")
    .min(3, "Address must be at least 3 characters")
    .max(100, "Address must be less than 100 characters"),
  
  registryPhoneNumber: Yup.string()
    .required("Phone number is required")
    .min(5, "Phone number must be at least 5 characters")
    .max(20, "Phone must be less than 20 characters"),

  isActive: Yup.boolean()
    .oneOf([true, false], "Status must be true or false")
    .default(false),
});