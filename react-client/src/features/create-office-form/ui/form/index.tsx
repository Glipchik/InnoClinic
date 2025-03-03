import { Formik } from "formik";
import { useDispatch } from "react-redux";
import { validationSchema } from "../../models/validationSchema";
import InnerForm from "../inner-form";
import { CreateOfficeModel } from "../../models/createOfficeModel";
import { createOfficeRequest } from "../../store/create-office";

interface CreateOfficeFormProps {
  close: () => void;
}

export interface CreateOfficeFormModel {
  address: string,
  registryPhoneNumber: string,
  isActive: boolean
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