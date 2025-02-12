import { useDispatch, useSelector } from "react-redux"
import { GET as doctorScheduleGET } from "../api/doctorScheduleApi"
import {
  fetchDoctorScheduleFailure,
  fetchDoctorScheduleRequest,
  fetchDoctorScheduleSuccess,
} from "../../store/slices/appointments/fetchDoctorSchedule"
import { RootState } from "../../store/store";

export const useDoctorSchedule = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading : fetchDoctorScheduleLoading, error : fetchDoctorScheduleError, data : fetchDoctorScheduleData } = useSelector(
    (state: RootState) => state.fetchDoctorScheduleReducer
  );

  const fetchDoctorSchedule = async (doctorId: string, date: Date) => {
    if (token) {
      try {
        dispatch(fetchDoctorScheduleRequest())
        const response = await doctorScheduleGET(doctorId, date, token)
        dispatch(fetchDoctorScheduleSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(fetchDoctorScheduleFailure(errorMessage))
      }
    }
  }

  return { fetchDoctorScheduleLoading, fetchDoctorScheduleError, fetchDoctorScheduleData, fetchDoctorSchedule }
}