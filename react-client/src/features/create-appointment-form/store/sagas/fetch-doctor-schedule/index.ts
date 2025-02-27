import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import { AnyAction } from 'redux-saga';
import TimeSlot from '@entities/timeSlot';
import { fetchDoctorScheduleFailure, fetchDoctorScheduleRequest, fetchDoctorScheduleSuccess } from '../../slices/fetch-doctor-schedule';
import timeSlotsApi from '@features/create-appointment-form/api/doctor-schedule';

type ApiResponse = AxiosResponse<TimeSlot[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchDoctorSchedule(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { doctorId, date } = action.payload;
    const response = yield call(timeSlotsApi.get, doctorId, date);
    yield put(fetchDoctorScheduleSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchDoctorScheduleFailure(errorMessage))
  }
}

function* watchFetchDoctorSchedule() {
  yield takeLatest(fetchDoctorScheduleRequest.type, fetchDoctorSchedule);
}

export { watchFetchDoctorSchedule }