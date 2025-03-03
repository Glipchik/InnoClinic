import { Formik } from "formik";
import { useDispatch } from "react-redux";
import { validationSchema } from "../../models/validationSchema";
import InnerForm from "../inner-form";
import CreateServiceCategoryModel from "@features/create-service-category-form/models/createServiceCategoryModel";
import { createServiceCategoryRequest } from "@features/create-service-category-form/store/create-service-category";

interface CreateServiceCategoryFormProps {
  close: () => void;
}

export interface CreateServiceCategoryFormModel {
  address: string,
  registryPhoneNumber: string,
  isActive: boolean
}

const CreateServiceCategoryForm = ({ close }: CreateServiceCategoryFormProps) => {
  const dispatch = useDispatch();

  const onSubmit = (values: CreateServiceCategoryModel) => {
    values.timeSlotSize = `00:${values.timeSlotSize.toString().padStart(2, '0')}:00`
    dispatch(createServiceCategoryRequest(values));
  };

  const initialValues: CreateServiceCategoryModel = {
    categoryName: "",
    timeSlotSize: "15"
  }

  return (
    <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
      <InnerForm close={close} />
    </Formik>
  );
};

export { CreateServiceCategoryForm };