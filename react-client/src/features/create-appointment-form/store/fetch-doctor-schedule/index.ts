import TimeSlot from '@entities/timeSlot';
import timeSlotsApi from '@features/create-appointment-form/api/doctor-schedule';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError from '@shared/lib/errorHandler';
import { AxiosResponse, AxiosError } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface DoctorScheduleState {
  loading: boolean
  error?: string
  data?: TimeSlot[]
}

const initialState : DoctorScheduleState = {
  loading: false
};

const fetchDoctorScheduleSlice = createSlice({
  name: 'FetchDoctorScheduleSlice',
  initialState,
  reducers: {
    fetchDoctorScheduleRequest: (state, action: PayloadAction<{ doctorId: string, date: string }>) => {
      state.loading = !!action;
    },
    fetchDoctorScheduleSuccess: (state, action: PayloadAction<TimeSlot[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchDoctorScheduleFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchDoctorScheduleRequest } = fetchDoctorScheduleSlice.actions;
export const fetchDoctorScheduleSliceReducer = fetchDoctorScheduleSlice.reducer

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
    const response: ApiResponse = yield call(timeSlotsApi.get, doctorId, date);
    yield put(fetchDoctorScheduleSlice.actions.fetchDoctorScheduleSuccess(response.data))
  } catch (error) {
    yield put(fetchDoctorScheduleSlice.actions.fetchDoctorScheduleFailure(handleError(error as ApiError)))
  }
}

function* watchFetchDoctorSchedule() {
  yield takeLatest(fetchDoctorScheduleRequest.type, fetchDoctorSchedule);
}

export { watchFetchDoctorSchedule }