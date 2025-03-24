import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import { Form, useFormikContext } from "formik";
import { useSelector } from "react-redux";
import FormFooter from "@widgets/form-footer";
import Input from "@shared/ui/forms/Input";
import { CreateOfficeModel } from "@features/create-office-form/models/createOfficeModel";
import Checkbox from "@shared/ui/forms/CheckBox";

interface InnerFormProps {
  close: () => void;
}

const InnerForm = ({ close }: InnerFormProps) => {
  const { values, touched, errors, handleChange, handleBlur } =
    useFormikContext<CreateOfficeModel>();
  const { loading, error, success } = useSelector(
    (state: RootState) => state.createOffice
  );

  return (
    <Form className="flex w-[40%] flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg m-6">
      <Input
        data_testid="create-office-form-address-input"
        label="Address"
        type="text"
        name="address"
        onChange={handleChange}
        value={values.address}
        error={touched.address && errors.address ? errors.address : undefined}
        onBlur={handleBlur}
        id="address-input-for-create-office-form"
      />
      <Input
        data_testid="create-office-form-phone-number-input"
        label="Registry Phone Number"
        type="text"
        name="registryPhoneNumber"
        onChange={handleChange}
        value={values.registryPhoneNumber}
        error={
          touched.registryPhoneNumber && errors.registryPhoneNumber
            ? errors.registryPhoneNumber
            : undefined
        }
        onBlur={handleBlur}
        id="registry-phone-number-input-for-create-office-form"
      />
      <Checkbox
        data_testid="create-office-form-is-active-checkbox"
        checked={values.isActive}
        label="Is active"
        name="isActive"
        onChange={handleChange}
        onBlur={handleBlur}
        id="is-active-checkbox-input-for-edit-office-form"
        error={
          touched.isActive && errors.isActive ? errors.isActive : undefined
        }
      />
      <FormFooter onCancel={close} />
      {loading && <Loading label="Creating office..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && success === true && (
        <Label value="Office was created successfully" type="success"></Label>
      )}
    </Form>
  );
};

export default InnerForm;
