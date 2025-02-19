import { useFormik } from "formik";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import { useEffect } from "react";
import Select from "../../shared/ui/forms/Select";
import Loading from "../../shared/ui/controls/Loading";
import { useOffices } from "../../shared/hooks/useOffices";
import { editReceptionistValidationSchema } from "./editReceptionistValidationSchema";
import Office from "../../entities/office";
import EditReceptionistModel from "../../models/receptionists/EditReceptionistModel";

interface EditReceptionistFormProps {
  editReceptionistModel: EditReceptionistModel;
  onSubmit: (editReceptionistModel: EditReceptionistModel) => void;
  onCancel: () => void;
  token: string | null
}

export function EditReceptionistForm({ editReceptionistModel, onSubmit, onCancel, token }: EditReceptionistFormProps) {
  const { fetchOfficesLoading, fetchOfficesError, fetchOfficesData, fetchOffices } = useOffices(token)

  useEffect(() => {
    fetchOffices()
  }, [token])

  const formik = useFormik({
    initialValues: {
      id: editReceptionistModel.id,
      firstName: editReceptionistModel.firstName,
      lastName: editReceptionistModel.lastName,
      middleName: editReceptionistModel.middleName,
      officeId: editReceptionistModel.officeId,
      photo: null
    },
    validationSchema: editReceptionistValidationSchema,
    onSubmit: (values: EditReceptionistModel) => onSubmit({...values, id: editReceptionistModel.id } as EditReceptionistModel),
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

      {/* Office Select */}
      <div className="flex flex-col">
        {fetchOfficesLoading && <Loading label="Loading offices..." />}
        {fetchOfficesError && <p className="text-red-500">Error: {fetchOfficesError}</p>}
        {fetchOfficesData && <Select
          disabled={false}
          label="Office"
          id="officeId"
          name="officeId"
          onChange={formik.handleChange}
          value={formik.values.officeId}
        >
          <option value="" label="Select office" />
          {fetchOfficesData &&
            (fetchOfficesData as Office[]).map((office: Office) => (
              <option key={office.id} value={office.id} label={office.address} />
            ))}
        </Select>}
        {formik.touched.officeId && formik.errors.officeId ? (
          <div className="text-red-500">{formik.errors.officeId}</div>
        ) : null}
      </div>

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