import specializationsApi from '@features/specializations-list/api/specializations';
import PaginatedList from '@models/paginatedList';
import SpecializationModel from '@models/specializations/specializationModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchSpecializationsWithPaginationState {
  loading: boolean
  error?: string
  data?: PaginatedList<SpecializationModel>
}

const initialState : FetchSpecializationsWithPaginationState = {
  loading: false
};

const fetchSpecializationsWithPaginationSlice = createSlice({
  name: 'FetchSpecializationsWithPaginationSlice',
  initialState,
  reducers: {
    fetchSpecializationsWithPaginationRequest: (state, action: PayloadAction<{ pageIndex: number, pageSize: number }>) => {
      state.loading = !!action;
    },
    fetchSpecializationsWithPaginationSuccess: (state, action: PayloadAction<PaginatedList<SpecializationModel>>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchSpecializationsWithPaginationFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchSpecializationsWithPaginationRequest } = fetchSpecializationsWithPaginationSlice.actions;
export const fetchSpecializationsWithPaginationSliceReducer = fetchSpecializationsWithPaginationSlice.reducer

type ApiResponse = AxiosResponse<PaginatedList<SpecializationModel>>;

function* fetchSpecializationsWithPagination(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { pageIndex, pageSize } = action.payload;
    const response: ApiResponse = yield call(specializationsApi.getAll, pageIndex, pageSize);
    yield put(fetchSpecializationsWithPaginationSlice.actions.fetchSpecializationsWithPaginationSuccess(response.data))
  } catch (error) {
    yield put(fetchSpecializationsWithPaginationSlice.actions.fetchSpecializationsWithPaginationFailure(handleError(error as ApiError)))
  }
}

function* watchFetchSpecializationsWithPagination() {
  yield takeLatest(fetchSpecializationsWithPaginationRequest.type, fetchSpecializationsWithPagination);
}

export { watchFetchSpecializationsWithPagination }