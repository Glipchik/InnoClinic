import { Formik } from "formik";
import { useDispatch } from "react-redux";
import CreateServiceModel from "../../models/createServiceModel";
import { validationSchema } from "../../models/validationSchema";
import { createServiceRequest } from "../../store/create-service";
import InnerForm from "../inner-form";

interface CreateServiceFormProps {
  close: () => void;
}

export const CreateServiceForm = ({ close }: CreateServiceFormProps) => {
  const dispatch = useDispatch();

  const initialValues: CreateServiceModel = {
    serviceCategoryId: "",
    specializationId: "",
    isActive: true,
    price: "0",
    serviceName: ""
  };

  const onSubmit = (values: CreateServiceModel) => {
    dispatch(createServiceRequest(values));
  };

  return (
    <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
      <InnerForm close={close} />
    </Formik>
  );
};