import appointmentsApi from '@features/create-appointment-form/api/appointments';
import CreateAppointmentModel from '@features/create-appointment-form/models/createAppointmentModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface CreateAppointmentState {
  success: boolean
  loading: boolean
  error?: string
}

const initialState : CreateAppointmentState = {
  success: false,
  loading: false,
  error: undefined
};

const createAppointmentSlice = createSlice({
  name: 'CreateAppointmentSlice',
  initialState,
  reducers: {
    createAppointmentRequest: (state, action: PayloadAction<CreateAppointmentModel>) => {
      state.loading = !!action;
    },
    createAppointmentSuccess: (state) => {
      state.loading = false;
      state.success = true
    },
    createAppointmentFailure: (state, action) => {
      state.success = false
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createAppointmentRequest } = createAppointmentSlice.actions;
export const createAppointmentSliceReducer = createAppointmentSlice.reducer

type ApiResponse = AxiosResponse<CreateAppointmentModel>;

function* createAppointment(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const createAppointmentModel : CreateAppointmentModel = action.payload;
    yield call(appointmentsApi.post, createAppointmentModel);
    yield put(createAppointmentSlice.actions.createAppointmentSuccess())
  } catch (error) {
    yield put(createAppointmentSlice.actions.createAppointmentFailure(handleError(error as ApiError)))
  }
}

function* watchCreateAppointment() {
  yield takeLatest(createAppointmentRequest.type, createAppointment);
}

export { watchCreateAppointment }