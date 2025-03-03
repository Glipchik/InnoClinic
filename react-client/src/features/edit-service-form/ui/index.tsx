import { Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { editServiceRequest } from "../store/edit-service";
import { validationSchema } from "../models/validationSchema";
import InnerForm from "./inner-form";
import { RootState } from "@app/store";
import { useEffect, useState } from "react";
import { fetchServiceByIdRequest } from "../store/fetch-service";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";
import EditServiceModel from "../models/EditServiceModel";

interface EditServiceFormProps {
  serviceId: string
}

const EditServiceForm = ({ serviceId }: EditServiceFormProps) => {
  const dispatch = useDispatch();
  const { loading, error, data } = useSelector((state: RootState) => state.fetchServiceById);
  const [initialValues, setInitialValues] = useState<EditServiceModel | null>(null);

  useEffect(() => {
    dispatch(fetchServiceByIdRequest({serviceId}));
  }, [dispatch, serviceId]);

  useEffect(() => {
    if (data) {
      setInitialValues({
        id: data.id,
        serviceName: data.serviceName,
        specializationId: data.specialization.id,
        serviceCategoryId: data.serviceCategory.id,
        isActive: data.isActive,
        price: data.price.toString()
      });
    }
  }, [data]);

  const onSubmit = (values: EditServiceModel) => {
    dispatch(editServiceRequest(values));
  };

  if (loading) return (<Loading label="Fetching service"></Loading>)
  if (error) return (<Label type="error" value="Error fetching service"></Label>)

  if (initialValues)
    return (
      <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
        <InnerForm />
      </Formik>
    );
};

export { EditServiceForm };