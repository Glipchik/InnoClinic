import { Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { editSpecializationRequest } from "../store/edit-specialization";
import { validationSchema } from "../models/validationSchema";
import InnerForm from "./inner-form";
import { RootState } from "@app/store";
import { useEffect, useState } from "react";
import { fetchSpecializationByIdRequest } from "../store/fetch-specialization";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";
import EditSpecializationModel from "../models/editSpecializationModel";

interface EditSpecializationFormProps {
  specializationId: string
}

const EditSpecializationForm = ({ specializationId }: EditSpecializationFormProps) => {
  const dispatch = useDispatch();
  const { loading, error, data } = useSelector((state: RootState) => state.fetchSpecializationById);
  const [initialValues, setInitialValues] = useState<EditSpecializationModel | null>(null);

  useEffect(() => {
    dispatch(fetchSpecializationByIdRequest({specializationId}));
  }, [dispatch, specializationId]);

  useEffect(() => {
    if (data) {
      setInitialValues({
        id: data.id,
        specializationName: data.specializationName,
        isActive: data.isActive
      });
    }
  }, [data]);

  const onSubmit = (values: EditSpecializationModel) => {
    dispatch(editSpecializationRequest(values));
  };

  if (loading) return (<Loading label="Fetching specialization"></Loading>)
  if (error) return (<Label type="error" value="Error fetching specialization"></Label>)

  if (initialValues)
    return (
      <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
        <InnerForm />
      </Formik>
    );
};

export { EditSpecializationForm };