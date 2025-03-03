import ServiceCategoryModel from '@models/serviceCategories/serviceCategoryModel';
import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import serviceCategoriesApi from '@shared/api/service-categories';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchServiceCategoriesState {
  loading: boolean
  error?: string
  data?: ServiceCategoryModel[]
}

const initialState : FetchServiceCategoriesState = {
  loading: false
};

const fetchServiceCategoriesSlice = createSlice({
  name: 'FetchServiceCategoriesSlice',
  initialState,
  reducers: {
    fetchServiceCategoriesRequest: (state) => {
      state.loading = true;
    },
    fetchServiceCategoriesSuccess: (state, action: PayloadAction<ServiceCategoryModel[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServiceCategoriesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServiceCategoriesRequest } = fetchServiceCategoriesSlice.actions;
export const fetchServiceCategoriesSliceReducer = fetchServiceCategoriesSlice.reducer

type ApiResponse = AxiosResponse<ServiceCategoryModel[]>;

function* fetchServiceCategories(): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const response: ApiResponse = yield call(serviceCategoriesApi.getAll);
    yield put(fetchServiceCategoriesSlice.actions.fetchServiceCategoriesSuccess(response.data))
  } catch (error) {
    yield put(fetchServiceCategoriesSlice.actions.fetchServiceCategoriesFailure(handleError(error as ApiError)))
  }
}

function* watchFetchServiceCategories() {
  yield takeLatest(fetchServiceCategoriesRequest.type, fetchServiceCategories);
}

export { watchFetchServiceCategories }