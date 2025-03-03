import servicecategorysApi from '@features/edit-service-category-form/api/service-categories';
import ServiceCategoryModel from '@models/serviceCategories/serviceCategoryModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchServiceCategoryByIdState {
  loading: boolean
  error?: string
  data?: ServiceCategoryModel
}

const initialState : FetchServiceCategoryByIdState = {
  loading: false
};

const fetchServiceCategoryByIdSlice = createSlice({
  name: 'FetchServiceCategoryByIdSlice',
  initialState,
  reducers: {
    fetchServiceCategoryByIdRequest: (state, action: PayloadAction<{ servicecategoryId: string }>) => {
      state.loading = !!action;
    },
    fetchServiceCategoryByIdSuccess: (state, action: PayloadAction<ServiceCategoryModel>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServiceCategoryByIdFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServiceCategoryByIdRequest } = fetchServiceCategoryByIdSlice.actions;
export const fetchServiceCategoryByIdSliceReducer = fetchServiceCategoryByIdSlice.reducer

type ApiResponse = AxiosResponse<ServiceCategoryModel>;

function* fetchServiceCategoryById(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { servicecategoryId } = action.payload;
    const response: ApiResponse = yield call(servicecategorysApi.getById, servicecategoryId);
    yield put(fetchServiceCategoryByIdSlice.actions.fetchServiceCategoryByIdSuccess(response.data))
  } catch (error) {
    yield put(fetchServiceCategoryByIdSlice.actions.fetchServiceCategoryByIdFailure(handleError(error as ApiError)))
  }
}

function* watchFetchServiceCategoryById() {
  yield takeLatest(fetchServiceCategoryByIdRequest.type, fetchServiceCategoryById);
}

export { watchFetchServiceCategoryById }