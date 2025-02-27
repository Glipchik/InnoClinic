import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import { AnyAction } from 'redux-saga';
import ServiceModel from '@models/services/serviceModel';
import servicesApi from '@features/create-appointment-form/api/services';
import { fetchServicesFailure, fetchServicesRequest, fetchServicesSuccess } from '@shared/store/slices/fetch-services';

type ApiResponse = AxiosResponse<ServiceModel[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchServices(action: AnyAction) : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const {specializationId} = action.payload
    const response : ApiResponse  = yield call(servicesApi.getAll, specializationId);
    yield put(fetchServicesSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchServicesFailure(errorMessage))
  }
}

function* watchFetchServices() {
  yield takeLatest(fetchServicesRequest.type, fetchServices);
}

export { watchFetchServices }