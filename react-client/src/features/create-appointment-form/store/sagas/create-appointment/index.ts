import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import { AnyAction } from 'redux-saga';
import TimeSlot from '@entities/timeSlot';
import appointmentsApi from '@features/create-appointment-form/api/appointments';
import CreateAppointmentModel from '@features/create-appointment-form/models/createAppointmentModel';
import { createAppointmentFailure, createAppointmentRequest, createAppointmentSuccess } from '../../slices/create-appointment';

type ApiResponse = AxiosResponse<TimeSlot[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* createAppointment(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const createAppointmentModel : CreateAppointmentModel = action.payload;
    console.log(createAppointmentModel)
    const response = yield call(appointmentsApi.post, createAppointmentModel);
    yield put(createAppointmentSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(createAppointmentFailure(errorMessage))
  }
}

function* watchCreateAppointment() {
  yield takeLatest(createAppointmentRequest.type, createAppointment);
}

export { watchCreateAppointment }