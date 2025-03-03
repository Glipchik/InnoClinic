import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import { Form, useFormikContext } from "formik";
import { useSelector } from "react-redux";
import FormFooter from "@widgets/form-footer";
import Input from "@shared/ui/forms/Input";
import { CreateOfficeModel } from "@features/create-office-form/models/createOfficeModel";

interface InnerFormProps {
  onCancel: () => void
}

const InnerForm = ({ onCancel }: InnerFormProps) => {
  const { values, touched, errors, handleChange, handleBlur } = useFormikContext<CreateOfficeModel>();
  const { loading, error, success } = useSelector((state: RootState) => state.createOffice);

  return (
    <Form className="flex w-[40%] flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg m-6">
      <Input
        type="text"
        name="address"   
        onChange={handleChange}
        value={values.address}
        error={(touched.address && errors.address) ? errors.address : undefined}
        onBlur={handleBlur}
        id="address-input-for-create-office-form"
      />
      <Input
        type="text"
        name="registryPhoneNumber"   
        onChange={handleChange}
        value={values.registryPhoneNumber}
        error={(touched.registryPhoneNumber && errors.registryPhoneNumber) ? errors.registryPhoneNumber : undefined}
        onBlur={handleBlur}
        id="registry-phone-number-input-for-create-office-form"
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
      <FormFooter onCancel={onCancel} />
      {loading && <Loading label="Creating office..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && success === true && <Label value="Office was created successfully" type="success"></Label>}
    </Form>
  );
};

export default InnerForm