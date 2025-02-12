import { useFormik } from "formik";
import { validationSchema } from "./validationSchema";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import ServiceModel from "./models/ServiceModel";
import ServiceCategory from "../../entities/serviceCategory";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { useContext, useEffect, useState } from "react";
import { RootState } from "../../store/store";
import { useSelector } from "react-redux";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useServiceCategories } from "../../shared/hooks/useServiceCategories";
import Select from "../../shared/ui/forms/Select";
import Loading from "../../shared/ui/controls/Loading";
import Specialization from "../../entities/specialization";

interface ServiceFormProps {
  serviceModel: ServiceModel;
  onSubmit: (serviceModel: ServiceModel) => void;
  onCancel: () => void;
}

export function ServiceForm({ serviceModel, onSubmit, onCancel }: ServiceFormProps) {
  const [token, setToken] = useState<string | null>(null)

  const userManager = useContext(UserManagerContext)
  const { isUserAuthorized } = useSelector(
    (state: RootState) => state.auth
  );

  const { fetchSpecializationsLoading, fetchSpecializationsError, fetchSpecializationsData, fetchSpecializations } = useSpecializations(token)
  const { fetchServiceCategoriesData, fetchServiceCategoriesError, fetchServiceCategoriesLoading, fetchServiceCategories } = useServiceCategories(token)
  
  useEffect(() => {
    fetchSpecializations()
    fetchServiceCategories()
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
      id: serviceModel.id,
      serviceName: serviceModel.serviceName,
      serviceCategoryId: serviceModel.serviceCategoryId,
      specializationId: serviceModel.specializationId,
      isActive: serviceModel.isActive,
      price: serviceModel.price,
    },
    validationSchema,
    onSubmit: (values: ServiceModel) => onSubmit(values),
  })

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-[50%]">
      {/* Service Name Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Service Name"
          id="serviceName"
          name="serviceName"
          onChange={formik.handleChange}
          value={formik.values.serviceName}
        />
        {formik.touched.serviceName && formik.errors.serviceName ? (
          <div className="text-red-500">{formik.errors.serviceName}</div>
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

      {/* Service Category Select */}
      <div className="flex flex-col">
        {fetchServiceCategoriesLoading && <Loading label="Loading specializations..." />}
        {fetchServiceCategoriesError && <p className="text-red-500">Error: {fetchServiceCategoriesError}</p>}
        <Select
          disabled={false}
          label="Service Category"
          id="serviceCategoryId"
          name="serviceCategoryId"
          onChange={formik.handleChange}
          value={formik.values.serviceCategoryId}
        >
          <option value="" label="Select specialization" />
          {fetchServiceCategoriesData &&
            (fetchServiceCategoriesData as ServiceCategory[]).map((spec: ServiceCategory) => (
              <option key={spec.id} value={spec.id} label={spec.categoryName} />
            ))}
        </Select>
        {formik.touched.specializationId && formik.errors.specializationId ? (
          <div className="text-red-500">{formik.errors.specializationId}</div>
        ) : null}
      </div>

      {/* Service Price Input */}
      <div className="flex flex-col">
        <label htmlFor="input-field" className="text-gray-700 font-medium">"Service Price"</label>
        <input 
          className="form-control mt-1 p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          type="number"
          id="price"
          name="price"
          onChange={formik.handleChange}
          value={formik.values.price}
        />
        {formik.touched.price && formik.errors.price ? (
          <div className="text-red-500">{formik.errors.price}</div>
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