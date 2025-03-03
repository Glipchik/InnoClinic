import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import { Form, useFormikContext } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import FormFooter from "@widgets/form-footer";
import Input from "@shared/ui/forms/Input";
import { useEffect } from "react";
import EditServiceCategoryModel from "@features/edit-service-category-form/models/editServiceCategoryModel";
import { resetState } from "@features/edit-service-category-form/store/edit-service-category";

const InnerForm = () => {
  const { values, touched, errors, handleChange, handleBlur } = useFormikContext<EditServiceCategoryModel>();
  const { loading, error, success } = useSelector((state: RootState) => state.editServiceCategory);
  const navigate = useNavigate()
  const dispatch = useDispatch()

  const handleCancel = () => {
    dispatch(resetState())
    navigate(-1)
  }

  useEffect(() => {
    if (success) {
      navigate(-1)
    }
  }, [success, navigate]);

  return (
    <Form className="flex w-[40%] flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg m-6">
      <Input
        label="Service Category Name"
        type="text"
        name="categoryName"   
        onChange={handleChange}
        value={values.categoryName}
        error={(touched.categoryName && errors.categoryName) ? errors.categoryName : undefined}
        onBlur={handleBlur}
        id="service-category-name-input-for-edit-service-category-form"
      />

      <Input
        label="Time Slot Size"
        type="range"
        min="0"
        max="120"
        step="5"
        name="timeSlotSize"   
        onChange={handleChange}
        value={values.timeSlotSize}
        error={(touched.timeSlotSize && errors.timeSlotSize) ? errors.timeSlotSize : undefined}
        onBlur={handleBlur}
        id="time-slot-size-input-for-edit-service-category-form"
      />
      <div className="mt-2">
        {`00:${values.timeSlotSize.toString().padStart(2, '0')}:00`}
      </div>

      <FormFooter onCancel={handleCancel} />
      {loading && <Loading label="Editing servicecategory..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && success === true && <Label value="ServiceCategory was updated successfully" type="success"></Label>}
    </Form>
  );
};

export default InnerForm