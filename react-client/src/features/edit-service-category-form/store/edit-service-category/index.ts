import servicecategorysApi from '@features/edit-service-category-form/api/service-categories';
import EditServiceCategoryModel from '@features/edit-service-category-form/models/editServiceCategoryModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { call, CallEffect, put, PutEffect, takeLatest } from 'redux-saga/effects';

interface EditServiceCategoryState {
  loading: boolean
  error?: string
  success?: boolean
}

const initialState : EditServiceCategoryState = {
  loading: false,
};

const editServiceCategorySlice = createSlice({
  name: 'EditServiceCategorySlice',
  initialState,
  reducers: {
    editServiceCategoryRequest: (state, action: PayloadAction<EditServiceCategoryModel>) => {
      state.loading = !!action;
    },
    editServiceCategorySuccess: (state) => {
      state.loading = false;
      state.success = true;
    },
    editServiceCategoryFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    },
    resetState: (state) => {
      state.loading = false;
      state.error = undefined;
      state.success = undefined
    }
  }
});

export const { editServiceCategoryRequest, resetState } = editServiceCategorySlice.actions;
export const editServiceCategorySliceReducer = editServiceCategorySlice.reducer

function* editServiceCategory(action: AnyAction) : Generator<CallEffect<AxiosResponse> | PutEffect, void, AxiosResponse> {
  try {
    const editServiceCategoryModel: EditServiceCategoryModel = action.payload
    yield call(servicecategorysApi.edit, editServiceCategoryModel);
    yield put(editServiceCategorySlice.actions.editServiceCategorySuccess())
  } catch (error) {
    yield put(editServiceCategorySlice.actions.editServiceCategoryFailure(handleError(error as ApiError)))
  }
}

function* watchEditServiceCategory() {
  yield takeLatest(editServiceCategoryRequest.type, editServiceCategory);
}

export { watchEditServiceCategory }