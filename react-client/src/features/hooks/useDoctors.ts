import { useDispatch, useSelector } from "react-redux"
import { GET as doctorGET } from "../../shared/api/doctorApi"
import { fetchDoctorsDataFailure, fetchDoctorsDataSuccess, fetchDoctorsDataRequest } from "../../store/slices/doctorsSlice"

export const useDoctors = (token: string | null) => {
  const dispatch = useDispatch()
  const { doctorsLoading, doctorsError, doctorsData } = useSelector((state) => state.doctors)

  const fetchDoctors = async (specializationId: string) => {
    if (token) {
      try {
        dispatch(fetchDoctorsDataRequest())
        const response = await doctorGET(null, specializationId, token)
        dispatch(fetchDoctorsDataSuccess(response.data))
      } catch (error) {
        dispatch(fetchDoctorsDataFailure(error instanceof Error ? error.message : "An unknown error occurred"))
      }
    }
  }

  return { doctorsLoading, doctorsError, doctorsData, fetchDoctors }
}