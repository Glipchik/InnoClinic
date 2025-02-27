import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import { AnyAction } from 'redux-saga';
import DoctorModel from '@models/doctors/doctorModel';
import doctorsApi from '@features/create-appointment-form/api/doctors';
import { fetchDoctorsFailure, fetchDoctorsRequest, fetchDoctorsSuccess } from '@shared/store/slices/fetch-doctors';

type ApiResponse = AxiosResponse<DoctorModel[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchDoctors(action: AnyAction) : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const {specializationId} = action.payload
    const response : ApiResponse  = yield call(doctorsApi.getAll, specializationId);
    yield put(fetchDoctorsSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchDoctorsFailure(errorMessage))
  }
}

function* watchFetchDoctors() {
  yield takeLatest(fetchDoctorsRequest.type, fetchDoctors);
}

export { watchFetchDoctors }