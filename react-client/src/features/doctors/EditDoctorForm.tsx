import { useFormik } from "formik";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { useContext, useEffect, useState } from "react";
import { RootState } from "../../store/store";
import { useSelector } from "react-redux";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import Select from "../../shared/ui/forms/Select";
import Loading from "../../shared/ui/controls/Loading";
import Specialization from "../../entities/specialization";
import { useOffices } from "../../shared/hooks/useOffices";
import { editDoctorValidationSchema } from "./editDoctorValidationSchema";
import Office from "../../entities/office";
import { DoctorStatus } from "../../entities/enums/doctorStatus";
import DatePicker from "../../shared/ui/forms/DatePicker";
import EditDoctorModel from "../../models/doctors/EditDoctorModel";

interface EditDoctorFormProps {
  editDoctorModel: EditDoctorModel;
  onSubmit: (editDoctorModel: EditDoctorModel) => void;
  onCancel: () => void;
}

export function EditDoctorForm({ editDoctorModel, onSubmit, onCancel }: EditDoctorFormProps) {
  const [token, setToken] = useState<string | null>(null)

  const userManager = useContext(UserManagerContext)
  const { isUserAuthorized } = useSelector(
    (state: RootState) => state.auth
  );

  const { fetchSpecializationsLoading, fetchSpecializationsError, fetchSpecializationsData, fetchSpecializations } = useSpecializations(token)
  const { fetchOfficesLoading, fetchOfficesError, fetchOfficesData, fetchOffices } = useOffices(token)

  useEffect(() => {
    fetchSpecializations()
    fetchOffices()
  }, [token])

  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser()
        setToken(user?.access_token ?? null)
      }
      fetchUser()
    }
  }, [userManager, isUserAuthorized])

  const formik = useFormik({
    initialValues: {
      id: editDoctorModel.id,
      firstName: editDoctorModel.firstName,
      lastName: editDoctorModel.lastName,
      middleName: editDoctorModel.middleName,
      officeId: editDoctorModel.officeId,
      specializationId: editDoctorModel.specializationId,
      status: editDoctorModel.status,
      dateOfBirth: (new Date(editDoctorModel.dateOfBirth)).toISOString().split("T")[0],
      careerStartYear: (new Date(editDoctorModel.careerStartYear)).toISOString().split("T")[0],
      photo: null
    },
    validationSchema: editDoctorValidationSchema,
    onSubmit: (values: EditDoctorModel) => onSubmit({...values, id: editDoctorModel.id } as EditDoctorModel),
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

      {/* Specialization Select */}
      <div className="flex flex-col">
        {fetchSpecializationsLoading && <Loading label="Loading specializations..." />}
        {fetchSpecializationsError && <p className="text-red-500">Error: {fetchSpecializationsError}</p>}
        <Select
          disabled={false}
          label="Specialization"
          id="specializationId"
          name="specializationId"
          onChange={formik.handleChange}
          value={formik.values.specializationId}
        >
          <option value="" label="Select specialization" />
          {fetchSpecializationsData &&
            (fetchSpecializationsData as Specialization[]).map((spec: Specialization) => (
              <option key={spec.id} value={spec.id} label={spec.specializationName} />
            ))}
        </Select>
        {formik.touched.specializationId && formik.errors.specializationId ? (
          <div className="text-red-500">{formik.errors.specializationId}</div>
        ) : null}
      </div>

      {/* Office Select */}
      <div className="flex flex-col">
        {fetchOfficesLoading && <Loading label="Loading offices..." />}
        {fetchOfficesError && <p className="text-red-500">Error: {fetchOfficesError}</p>}
        <Select
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
        </Select>
        {formik.touched.officeId && formik.errors.officeId ? (
          <div className="text-red-500">{formik.errors.officeId}</div>
        ) : null}
      </div>

      {/* Status Select */}
      <div className="flex flex-col">
        <Select
          label="Status"
          id="status"
          name="status"
          onChange={formik.handleChange}
          value={formik.values.status}
        >
          <option value="" label="Select status" />
          {Object.keys(DoctorStatus)
            .filter((key) => isNaN(Number(key)))
            .map((key) => (
              <option key={key} value={DoctorStatus[key as keyof typeof DoctorStatus]} label={key} />
            ))}
        </Select>
        {formik.touched.status && formik.errors.status ? (
          <div className="text-red-500">{formik.errors.status}</div>
        ) : null}
      </div>

      {/* Date Picker */}
      <DatePicker
        label="Choose a career start year"
        id="careerStartYear"
        name="careerStartYear"
        value={formik.values.careerStartYear}
        onChange={formik.handleChange}
        disabled={false}
        className="bg-gray-100"
      />

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