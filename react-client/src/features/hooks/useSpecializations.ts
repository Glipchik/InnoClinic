"use client"

import { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { GET as specializationGET } from "../../shared/api/specializationApi"
import {
  fetchSpecializationsDataFailure,
  fetchSpecializationsDataSuccess,
  fetchSpecializationsDataRequest,
} from "../../store/slices/specializationsSlice"
import { RootState } from "../../store/store";

export const useSpecializations = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading, error, specializationsData } = useSelector(
    (state: RootState) => state.specializations
  );

  useEffect(() => {
    const fetchSpecializations = async () => {
      if (token) {
        try {
          dispatch(fetchSpecializationsDataRequest())
          const response = await specializationGET(null, token)
          dispatch(fetchSpecializationsDataSuccess(response.data))
        } catch (error: unknown) {
          let errorMessage = "An unknown error occurred";
  
          if (error.response && error.response.data) {
            const problemDetails = error.response.data;
            errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
          }

          dispatch(
            fetchSpecializationsDataFailure(errorMessage),
          )
        }
      }
    }

    fetchSpecializations()
  }, [dispatch, token])

  return { loading, error, specializationsData }
}

