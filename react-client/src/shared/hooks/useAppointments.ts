import { useDispatch, useSelector } from "react-redux"
import { POST } from "../api/appointmentApi"
import { createAppointmentFailure, createAppointmentRequest, createAppointmentSuccess } from "../../store/slices/appointments/createAppointmentSlice"
import { RootState } from "../../store/store";
import CreateAppointmentModel from "../../models/appointments/CreateAppointmentModel";

export const useAppointments = (token: string | null) => {
  const dispatch = useDispatch()

  const { loading : createAppointmentLoading, error : createAppointmentError } = useSelector(
    (state: RootState) => state.createAppointmentReducer
  );

  const createAppointment = async (createAppointmentModel: CreateAppointmentModel) => {
    if (token) {
      try {
        dispatch(createAppointmentRequest())
        await POST(createAppointmentModel, token)
        dispatch(createAppointmentSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(createAppointmentFailure(errorMessage))
      }
    }
  }

  return { createAppointmentLoading, createAppointmentError,
    createAppointment }
}