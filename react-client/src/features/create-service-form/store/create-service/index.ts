import servicesApi from '@features/create-service-form/api/services';
import CreateServiceModel from '@features/create-service-form/models/createServiceModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface CreateServiceState {
  success: boolean
  loading: boolean
  error?: string
}

const initialState : CreateServiceState = {
  success: false,
  loading: false,
  error: undefined
};

const createServiceSlice = createSlice({
  name: 'CreateServiceSlice',
  initialState,
  reducers: {
    createServiceRequest: (state, action: PayloadAction<CreateServiceModel>) => {
      state.loading = !!action;
    },
    createServiceSuccess: (state) => {
      state.loading = false;
      state.success = true
    },
    createServiceFailure: (state, action) => {
      state.success = false
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createServiceRequest } = createServiceSlice.actions;
export const createServiceSliceReducer = createServiceSlice.reducer

type ApiResponse = AxiosResponse<CreateServiceModel>;

function* createService(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const createServiceModel : CreateServiceModel = action.payload;
    yield call(servicesApi.post, createServiceModel);
    yield put(createServiceSlice.actions.createServiceSuccess())
  } catch (error) {
    yield put(createServiceSlice.actions.createServiceFailure(handleError(error as ApiError)))
  }
}

function* watchCreateService() {
  yield takeLatest(createServiceRequest.type, createService);
}

export { watchCreateService }