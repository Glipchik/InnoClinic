import { useFormik } from "formik";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import { editPatientValidationSchema } from "./editPatientValidationSchema";
import DatePicker from "../../shared/ui/forms/DatePicker";
import EditPatientModel from "../../models/patients/EditPatientModel";

interface EditPatientFormProps {
  editPatientModel: EditPatientModel;
  onSubmit: (editPatientModel: EditPatientModel) => void;
  onCancel: () => void;
}

export function EditPatientForm({ editPatientModel, onSubmit, onCancel }: EditPatientFormProps) {

  const formik = useFormik({
    initialValues: {
      id: editPatientModel.id,
      firstName: editPatientModel.firstName,
      lastName: editPatientModel.lastName,
      middleName: editPatientModel.middleName,
      dateOfBirth: editPatientModel.dateOfBirth,
      photo: null
    },
    validationSchema: editPatientValidationSchema,
    onSubmit: (values: EditPatientModel) => onSubmit(values),
  })

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-full">
      {/* First Name Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="First Name"
          id="firstName"
          name="firstName"
          onChange={formik.handleChange}
          value={formik.values.firstName}
        />
        {formik.touched.firstName && formik.errors.firstName ? (
          <div className="text-red-500">{formik.errors.firstName}</div>
        ) : null}
      </div>

      {/* Last Name Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Last Name"
          id="lastName"
          name="lastName"
          onChange={formik.handleChange}
          value={formik.values.lastName}
        />
        {formik.touched.lastName && formik.errors.lastName ? (
          <div className="text-red-500">{formik.errors.lastName}</div>
        ) : null}
      </div>

      {/* Middle Name Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Middle Name"
          id="middleName"
          name="middleName"
          onChange={formik.handleChange}
          value={formik.values.middleName ?? ""}
        />
        {formik.touched.middleName && formik.errors.middleName ? (
          <div className="text-red-500">{formik.errors.middleName}</div>
        ) : null}
      </div>

      {/* Date Picker */}
      <DatePicker
        label="Choose a date of birth"
        id="dateOfBirth"
        name="dateOfBirth"
        value={formik.values.dateOfBirth}
        onChange={formik.handleChange}
        disabled={false}
        className="bg-gray-100"
      />

      {/* Photo Upload Input */}
      <div className="flex flex-col">
        <label htmlFor="photo" className="font-semibold">Upload Photo</label>
        <input
          type="file"
          id="photo"
          name="photo"
          accept="image/*"
          onChange={(event) => {
            formik.setFieldValue("photo", event.currentTarget.files?.[0] ?? null);
          }}
        />
        {formik.touched.photo && formik.errors.photo ? (
          <div className="text-red-500">{formik.errors.photo}</div>
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