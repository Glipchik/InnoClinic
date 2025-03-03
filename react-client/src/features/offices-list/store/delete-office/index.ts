import officesApi from '@features/offices-list/api/offices';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface DeleteOfficeState {
  loading: boolean
  error?: string
  success?: boolean
}

const initialState : DeleteOfficeState = {
  loading: false
};

const deleteOfficeSlice = createSlice({
  name: 'DeleteOfficeSlice',
  initialState,
  reducers: {
    deleteOfficeRequest: (state, action: PayloadAction<{ officeId: string }>) => {
      state.loading = !!action;
    },
    deleteOfficeSuccess: (state) => {
      state.loading = false;
      state.success = true;
    },
    deleteOfficeFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
      state.success = false
    }
  }
});

export const { deleteOfficeRequest } = deleteOfficeSlice.actions;
export const deleteOfficeSliceReducer = deleteOfficeSlice.reducer

function* deleteOffice(
  action: AnyAction
): Generator<CallEffect<AxiosResponse> | PutEffect, void, void> {
  try {
    const { officeId } = action.payload;
    yield call(officesApi.delete, officeId);
    yield put(deleteOfficeSlice.actions.deleteOfficeSuccess())
  } catch (error) {
    yield put(deleteOfficeSlice.actions.deleteOfficeFailure(handleError(error as ApiError)))
  }
}

function* watchDeleteOffice() {
  yield takeLatest(deleteOfficeRequest.type, deleteOffice);
}

export { watchDeleteOffice }