import Specialization from "../../entities/specialization";
import { useFormik } from "formik";
import { validationSchema } from "./validationSchema";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";

interface SpecializationFormProps {
  specialization: Specialization;
  onSubmit: (specialization: Specialization) => void;
  onCancel: () => void;
}

export function SpecializationForm({ specialization, onSubmit, onCancel }: SpecializationFormProps) {

  const formik = useFormik({
    initialValues: {
      id: specialization.id,
      specializationName: specialization.specializationName,
      isActive: specialization.isActive,
    },
    validationSchema,
    onSubmit: (values: Specialization) => onSubmit(values),
  })

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-[50%]">
      {/* Specialization Name Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Specialization Name"
          id="specializationName"
          name="specializationName"
          onChange={formik.handleChange}
          value={formik.values.specializationName}
        />
        {formik.touched.specializationName && formik.errors.specializationName ? (
          <div className="text-red-500">{formik.errors.specializationName}</div>
        ) : null}
      </div>

      {/* Is Active Checkbox */}
      <div className="flex items-center space-x-2">
        <input
          type="checkbox"
          id="isActive"
          name="isActive"
          checked={formik.values.isActive}
          onChange={formik.handleChange}
          className="w-5 h-5"
        />
        <label htmlFor="isActive" className="text-lg">
          Is Active
        </label>
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