import { Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import CreateSpecializationModel from "@features/create-specialization-form/models/createSpecializationModel";
import { createSpecializationRequest, resetState } from "../store/create-specialization";
import { validationSchema } from "../models/validationSchema";
import InnerForm from "./inner-form";
import { useEffect, useState } from "react";
import { RootState } from "@app/store";

interface CreateSpecializationFormProps {
  close: () => void;
}

const CreateSpecializationForm = ({ close }: CreateSpecializationFormProps) => {
  const dispatch = useDispatch();
  const [isSubmited, setIsSubmited] = useState(false);
  const { success } = useSelector((state: RootState) => state.createSpecialization);

  useEffect(() => {
    dispatch(resetState());
  }, []);

  useEffect(() => {
    if (success && isSubmited) {
      close()
    }
  }, [success, isSubmited]);

  const onSubmit = (values: CreateSpecializationModel) => {
    dispatch(createSpecializationRequest(values));
    setIsSubmited(true);
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