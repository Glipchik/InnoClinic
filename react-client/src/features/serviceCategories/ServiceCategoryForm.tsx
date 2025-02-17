import { useFormik } from "formik";
import { validationSchema } from "./validationSchema";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import CreateServiceCategoryModel from "../../models/serviceCategories/createServiceCategoryModel";

interface ServiceCategoryFormProps {
  createServiceCategoryModel: CreateServiceCategoryModel;
  onSubmit: (createServiceCategoryModel: CreateServiceCategoryModel) => void;
  onCancel: () => void;
}

export function ServiceCategoryForm({ createServiceCategoryModel, onSubmit, onCancel }: ServiceCategoryFormProps) {
  const formik = useFormik({
    initialValues: {
      categoryName: createServiceCategoryModel.categoryName,
      timeSlotSize: createServiceCategoryModel.timeSlotSize,
    },
    validationSchema,
    onSubmit: (values: CreateServiceCategoryModel) => {
      const hours = Math.floor(Number(values.timeSlotSize) / 60);
      const minutes = Number(values.timeSlotSize) % 60;
      values.timeSlotSize = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:00`;
      onSubmit(values);
    },
  })

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-[50%]">
      {/* Service Category Name Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Service Category Name"
          id="categoryName"
          name="categoryName"
          onChange={formik.handleChange}
          value={formik.values.categoryName}
        />
        {formik.touched.categoryName && formik.errors.categoryName ? (
          <div className="text-red-500">{formik.errors.categoryName}</div>
        ) : null}
      </div>

      {/* Time Slot Size */}
      <div className="flex flex-col">
        <label htmlFor="timeSloSize" className="text-lg">
          Time Slot Size (in minutes)
        </label>
        <input
          type="range"
          id="timeSlotSize"
          name="timeSlotSize"
          min="0"
          max="120"
          step="5"
          value={formik.values.timeSlotSize}
          onChange={formik.handleChange}
          className="w-full"
        />
        <div className="text-center mt-2">
          {`00:${formik.values.timeSlotSize.toString().padStart(2, '0')}:00`}
        </div>
        {formik.touched.timeSlotSize && formik.errors.timeSlotSize ? (
          <div className="text-red-500">{formik.errors.timeSlotSize}</div>
        ) : null}
      </div>

      {/* Submit and Cancel Buttons */}
      <div className="flex space-x-2">
        <Button type="submit" className="w-full bg-green-500 hover:bg-green-600">
          Save
        </Button>
        <Button type="button" onClick={onCancel} className="w-full bg-gray-500 hover:bg-gray-600">
          Cancel
        </Button>
      </div>
    </form>
  )
}