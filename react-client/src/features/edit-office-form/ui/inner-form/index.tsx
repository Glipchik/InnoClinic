import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import { Form, useFormikContext } from "formik";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import FormFooter from "@widgets/form-footer";
import Input from "@shared/ui/forms/Input";
import { EditOfficeModel } from "@features/edit-office-form/models/editOfficeModel";
import { useEffect } from "react";

const InnerForm = () => {
  const { values, touched, errors, handleChange, handleBlur } = useFormikContext<EditOfficeModel>();
  const { loading, error, success } = useSelector((state: RootState) => state.editOffice);
  const navigate = useNavigate()

  const handleCancel = () => {
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
        label="Address"
        type="text"
        name="address"   
        onChange={handleChange}
        value={values.address}
        error={(touched.address && errors.address) ? errors.address : undefined}
        onBlur={handleBlur}
        id="address-input-for-edit-office-form"
      />
      <Input
        label="Registry Phone Number"
        type="text"
        name="registryPhoneNumber"   
        onChange={handleChange}
        value={values.registryPhoneNumber}
        error={(touched.registryPhoneNumber && errors.registryPhoneNumber) ? errors.registryPhoneNumber : undefined}
        onBlur={handleBlur}
        id="registry-phone-number-input-for-edit-office-form"
      />
      <label className="flex items-center gap-2">
        <input
          type="checkbox"
          name="isActive"
          id="is-active-checkbox-for-create-office-form"
          checked={values.isActive}
          onChange={handleChange}
        />
        Is active
      </label>
      <FormFooter onCancel={handleCancel} />
      {loading && <Loading label="Editing office..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && success === true && <Label value="Office was updated successfully" type="success"></Label>}
    </Form>
  );
};

export default InnerForm