import { Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { CreateOfficeModel } from "../models/createOfficeModel";
import { validationSchema } from "../models/validationSchema";
import { createOfficeRequest, resetState } from "../store/create-office";
import InnerForm from "./inner-form";
import { RootState } from "@app/store";
import { useState, useEffect } from "react";

interface CreateOfficeFormProps {
  close: () => void;
}

const CreateOfficeForm = ({ close }: CreateOfficeFormProps) => {
  const dispatch = useDispatch();
  const [isSubmited, setIsSubmited] = useState(false);
  const { success } = useSelector((state: RootState) => state.createOffice);

  useEffect(() => {
    dispatch(resetState());
  }, []);

  useEffect(() => {
    if (success && isSubmited) {
      close()
    }
  }, [success, isSubmited]);

  const onSubmit = (values: CreateOfficeModel) => {
    dispatch(createOfficeRequest(values));
    setIsSubmited(true);
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