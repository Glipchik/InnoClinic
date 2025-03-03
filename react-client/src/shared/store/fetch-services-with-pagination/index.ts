import servicesApi from '@shared/api/services';
import PaginatedList from '@models/paginatedList';
import ServiceModel from '@models/services/serviceModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchServicesWithPaginationState {
  loading: boolean
  error?: string
  data?: PaginatedList<ServiceModel>
}

const initialState : FetchServicesWithPaginationState = {
  loading: false
};

const fetchServicesWithPaginationSlice = createSlice({
  name: 'FetchServicesWithPaginationSlice',
  initialState,
  reducers: {
    fetchServicesWithPaginationRequest: (state, action: PayloadAction<{ pageIndex?: number, pageSize?: number }>) => {
      state.loading = !!action;
    },
    fetchServicesWithPaginationSuccess: (state, action: PayloadAction<PaginatedList<ServiceModel>>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServicesWithPaginationFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServicesWithPaginationRequest } = fetchServicesWithPaginationSlice.actions;
export const fetchServicesWithPaginationSliceReducer = fetchServicesWithPaginationSlice.reducer

type ApiResponse = AxiosResponse<PaginatedList<ServiceModel>>;

function* fetchServicesWithPagination(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { pageIndex, pageSize } = action.payload;
    const response: ApiResponse = yield call(servicesApi.getAllWithPagination, pageIndex, pageSize);
    yield put(fetchServicesWithPaginationSlice.actions.fetchServicesWithPaginationSuccess(response.data))
  } catch (error) {
    yield put(fetchServicesWithPaginationSlice.actions.fetchServicesWithPaginationFailure(handleError(error as ApiError)))
  }
}

function* watchFetchServicesWithPagination() {
  yield takeLatest(fetchServicesWithPaginationRequest.type, fetchServicesWithPagination);
}

export { watchFetchServicesWithPagination }