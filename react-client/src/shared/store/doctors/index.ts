import { Action, createSlice, PayloadAction } from '@reduxjs/toolkit';
import Doctor from '../../../entities/doctor';
import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import { GETAll } from '../../api/specializations';

interface DoctorsState {
  loading: boolean
  error?: string
  data?: Doctor[] 
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
    fetchDoctorsRequest: state => {
      state.loading = true;
    },
    fetchDoctorsSuccess: (state, action: PayloadAction<Doctor[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchDoctorsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

type ApiResponse = AxiosResponse<Doctor[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchDoctors() : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const response : ApiResponse  = yield call(GETAll);
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
  yield takeLatest(fetchDoctorsSlice.actions.fetchDoctorsRequest.type, fetchDoctors);
}

export const { fetchDoctorsRequest, fetchDoctorsSuccess, fetchDoctorsFailure } = fetchDoctorsSlice.actions;
export const fetchDoctorsSliceReducer = fetchDoctorsSlice.reducer
export { watchFetchDoctors }