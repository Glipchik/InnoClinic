import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import { Form, useFormikContext } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import FormFooter from "@widgets/form-footer";
import Input from "@shared/ui/forms/Input";
import { useEffect } from "react";
import EditServiceModel from "@features/edit-service-form/models/EditServiceModel";
import ServiceCategorySelect from "@shared/ui/service-category-select";
import SpecializationSelect from "@shared/ui/specialization-select";
import { resetState } from "@features/edit-service-form/store/edit-service";

const InnerForm = () => {
  const { values, touched, errors, handleChange, handleBlur } = useFormikContext<EditServiceModel>();
  const { loading, error, success } = useSelector((state: RootState) => state.editService);
  const dispatch = useDispatch();
  const navigate = useNavigate()

  const handleCancel = () => {
    navigate(-1)
  }

  useEffect(() => {
    if (success) {
      dispatch(resetState())
      navigate(-1)
    }
  }, [success]);

  return (
    <Form className="flex w-[40%] flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg m-6">
      <SpecializationSelect
        disabled={false}
        id="specialization-select-for-create-service-form"
        name="specializationId"
        value={values.specializationId}
        onChange={handleChange}
        onBlur={handleBlur}
        error={(touched.specializationId && errors.specializationId) ? errors.specializationId : undefined}
        isLoadingRequired={true}
      />
      <ServiceCategorySelect
        id="service-category-select-for-create-service-form"
        disabled={false}
        name="serviceCategoryId"
        onChange={handleChange}
        onBlur={handleBlur}
        value={values.serviceCategoryId}
        error={(touched.serviceCategoryId && errors.serviceCategoryId) ? errors.serviceCategoryId : undefined}
        isLoadingRequired={true}
      />
      <Input
        label="Service Name"
        type="text"
        name="serviceName"
        onChange={handleChange}
        value={values.serviceName}
        error={(touched.serviceName && errors.serviceName) ? errors.serviceName : undefined}
        onBlur={handleBlur}
        id="service-name-input-for-edit-service-form"
      />
      <Input
        label="Price"
        type="number"
        name="price"
        onChange={handleChange}
        value={values.price}
        error={(touched.price && errors.price) ? errors.price : undefined}
        onBlur={handleBlur}
        id="service-name-input-for-edit-service-form"
      />
      <label className="flex items-center gap-2">
        <input
          type="checkbox"
          name="isActive"
          id="is-active-checkbox-for-create-service-form"
          checked={values.isActive}
          onChange={handleChange}
        />
        Is active
      </label>
      <FormFooter onCancel={handleCancel} />
      {loading && <Loading label="Editing service..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && success === true && <Label value="Service was updated successfully" type="success"></Label>}
    </Form>
  );
};

export default InnerForm