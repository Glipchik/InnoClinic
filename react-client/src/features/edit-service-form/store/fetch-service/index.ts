import servicesApi from '@features/edit-service-form/api/services';
import ServiceModel from '@models/services/serviceModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchServiceByIdState {
  loading: boolean
  error?: string
  data?: ServiceModel
}

const initialState : FetchServiceByIdState = {
  loading: false
};

const fetchServiceByIdSlice = createSlice({
  name: 'FetchServiceByIdSlice',
  initialState,
  reducers: {
    fetchServiceByIdRequest: (state, action: PayloadAction<{ serviceId: string }>) => {
      state.loading = !!action;
    },
    fetchServiceByIdSuccess: (state, action: PayloadAction<ServiceModel>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServiceByIdFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServiceByIdRequest } = fetchServiceByIdSlice.actions;
export const fetchServiceByIdSliceReducer = fetchServiceByIdSlice.reducer

type ApiResponse = AxiosResponse<ServiceModel>;

function* fetchServiceById(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { serviceId } = action.payload;
    const response: ApiResponse = yield call(servicesApi.getById, serviceId);
    yield put(fetchServiceByIdSlice.actions.fetchServiceByIdSuccess(response.data))
  } catch (error) {
    yield put(fetchServiceByIdSlice.actions.fetchServiceByIdFailure(handleError(error as ApiError)))
  }
}

function* watchFetchServiceById() {
  yield takeLatest(fetchServiceByIdRequest.type, fetchServiceById);
}

export { watchFetchServiceById }