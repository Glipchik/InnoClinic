import officesApi from '@shared/api/offices';
import OfficeModel from '@shared/models/offices/officeModel';
import PaginatedList from '@models/paginatedList';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchOfficesState {
  loading: boolean
  error?: string
  data?: PaginatedList<OfficeModel>
}

const initialState : FetchOfficesState = {
  loading: false
};

const fetchOfficesSlice = createSlice({
  name: 'FetchOfficesSlice',
  initialState,
  reducers: {
    fetchOfficesRequest: (state, action: PayloadAction<{ pageIndex?: number, pageSize?: number }>) => {
      state.loading = !!action;
    },
    fetchOfficesSuccess: (state, action: PayloadAction<PaginatedList<OfficeModel>>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchOfficesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchOfficesRequest } = fetchOfficesSlice.actions;
export const fetchOfficesSliceReducer = fetchOfficesSlice.reducer

type ApiResponse = AxiosResponse<PaginatedList<OfficeModel>>;

function* fetchOffices(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { pageIndex, pageSize } = action.payload;
    const response: ApiResponse = yield call(officesApi.getAll, pageIndex, pageSize);
    yield put(fetchOfficesSlice.actions.fetchOfficesSuccess(response.data))
  } catch (error) {
    yield put(fetchOfficesSlice.actions.fetchOfficesFailure(handleError(error as ApiError)))
  }
}

function* watchFetchOffices() {
  yield takeLatest(fetchOfficesRequest.type, fetchOffices);
}

export { watchFetchOffices }