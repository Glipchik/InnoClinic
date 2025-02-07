import { useDispatch, useSelector } from "react-redux"
import { GET as doctorScheduleGET } from "../../shared/api/doctorScheduleApi"
import {
  fetchDoctorScheduleDataDataRequest,
  fetchDoctorScheduleDataFailure,
  fetchDoctorScheduleDataSuccess,
} from "../../store/slices/doctorScheduleSlice"
import { RootState } from "../../store/store";

export const useDoctorSchedule = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading, error, doctorScheduleData } = useSelector(
    (state: RootState) => state.doctorSchedule
  );

  const fetchDoctorSchedule = async (doctorId: string, date: Date) => {
    if (token) {
      try {
        dispatch(fetchDoctorScheduleDataDataRequest())
        const response = await doctorScheduleGET(doctorId, date, token)
        dispatch(fetchDoctorScheduleDataSuccess(response.data))
      } catch (error) {
        dispatch(fetchDoctorScheduleDataFailure(error instanceof Error ? error.message : "An unknown error occurred"))
      }
    }
  }

  return { loading, error, doctorScheduleData, fetchDoctorSchedule }
}

