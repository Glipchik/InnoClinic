import SpecializationModel from "@models/specializations/specializationModel";
import specializationsApi from "@shared/api/specializations";
import { fetchSpecializationsFailure, fetchSpecializationsRequest, fetchSpecializationsSuccess } from "@shared/store/slices/fetch-specializations";
import { AxiosError } from "axios";
import { AxiosResponse } from "axios";
import { call, CallEffect, put, PutEffect } from "redux-saga/effects";
import { takeLatest } from 'redux-saga/effects';

type ApiResponse = AxiosResponse<SpecializationModel[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchSpecializations() : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const response : ApiResponse  = yield call(specializationsApi.getAll);
    yield put(fetchSpecializationsSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchSpecializationsFailure(errorMessage))
  }
}

function* watchFetchSpecializations() {
  yield takeLatest(fetchSpecializationsRequest.type, fetchSpecializations);
}

export { watchFetchSpecializations }