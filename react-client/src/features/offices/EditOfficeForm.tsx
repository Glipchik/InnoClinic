import { useContext, useEffect, useState } from "react";
import Office from "../../entities/office";
import { useDispatch, useSelector } from "react-redux";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { RootState } from "../../store/store";
import { useOffices } from "../../shared/hooks/useOffices";
import { useFormik } from "formik";
import { validationSchema } from "./validationSchema";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";

interface EditOfficeFormProps {
  office: Office;
  onSubmit: (office: Office) => void;
  onCancel: () => void;
}

export function EditOfficeForm({ office, onSubmit, onCancel }: EditOfficeFormProps) {
  const [token, setToken] = useState<string | null>(null)
  const dispatch = useDispatch();
  const userManager = useContext(UserManagerContext);
  const { isUserAuthorized } = useSelector((state: RootState) => state.auth);

  const { loading: officesLoading, error: officesError } = useOffices(token);

  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser();
        setToken(user?.access_token ?? null);
      }
      fetchUser();
    }
  }, [userManager, isUserAuthorized]);

  const formik = useFormik({
    initialValues: {
      id: office.id,
      address: office.address,
      registryPhoneNumber: office.registryPhoneNumber,
      isActive: office.isActive,
    },
    validationSchema,
    onSubmit: (values: Office) => onSubmit(values),
  })

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-[50%]">
      {/* Address Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Address"
          id="address"
          name="address"
          onChange={formik.handleChange}
          value={formik.values.address}
        />
        {formik.touched.address && formik.errors.address ? (
          <div className="text-red-500">{formik.errors.address}</div>
        ) : null}
      </div>

      {/* Registry Phone Number Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Registry Phone Number"
          id="registryPhoneNumber"
          name="registryPhoneNumber"
          onChange={formik.handleChange}
          value={formik.values.registryPhoneNumber}
        />
        {formik.touched.registryPhoneNumber && formik.errors.registryPhoneNumber ? (
          <div className="text-red-500">{formik.errors.registryPhoneNumber}</div>
        ) : null}
      </div>

      {/* Is Active Checkbox */}
      <div className="flex items-center space-x-2">
        <input
          type="checkbox"
          id="isActive"
          name="isActive"
          checked={formik.values.isActive}
          onChange={formik.handleChange}
          className="w-5 h-5"
        />
        <label htmlFor="isActive" className="text-lg">
          Is Active
        </label>
      </div>

      {/* Submit and Cancel Buttons */}
      <div className="flex space-x-2">
        <Button type="submit" className="w-full bg-green-500 hover:bg-green-600">
          Save
        </Button>
        <Button type="button" onClick={onCancel} className="w-full bg-gray-500 hover:bg-gray-600">
          Cancel
        </Button>
      </div>

      {officesLoading && <Loading label="Editing office..." />}
      {officesError && <ErrorBox value={officesError} />}
    </form>
  )
}