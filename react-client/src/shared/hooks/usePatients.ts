import { useDispatch, useSelector } from "react-redux"
import { DELETE, GETWithPagination, GET as patientGET, POST, PUT } from "../api/patientApi"
import { fetchPatientsFailure, fetchPatientsSuccess, fetchPatientsRequest } from "../../store/slices/patients/fetchPatientsSlice"
import { RootState } from "../../store/store";
import CreatePatientModel from "../../models/patients/CreatePatientModel";
import { createPatientFailure, createPatientRequest, createPatientSuccess } from "../../store/slices/patients/createPatientSlice";
import Patient from "../../entities/patient";
import { editPatientFailure, editPatientRequest, editPatientSuccess } from "../../store/slices/patients/editPatientSlice";
import { deletePatientFailure, deletePatientRequest, deletePatientSuccess } from "../../store/slices/patients/deletePatientSlice";

export const usePatients = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading : fetchPatientsLoading, error : fetchPatientsError, data : fetchPatientsData } = useSelector(
    (state: RootState) => state.fetchPatientsReducer
  );

  const fetchPatients = async () => {
    if (token) {
      try {
        dispatch(fetchPatientsRequest())
        const response = await patientGET(null, token)
        dispatch(fetchPatientsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchPatientsFailure(errorMessage))
      }
    }
  }

  const fetchPatientsWithPagination = async (pageIndex: number | null, pageSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchPatientsRequest())
        const response = await GETWithPagination(pageIndex, pageSize, token)
        dispatch(fetchPatientsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(fetchPatientsFailure(errorMessage))
      }
    }
  }
  
  const { loading : createPatientLoading, error : createPatientError } = useSelector(
    (state: RootState) => state.createPatientReducer
  );

  const createPatient = async (createPatientModel: CreatePatientModel) => {
    if (token) {
      try {
        dispatch(createPatientRequest())
        await POST(createPatientModel, token)
        dispatch(createPatientSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(createPatientFailure(errorMessage))
      }
    }
  }

  const { loading : editPatientLoading, error : editPatientError } = useSelector(
    (state: RootState) => state.editPatientReducer
  );

  const editPatient = async (patient: Patient) => {
    if (token) {
      try {
        dispatch(editPatientRequest())
        await PUT(patient, token)
        dispatch(editPatientSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editPatientFailure(errorMessage))
      }
    }
  }

  const { loading : deletePatientLoading, error : deletePatientError } = useSelector(
    (state: RootState) => state.deletePatientReducer
  );

  const deletePatient = async (id: string) => {
    if (token) {
      try {
        dispatch(deletePatientRequest())
        await DELETE(id, token)
        dispatch(deletePatientSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(deletePatientFailure(errorMessage))
      }
    }
  }

  return {
    fetchPatientsLoading, fetchPatientsError, fetchPatientsData,
    createPatientLoading, createPatientError,
    editPatientLoading, editPatientError,
    deletePatientLoading, deletePatientError,
    fetchPatients, editPatient, createPatient, deletePatient, fetchPatientsWithPagination }
}