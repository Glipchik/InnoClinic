import { useContext, useEffect, useState } from "react";
import Specialization from "../../entities/specialization";
import { useSelector } from "react-redux";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { RootState } from "../../store/store";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { useFormik } from "formik";
import { validationSchema } from "./validationSchema";
import Input from "../../shared/ui/forms/Input";
import Button from "../../shared/ui/controls/Button";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";

interface SpecializationFormProps {
  specialization: Specialization;
  onSubmit: (specialization: Specialization) => void;
  onCancel: () => void;
}

export function SpecializationForm({ specialization, onSubmit, onCancel }: SpecializationFormProps) {
  const [token, setToken] = useState<string | null>(null)
  const userManager = useContext(UserManagerContext);
  const { isUserAuthorized } = useSelector((state: RootState) => state.auth);

  const { loading: specializationsLoading, error: specializationsError } = useSpecializations(token);

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
      id: specialization.id,
      specializationName: specialization.specializationName,
      isActive: specialization.isActive,
    },
    validationSchema,
    onSubmit: (values: Specialization) => onSubmit(values),
  })

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-[50%]">
      {/* Specialization Name Input */}
      <div className="flex flex-col">
        <Input
          type="text"
          label="Specialization Name"
          id="specializationName"
          name="specializationName"
          onChange={formik.handleChange}
          value={formik.values.specializationName}
        />
        {formik.touched.specializationName && formik.errors.specializationName ? (
          <div className="text-red-500">{formik.errors.specializationName}</div>
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

      {specializationsLoading && <Loading label="Editing specialization..." />}
      {specializationsError && <ErrorBox value={specializationsError} />}
    </form>
  )
}