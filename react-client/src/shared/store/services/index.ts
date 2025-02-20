import { Action, createSlice, PayloadAction } from '@reduxjs/toolkit';
import Service from '../../../entities/service';
import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import { GETAll } from '../../api/specializations';

interface ServicesState {
  loading: boolean
  error?: string
  data?: Service[] 
}

const initialState : ServicesState = {
  loading: false,
  error: undefined,
  data: undefined
};

const fetchServicesSlice = createSlice({
  name: 'FetchServicesSlice',
  initialState,
  reducers: {
    fetchServicesRequest: state => {
      state.loading = true;
    },
    fetchServicesSuccess: (state, action: PayloadAction<Service[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServicesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

type ApiResponse = AxiosResponse<Service[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchServices() : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const response : ApiResponse  = yield call(GETAll);
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
  yield takeLatest(fetchServicesSlice.actions.fetchServicesRequest.type, fetchServices);
}

export const { fetchServicesRequest, fetchServicesSuccess, fetchServicesFailure } = fetchServicesSlice.actions;
export const fetchServicesSliceReducer = fetchServicesSlice.reducer
export { watchFetchServices }