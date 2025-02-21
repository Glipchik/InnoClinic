import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import { GET } from '../../api/doctor-schedule';
import TimeSlot from '../../../entities/timeSlot';
import { AnyAction } from 'redux-saga';

interface DoctorScheduleState {
  loading: boolean
  error?: string
  data?: TimeSlot[] 
}

const initialState : DoctorScheduleState = {
  loading: false,
  error: undefined,
  data: undefined
};

const fetchDoctorScheduleSlice = createSlice({
  name: 'FetchDoctorScheduleSlice',
  initialState,
  reducers: {
    fetchDoctorScheduleRequest: (state, action: PayloadAction<{ doctorId: string, date: Date }>) => {
      state.loading = true;
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
    const response = yield call(GET, doctorId, date);
    yield put(fetchDoctorScheduleSlice.actions.fetchDoctorScheduleSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchDoctorScheduleSlice.actions.fetchDoctorScheduleFailure(errorMessage))
  }
}

function* watchFetchDoctorSchedule() {
  yield takeLatest(fetchDoctorScheduleSlice.actions.fetchDoctorScheduleRequest.type, fetchDoctorSchedule);
}

export const { fetchDoctorScheduleRequest } = fetchDoctorScheduleSlice.actions;
export const fetchDoctorScheduleSliceReducer = fetchDoctorScheduleSlice.reducer
export { watchFetchDoctorSchedule }