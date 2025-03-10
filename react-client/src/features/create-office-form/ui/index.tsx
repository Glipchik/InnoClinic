import { Formik } from "formik";
import { useDispatch } from "react-redux";
import { CreateOfficeModel } from "../models/createOfficeModel";
import { validationSchema } from "../models/validationSchema";
import { createOfficeRequest } from "../store/create-office";
import InnerForm from "./inner-form";

interface CreateOfficeFormProps {
  close: () => void;
}

const CreateOfficeForm = ({ close }: CreateOfficeFormProps) => {
  const dispatch = useDispatch();

  const onSubmit = (values: CreateOfficeModel) => {
    dispatch(createOfficeRequest(values));
  };

  const initialValues: CreateOfficeModel = {
    address: "",
    isActive: true,
    registryPhoneNumber: ""
  }

  return (
    <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
      <InnerForm close={close} />
    </Formik>
  );
};

export { CreateOfficeForm };