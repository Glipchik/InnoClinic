import servicesApi from '@features/create-appointment-form/api/services';
import ServiceModel from '@models/services/serviceModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { call, CallEffect, put, PutEffect, takeLatest } from 'redux-saga/effects';

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

export const { fetchServicesRequest } = fetchServicesSlice.actions;
export const fetchServicesSliceReducer = fetchServicesSlice.reducer

type ApiResponse = AxiosResponse<ServiceModel[]>;

function* fetchServices(action: AnyAction) : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const {specializationId} = action.payload
    const response : ApiResponse  = yield call(servicesApi.getAll, specializationId);
    yield put(fetchServicesSlice.actions.fetchServicesSuccess(response.data))
  } catch (error) {
    yield put(fetchServicesSlice.actions.fetchServicesFailure(handleError(error as ApiError)))
  }
}

function* watchFetchServices() {
  yield takeLatest(fetchServicesRequest.type, fetchServices);
}

export { watchFetchServices }