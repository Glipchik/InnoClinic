import specializationsApi from '@features/specializations-list/api/specializations';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface DeleteSpecializationState {
  loading: boolean
  error?: string
  success?: boolean
}

const initialState : DeleteSpecializationState = {
  loading: false
};

const deleteSpecializationSlice = createSlice({
  name: 'DeleteSpecializationSlice',
  initialState,
  reducers: {
    deleteSpecializationRequest: (state, action: PayloadAction<{ id: string }>) => {
      state.loading = !!action;
    },
    deleteSpecializationSuccess: (state) => {
      state.loading = false;
      state.success = true;
    },
    deleteSpecializationFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
      state.success = false
    }
  }
});

export const { deleteSpecializationRequest } = deleteSpecializationSlice.actions;
export const deleteSpecializationSliceReducer = deleteSpecializationSlice.reducer

function* deleteSpecialization(
  action: AnyAction
): Generator<CallEffect<AxiosResponse> | PutEffect, void, void> {
  try {
    const { id } = action.payload;
    yield call(specializationsApi.delete, id);
    yield put(deleteSpecializationSlice.actions.deleteSpecializationSuccess())
  } catch (error) {
    yield put(deleteSpecializationSlice.actions.deleteSpecializationFailure(handleError(error as ApiError)))
  }
}

function* watchDeleteSpecialization() {
  yield takeLatest(deleteSpecializationRequest.type, deleteSpecialization);
}

export { watchDeleteSpecialization }