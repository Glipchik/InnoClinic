import * as Yup from "yup";

export const validationSchema = Yup.object({
  specializationName: Yup.string()
    .required("Specialization Name is required")
    .min(3, "Specialization Name must be at least 3 characters"),

  isActive: Yup.boolean()
    .oneOf([true, false], "Status must be true or false")
    .default(false),
});