import { Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { editOfficeRequest, resetState } from "../store/edit-office";
import { EditOfficeModel } from "../models/editOfficeModel";
import { validationSchema } from "../models/validationSchema";
import InnerForm from "./inner-form";
import { RootState } from "@app/store";
import { useEffect, useState } from "react";
import { fetchOfficeByIdRequest, resetState as resetFetchOfficeByIdState } from "../store/fetch-office";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";
import { useNavigate } from "react-router-dom";

interface EditOfficeFormProps {
  officeId: string
}

const EditOfficeForm = ({ officeId }: EditOfficeFormProps) => {
  const dispatch = useDispatch();
  const { loading, error, data } = useSelector((state: RootState) => state.fetchOfficeById);
  const { success } = useSelector((state: RootState) => state.editOffice);
  const navigate = useNavigate();
  const [initialValues, setInitialValues] = useState<EditOfficeModel | null>(null);
  const [isSubmited, setIsSubmited] = useState(false);

  useEffect(() => {
    dispatch(resetState());
  }, []);

  useEffect(() => {
    if (success && isSubmited) {
      dispatch(resetFetchOfficeByIdState())
      navigate(-1)
    }
  }, [success, isSubmited]);

  useEffect(() => {
    dispatch(fetchOfficeByIdRequest({officeId}));
  }, [officeId]);

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
    setIsSubmited(true);
  };

  if (loading) return (<Loading label="Fetching office"></Loading>)
  if (error) return (<Label type="error" value="Error fetching office"></Label>)

  if (initialValues)
    return (
      <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
        <InnerForm originalValues={initialValues} />
      </Formik>
    );
};

export { EditOfficeForm };