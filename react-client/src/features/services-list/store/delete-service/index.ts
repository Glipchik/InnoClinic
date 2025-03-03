import servicesApi from '@features/services-list/api/service';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface DeleteServiceState {
  loading: boolean
  error?: string
  success?: boolean
}

const initialState : DeleteServiceState = {
  loading: false
};

const deleteServiceSlice = createSlice({
  name: 'DeleteServiceSlice',
  initialState,
  reducers: {
    deleteServiceRequest: (state, action: PayloadAction<{ id: string }>) => {
      state.loading = !!action;
    },
    deleteServiceSuccess: (state) => {
      state.loading = false;
      state.success = true;
    },
    deleteServiceFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
      state.success = false
    }
  }
});

export const { deleteServiceRequest } = deleteServiceSlice.actions;
export const deleteServiceSliceReducer = deleteServiceSlice.reducer

function* deleteService(
  action: AnyAction
): Generator<CallEffect<AxiosResponse> | PutEffect, void, void> {
  try {
    const { id } = action.payload;
    yield call(servicesApi.delete, id);
    yield put(deleteServiceSlice.actions.deleteServiceSuccess())
  } catch (error) {
    yield put(deleteServiceSlice.actions.deleteServiceFailure(handleError(error as ApiError)))
  }
}

function* watchDeleteService() {
  yield takeLatest(deleteServiceRequest.type, deleteService);
}

export { watchDeleteService }