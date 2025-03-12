import * as Yup from "yup";

export const validationSchema = Yup.object({
  address: Yup.string()
    .required("Address is required")
    .min(3, "Address must be at least 3 characters")
    .max(30, "Address must be less than 30 characters"),

  registryPhoneNumber: Yup.string()
    .required("Phone number is required")
    .matches(
      /^\+375[0-9]{9}$/,
      "Phone number must start with +375 and contain exactly 9 digits after"
    )
    .min(13, "Phone number must be exactly 13 characters (including +375)")
    .max(13, "Phone number must be exactly 13 characters (including +375)"),

  isActive: Yup.boolean()
    .required("Is active field is required"),
});