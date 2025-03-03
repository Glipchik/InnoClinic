import PaginatedList from '@models/paginatedList';
import ServiceCategoryModel from '@models/serviceCategories/serviceCategoryModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import serviceCategoriesApi from '@shared/api/service-categories';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchServiceCategoriesWithPaginationState {
  loading: boolean
  error?: string
  data?: PaginatedList<ServiceCategoryModel>
}

const initialState : FetchServiceCategoriesWithPaginationState = {
  loading: false
};

const fetchServiceCategoriesWithPaginationSlice = createSlice({
  name: 'FetchServiceCategoriesWithPaginationSlice',
  initialState,
  reducers: {
    fetchServiceCategoriesWithPaginationRequest: (state, action: PayloadAction<{ pageIndex?: number, pageSize?: number }>) => {
      state.loading = !!action;
    },
    fetchServiceCategoriesWithPaginationSuccess: (state, action: PayloadAction<PaginatedList<ServiceCategoryModel>>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServiceCategoriesWithPaginationFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServiceCategoriesWithPaginationRequest } = fetchServiceCategoriesWithPaginationSlice.actions;
export const fetchServiceCategoriesWithPaginationSliceReducer = fetchServiceCategoriesWithPaginationSlice.reducer

type ApiResponse = AxiosResponse<PaginatedList<ServiceCategoryModel>>;

function* fetchServiceCategoriesWithPagination(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { pageIndex, pageSize } = action.payload;
    const response: ApiResponse = yield call(serviceCategoriesApi.getAllWithPagination, pageIndex, pageSize);
    yield put(fetchServiceCategoriesWithPaginationSlice.actions.fetchServiceCategoriesWithPaginationSuccess(response.data))
  } catch (error) {
    yield put(fetchServiceCategoriesWithPaginationSlice.actions.fetchServiceCategoriesWithPaginationFailure(handleError(error as ApiError)))
  }
}

function* watchFetchServiceCategoriesWithPagination() {
  yield takeLatest(fetchServiceCategoriesWithPaginationRequest.type, fetchServiceCategoriesWithPagination);
}

export { watchFetchServiceCategoriesWithPagination }