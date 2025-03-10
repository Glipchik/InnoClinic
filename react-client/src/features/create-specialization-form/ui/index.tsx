import { Formik } from "formik";
import { useDispatch } from "react-redux";
import CreateSpecializationModel from "@features/create-specialization-form/models/createSpecializationModel";
import { createSpecializationRequest } from "../store/create-specialization";
import { validationSchema } from "../models/validationSchema";
import InnerForm from "./inner-form";

interface CreateSpecializationFormProps {
  close: () => void;
}

export interface CreateSpecializationFormModel {
  address: string,
  registryPhoneNumber: string,
  isActive: boolean
}

const CreateSpecializationForm = ({ close }: CreateSpecializationFormProps) => {
  const dispatch = useDispatch();

  const onSubmit = (values: CreateSpecializationModel) => {
    dispatch(createSpecializationRequest(values));
  };

  const initialValues: CreateSpecializationModel = {
    specializationName: "",
    isActive: true
  }

  return (
    <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
      <InnerForm close={close} />
    </Formik>
  );
};

export { CreateSpecializationForm };