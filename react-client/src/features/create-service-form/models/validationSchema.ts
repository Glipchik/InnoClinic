import * as Yup from 'yup';

export const validationSchema = Yup.object({
  serviceName: Yup.string()
    .required("Service Name is required")
    .min(3, "Service Name must be at least 3 characters")
    .max(100, "Service Name must be less than 100 characters"),

  serviceCategoryId: Yup.string().required("Service Category is required"),
  specializationId: Yup.string().required("Specialization is required"),

  price: Yup.number()
    .required("Price is required")
    .moreThan(0, "Price must be greater than 0"),

  isActive: Yup.boolean()
    .oneOf([true, false], "Status must be true or false")
    .default(false),
});