import { Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { editOfficeRequest } from "../store/edit-office";
import { EditOfficeModel } from "../models/editOfficeModel";
import { validationSchema } from "../models/validationSchema";
import InnerForm from "./inner-form";
import { RootState } from "@app/store";
import { useEffect, useState } from "react";
import { fetchOfficeByIdRequest } from "../store/fetch-office";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";

interface EditOfficeFormProps {
  officeId: string
}

export interface EditOfficeFormModel {
  id: string,
  address: string,
  registryPhoneNumber: string,
  isActive: boolean
}

const EditOfficeForm = ({ officeId }: EditOfficeFormProps) => {
  const dispatch = useDispatch();
  const { loading, error, data } = useSelector((state: RootState) => state.fetchOfficeById);
  const [initialValues, setInitialValues] = useState<EditOfficeFormModel | null>(null);

  useEffect(() => {
    dispatch(fetchOfficeByIdRequest({officeId}));
  }, [dispatch, officeId]);

  useEffect(() => {
    if (data) {
      setInitialValues({
        id: data.id,
        address: data.address,
        registryPhoneNumber: data.registryPhoneNumber,
        isActive: data.isActive
      });
    }
  }, [data]);

  const onSubmit = (values: EditOfficeModel) => {
    dispatch(editOfficeRequest(values));
  };

  if (loading) return (<Loading label="Fetching office"></Loading>)
  if (error) return (<Label type="error" value="Error fetching office"></Label>)

  if (initialValues)
    return (
      <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
        <InnerForm />
      </Formik>
    );
};

export { EditOfficeForm };