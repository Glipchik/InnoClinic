import serviceCategoriesApi from '@features/create-service-category-form/api/service-categories';
import CreateServiceCategoryModel from '@features/create-service-category-form/models/createServiceCategoryModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface CreateServiceCategoryState {
  success: boolean
  loading: boolean
  error?: string
}

const initialState : CreateServiceCategoryState = {
  success: false,
  loading: false,
  error: undefined
};

const createServiceCategorySlice = createSlice({
  name: 'CreateServiceCategorySlice',
  initialState,
  reducers: {
    createServiceCategoryRequest: (state, action: PayloadAction<CreateServiceCategoryModel>) => {
      state.loading = !!action;
    },
    createServiceCategorySuccess: (state) => {
      state.loading = false;
      state.success = true
    },
    createServiceCategoryFailure: (state, action) => {
      state.success = false
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createServiceCategoryRequest } = createServiceCategorySlice.actions;
export const createServiceCategorySliceReducer = createServiceCategorySlice.reducer

type ApiResponse = AxiosResponse<CreateServiceCategoryModel>;

function* createServiceCategory(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const createServiceCategoryModel : CreateServiceCategoryModel = action.payload;
    yield call(serviceCategoriesApi.post, createServiceCategoryModel);
    yield put(createServiceCategorySlice.actions.createServiceCategorySuccess())
  } catch (error) {
    yield put(createServiceCategorySlice.actions.createServiceCategoryFailure(handleError(error as ApiError)))
  }
}

function* watchCreateServiceCategory() {
  yield takeLatest(createServiceCategoryRequest.type, createServiceCategory);
}

export { watchCreateServiceCategory }