import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import { Form, useFormikContext } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import FormFooter from "@widgets/form-footer";
import Input from "@shared/ui/forms/Input";
import { EditOfficeModel } from "@features/edit-office-form/models/editOfficeModel";
import Checkbox from "@shared/ui/forms/CheckBox";
import { resetState } from "@features/edit-office-form/store/fetch-office";

interface InnerFormProps {
  originalValues: EditOfficeModel
}

const InnerForm = ({ originalValues }: InnerFormProps) => {
  const { values, touched, errors, handleChange, handleBlur } = useFormikContext<EditOfficeModel>();
  const { loading, error, success } = useSelector((state: RootState) => state.editOffice);
  const navigate = useNavigate()
  const dispatch = useDispatch();

  const handleCancel = () => {
    dispatch(resetState())
    navigate(-1)
  }

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
      <Checkbox
        checked={values.isActive}
        label="Is active"
        name="isActive"
        onChange={handleChange}
        onBlur={handleBlur}
        id="is-active-checkbox-input-for-edit-office-form"
        error={(touched.isActive && errors.isActive) ? errors.isActive : undefined}
      />
      <FormFooter onCancel={handleCancel} submitDisabled={JSON.stringify(values) == JSON.stringify(originalValues)}/>
      {loading && <Loading label="Editing office..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && success === true && <Label value="Office was updated successfully" type="success"></Label>}
    </Form>
  );
};

export default InnerForm