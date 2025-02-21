import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import doctorsApi from '../../api/doctors';
import { AnyAction } from 'redux-saga';
import DoctorModel from '../../api/doctors/models/doctorModel';

interface DoctorsState {
  loading: boolean
  error?: string
  data?: DoctorModel[]
}

const initialState : DoctorsState = {
  loading: false,
  error: undefined,
  data: undefined
};

const fetchDoctorsSlice = createSlice({
  name: 'FetchDoctorsSlice',
  initialState,
  reducers: {
    fetchDoctorsRequest: (state, action: PayloadAction<{ specializationId: string }>) => {
      state.loading = !!action;
    },
    fetchDoctorsSuccess: (state, action: PayloadAction<DoctorModel[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchDoctorsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

type ApiResponse = AxiosResponse<DoctorModel[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchDoctors(action: AnyAction) : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const {specializationId} = action.payload
    const response : ApiResponse  = yield call(doctorsApi.GETAll, specializationId);
    yield put(fetchDoctorsSlice.actions.fetchDoctorsSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchDoctorsSlice.actions.fetchDoctorsFailure(errorMessage))
  }
}

function* watchFetchDoctors() {
  yield takeLatest(fetchDoctorsSlice.actions.fetchDoctorsRequest.type, fetchDoctors);
}

export const { fetchDoctorsRequest } = fetchDoctorsSlice.actions;
export const fetchDoctorsSliceReducer = fetchDoctorsSlice.reducer
export { watchFetchDoctors }