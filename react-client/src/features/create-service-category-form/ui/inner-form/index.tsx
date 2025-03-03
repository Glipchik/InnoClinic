import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import { Form, useFormikContext } from "formik";
import { useDispatch, useSelector } from "react-redux";
import FormFooter from "@widgets/form-footer";
import Input from "@shared/ui/forms/Input";
import { useEffect } from "react";
import { fetchServiceCategoriesWithPaginationRequest } from "@shared/store/fetch-service-categories-with-pagination";
import CreateServiceCategoryModel from "@features/create-service-category-form/models/createServiceCategoryModel";
import { resetState } from "@features/create-service-category-form/store/create-service-category";

interface InnerFormProps {
  close: () => void
}

const InnerForm = ({ close }: InnerFormProps) => {
  const { values, touched, errors, handleChange, handleBlur } = useFormikContext<CreateServiceCategoryModel>();
  const { loading, error, success } = useSelector((state: RootState) => state.createServiceCategory);
  const dispatch = useDispatch()

  useEffect(() => {
    if (success) {
      dispatch(fetchServiceCategoriesWithPaginationRequest({}))
      dispatch(resetState())
      close()
    }
  }, [success, dispatch]);

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
        id="service-category-name-input-for-create-service-category-form"
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
        id="time-slot-size-input-for-create-service-category-form"
      />
      
      <div className="mt-2">
        {`00:${values.timeSlotSize.toString().padStart(2, '0')}:00`}
      </div>
      
      <FormFooter onCancel={close} />
      {loading && <Loading label="Creating service category..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && success === true && <Label value="Service category was created successfully" type="success"></Label>}
    </Form>
  );
};

export default InnerForm