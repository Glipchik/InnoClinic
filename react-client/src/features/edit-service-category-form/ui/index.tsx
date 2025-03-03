import { Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import InnerForm from "./inner-form";
import { RootState } from "@app/store";
import { useEffect, useState } from "react";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";
import EditServiceCategoryModel from "../models/editServiceCategoryModel";
import { fetchServiceCategoryByIdRequest } from "../store/fetch-service-category";
import { editServiceCategoryRequest } from "../store/edit-service-category";
import { validationSchema } from "../models/validationSchema";

interface EditServiceCategoryFormProps {
  servicecategoryId: string
}

const EditServiceCategoryForm = ({ servicecategoryId }: EditServiceCategoryFormProps) => {
  const dispatch = useDispatch();
  const { loading, error, data } = useSelector((state: RootState) => state.fetchServiceCategoryById);
  const [initialValues, setInitialValues] = useState<EditServiceCategoryModel | null>(null);

  useEffect(() => {
    dispatch(fetchServiceCategoryByIdRequest({servicecategoryId}));
  }, [dispatch, servicecategoryId]);

  useEffect(() => {
    if (data) {
      setInitialValues({
        id: data.id,
        categoryName: data.categoryName,
        timeSlotSize: data.timeSlotSize
      });
    }
  }, [data]);

  const onSubmit = (values: EditServiceCategoryModel) => {
    values.timeSlotSize = `00:${values.timeSlotSize.toString().padStart(2, '0')}:00`
    dispatch(editServiceCategoryRequest(values));
  };

  if (loading) return (<Loading label="Fetching service category"></Loading>)
  if (error) return (<Label type="error" value="Error fetching service category"></Label>)

  if (initialValues)
    return (
      <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
        <InnerForm />
      </Formik>
    );
};

export { EditServiceCategoryForm };