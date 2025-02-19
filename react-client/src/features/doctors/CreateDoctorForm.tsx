import { useFormik } from "formik";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { useEffect } from "react";
import Select from "../../shared/ui/forms/Select";
import Loading from "../../shared/ui/controls/Loading";
import Specialization from "../../entities/specialization";
import CreateDoctorModel from "../../models/doctors/CreateDoctorModel";
import { useOffices } from "../../shared/hooks/useOffices";
import { createDoctorValidationSchema } from "./createDoctorValidationSchema";
import Office from "../../entities/office";
import { DoctorStatus } from "../../entities/enums/doctorStatus";
import DatePicker from "../../shared/ui/forms/DatePicker";

interface CreateDoctorFormProps {
  createDoctorModel: CreateDoctorModel;
  onSubmit: (createDoctorModel: CreateDoctorModel) => void;
  onCancel: () => void;
  token: string | null
}

export function CreateDoctorForm({ createDoctorModel, onSubmit, onCancel, token }: CreateDoctorFormProps) {

  const { fetchSpecializationsLoading, fetchSpecializationsError, fetchSpecializationsData, fetchSpecializations } = useSpecializations(token)
  const { fetchOfficesLoading, fetchOfficesError, fetchOfficesData, fetchOffices } = useOffices(token)

  useEffect(() => {
    fetchSpecializations()
    fetchOffices()
  }, [token])

  const formik = useFormik({
    initialValues: {
      firstName: createDoctorModel.firstName,
      lastName: createDoctorModel.lastName,
      middleName: createDoctorModel.middleName,
      officeId: createDoctorModel.officeId,
      specializationId: createDoctorModel.specializationId,
      status: createDoctorModel.status,
      dateOfBirth: createDoctorModel.dateOfBirth,
      careerStartYear: createDoctorModel.careerStartYear,
      account: {
        email: createDoctorModel.account.email,
        phoneNumber: createDoctorModel.account.phoneNumber,
      },
      photo: null
    },
    validationSchema: createDoctorValidationSchema,
    onSubmit: (values: CreateDoctorModel) => onSubmit(values),
  })

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-[50%]">
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

      {/* Email Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Email"
          id="account.email"
          name="account.email"
          onChange={formik.handleChange}
          value={formik.values.account.email}
        />
        {formik.touched.account?.email && formik.errors.account?.email ? (
          <div className="text-red-500">{formik.errors.account?.email}</div>
        ) : null}
      </div>

      {/* Phone Number Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Phone Number"
          id="account.phoneNumber"
          name="account.phoneNumber"
          onChange={formik.handleChange}
          value={formik.values.account.phoneNumber}
        />
        {formik.touched.account?.phoneNumber && formik.errors.account?.phoneNumber ? (
          <div className="text-red-500">{formik.errors.account?.phoneNumber}</div>
        ) : null}
      </div>

      {/* Specialization Select */}
      <div className="flex flex-col">
        {fetchSpecializationsLoading && <Loading label="Loading specializations..." />}
        {fetchSpecializationsError && <p className="text-red-500">Error: {fetchSpecializationsError}</p>}
        {fetchSpecializationsData && <Select
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
        </Select>}
        {formik.touched.specializationId && formik.errors.specializationId ? (
          <div className="text-red-500">{formik.errors.specializationId}</div>
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

      {/* Status Select */}
      <div className="flex flex-col">
        <Select
          label="Status"
          id="Status"
          name="Status"
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