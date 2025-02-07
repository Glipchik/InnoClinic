import { useDispatch, useSelector } from "react-redux"
import { GET as serviceGET } from "../../shared/api/serviceApi"
import {
  fetchServicesDataFailure,
  fetchServicesDataSuccess,
  fetchServicesDataRequest,
} from "../../store/slices/servicesSlice"

export const useServices = (token: string | null) => {
  const dispatch = useDispatch()
  const { servicesLoading, servicesError, servicesData } = useSelector((state) => state.services)

  const fetchServices = async (specializationId: string) => {
    if (token) {
      try {
        dispatch(fetchServicesDataRequest())
        const response = await serviceGET(null, specializationId, token)
        dispatch(fetchServicesDataSuccess(response.data))
      } catch (error) {
        dispatch(fetchServicesDataFailure(error instanceof Error ? error.message : "An unknown error occurred"))
      }
    }
  }

  return { servicesLoading, servicesError, servicesData, fetchServices }
}

