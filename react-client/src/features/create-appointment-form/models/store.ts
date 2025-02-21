import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import TimeSlot from '../../../entities/timeSlot';
import { AnyAction } from 'redux-saga';
import CreateAppointmentModel from '../../../features/create-appointment-form/models/createAppointmentModel';
import appointmentsApi from '../../../shared/api/appointments';

interface CreateAppointmentState {
  loading: boolean
  error?: string
}

const initialState : CreateAppointmentState = {
  loading: false,
  error: undefined
};

const createAppointmentSlice = createSlice({
  name: 'CreateAppointmentSlice',
  initialState,
  reducers: {
    createAppointmentRequest: (state, action: PayloadAction<CreateAppointmentModel>) => {
      state.loading = true;
    },
    createAppointmentSuccess: (state) => {
      state.loading = false;
    },
    createAppointmentFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

type ApiResponse = AxiosResponse<TimeSlot[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* createAppointment(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const createAppointment : CreateAppointmentModel = action.payload;
    const response = yield call(appointmentsApi.POST, createAppointment);
    yield put(createAppointmentSlice.actions.createAppointmentSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(createAppointmentSlice.actions.createAppointmentFailure(errorMessage))
  }
}

function* watchCreateAppointment() {
  yield takeLatest(createAppointmentSlice.actions.createAppointmentRequest.type, createAppointment);
}

export const { createAppointmentRequest } = createAppointmentSlice.actions;
export const createAppointmentSliceReducer = createAppointmentSlice.reducer
export { watchCreateAppointment }