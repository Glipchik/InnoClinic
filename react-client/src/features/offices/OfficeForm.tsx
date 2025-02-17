import { useFormik } from "formik";
import { validationSchema } from "./validationSchema";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import CreateOfficeModel from "../../models/offices/CreateOfficeModel";

interface OfficeFormProps {
  createOfficeModel: CreateOfficeModel;
  onSubmit: (createOfficeModel: CreateOfficeModel) => void;
  onCancel: () => void;
}

export function OfficeForm({ createOfficeModel, onSubmit, onCancel }: OfficeFormProps) {

  const formik = useFormik({
    initialValues: {
      address: createOfficeModel.address,
      registryPhoneNumber: createOfficeModel.registryPhoneNumber,
      isActive: createOfficeModel.isActive,
    },
    validationSchema,
    onSubmit: (values: CreateOfficeModel) => onSubmit(values),
  })

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-[50%]">
      {/* Address Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Address"
          id="address"
          name="address"
          onChange={formik.handleChange}
          value={formik.values.address}
        />
        {formik.touched.address && formik.errors.address ? (
          <div className="text-red-500">{formik.errors.address}</div>
        ) : null}
      </div>

      {/* Registry Phone Number Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Registry Phone Number"
          id="registryPhoneNumber"
          name="registryPhoneNumber"
          onChange={formik.handleChange}
          value={formik.values.registryPhoneNumber}
        />
        {formik.touched.registryPhoneNumber && formik.errors.registryPhoneNumber ? (
          <div className="text-red-500">{formik.errors.registryPhoneNumber}</div>
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