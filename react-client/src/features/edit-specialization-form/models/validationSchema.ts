import * as Yup from "yup";

export const validationSchema = Yup.object({
  specializationName: Yup.string()
    .required("Specialization Name is required")
    .min(3, "Specialization Name must be at least 3 characters")
    .max(100, "Specialization Name must be less than 100 characters"),

  isActive: Yup.boolean()
    .required("Is active field is required")
});