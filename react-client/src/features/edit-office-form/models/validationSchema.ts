import * as Yup from "yup";

export const validationSchema = Yup.object({
  address: Yup.string()
    .required("Address is required")
    .min(3, "Address must be at least 3 characters")
    .max(30, "Address must be less than 30 characters"),

  registryPhoneNumber: Yup.string()
    .required("Phone number is required")
    .matches(
      /^\+?[0-9]{5,30}$/,
      "Phone number must be valid and contain only digits, optionally starting with a '+'"
    )
    .min(5, "Phone number must be at least 5 characters")
    .max(30, "Phone number must be less than 30 characters"),

  isActive: Yup.boolean()
    .required("Is active field is required"),
});