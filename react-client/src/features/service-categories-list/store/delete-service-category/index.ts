import serviceCategoriesApi from '@features/service-categories-list/api/service-categories';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface DeleteServiceCategoryState {
  loading: boolean
  error?: string
  success?: boolean
}

const initialState : DeleteServiceCategoryState = {
  loading: false
};

const deleteServiceCategorySlice = createSlice({
  name: 'DeleteServiceCategorySlice',
  initialState,
  reducers: {
    deleteServiceCategoryRequest: (state, action: PayloadAction<{ id: string }>) => {
      state.loading = !!action;
    },
    deleteServiceCategorySuccess: (state) => {
      state.loading = false;
      state.success = true;
    },
    deleteServiceCategoryFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
      state.success = false
    }
  }
});

export const { deleteServiceCategoryRequest } = deleteServiceCategorySlice.actions;
export const deleteServiceCategorySliceReducer = deleteServiceCategorySlice.reducer

function* deleteServiceCategory(
  action: AnyAction
): Generator<CallEffect<AxiosResponse> | PutEffect, void, void> {
  try {
    const { id } = action.payload;
    yield call(serviceCategoriesApi.delete, id);
    yield put(deleteServiceCategorySlice.actions.deleteServiceCategorySuccess())
  } catch (error) {
    yield put(deleteServiceCategorySlice.actions.deleteServiceCategoryFailure(handleError(error as ApiError)))
  }
}

function* watchDeleteServiceCategory() {
  yield takeLatest(deleteServiceCategoryRequest.type, deleteServiceCategory);
}

export { watchDeleteServiceCategory }