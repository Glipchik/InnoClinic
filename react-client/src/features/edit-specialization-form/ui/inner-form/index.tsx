import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import { Form, useFormikContext } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import FormFooter from "@widgets/form-footer";
import Input from "@shared/ui/forms/Input";
import EditSpecializationModel from "@features/edit-specialization-form/models/editSpecializationModel";
import { useEffect } from "react";
import { resetState } from "@features/edit-specialization-form/store/edit-specialization";
import Checkbox from "@shared/ui/forms/CheckBox";

const InnerForm = () => {
  const { values, touched, errors, handleChange, handleBlur } = useFormikContext<EditSpecializationModel>();
  const { loading, error, success } = useSelector((state: RootState) => state.editSpecialization);
  const navigate = useNavigate()
  const dispatch = useDispatch()

  const handleCancel = () => {
    dispatch(resetState())
    navigate(-1)
  }

  useEffect(() => {
    if (success) {
      navigate(-1)
    }
  }, [success, navigate]);

  return (
    <Form className="flex w-[40%] flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg m-6">
      <Input
        label="Specialization Name"
        type="text"
        name="specializationName"   
        onChange={handleChange}
        value={values.specializationName}
        error={(touched.specializationName && errors.specializationName) ? errors.specializationName : undefined}
        onBlur={handleBlur}
        id="specialization-name-input-for-edit-specialization-form"
      />
      <Checkbox
        checked={values.isActive}
        label="Is active"
        name="isActive"
        onChange={handleChange}
        onBlur={handleBlur}
        id="is-active-checkbox-input-for-create-specialization-form"
        error={(touched.isActive && errors.isActive) ? errors.isActive : undefined}
      />
      <FormFooter onCancel={handleCancel} />
      {loading && <Loading label="Editing specialization..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && success === true && <Label value="Specialization was updated successfully" type="success"></Label>}
    </Form>
  );
};

export default InnerForm