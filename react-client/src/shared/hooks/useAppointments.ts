import { useDispatch, useSelector } from "react-redux"
import { POST } from "../api/appointmentApi"
import { fetchAppointmentsDataFailure, fetchAppointmentsDataSuccess, fetchAppointmentsDataRequest } from "../../store/slices/appointmentsSlice"
import { RootState } from "../../store/store";
import CreateAppointmentModel from "../../features/appointments/models/CreateAppointmentModel";

export const useAppointments = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading, error, appointmentsData } = useSelector(
    (state: RootState) => state.appointments
  );

  const createAppointment = async (createAppointmentModel: CreateAppointmentModel) => {
    if (token) {
      try {
        dispatch(fetchAppointmentsDataRequest())
        await POST(createAppointmentModel, token)
        dispatch(fetchAppointmentsDataSuccess(null))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchAppointmentsDataFailure(errorMessage))
      }
    }
  }

  return { loading, error, appointmentsData, createAppointment }
}