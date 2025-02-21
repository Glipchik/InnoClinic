import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import servicesApi from '../../api/services';
import { AnyAction } from 'redux-saga';
import ServiceModel from '../../api/services/models/serviceModel';

interface ServicesState {
  loading: boolean
  error?: string
  data?: ServiceModel[]
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
    fetchServicesRequest: (state, action: PayloadAction<{ specializationId: string }>) => {
      state.loading = !!action;
    },
    fetchServicesSuccess: (state, action: PayloadAction<ServiceModel[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServicesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

type ApiResponse = AxiosResponse<ServiceModel[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchServices(action: AnyAction) : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const {specializationId} = action.payload
    const response : ApiResponse  = yield call(servicesApi.GETAll, specializationId);
    yield put(fetchServicesSlice.actions.fetchServicesSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchServicesSlice.actions.fetchServicesFailure(errorMessage))
  }
}

function* watchFetchServices() {
  yield takeLatest(fetchServicesSlice.actions.fetchServicesRequest.type, fetchServices);
}

export const { fetchServicesRequest } = fetchServicesSlice.actions;
export const fetchServicesSliceReducer = fetchServicesSlice.reducer
export { watchFetchServices }