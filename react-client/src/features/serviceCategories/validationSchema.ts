import * as Yup from "yup";

export const validationSchema = Yup.object({
  categoryName: Yup.string()
    .required("Category Name is required")
    .min(3, "Address must be at least 3 characters"),
  
  timeSlotSize: Yup.string()
    .required("Time Slot Size is required")
});