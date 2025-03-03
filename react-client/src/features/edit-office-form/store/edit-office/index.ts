import officesApi from '@features/edit-office-form/api/offices';
import { EditOfficeModel } from '@features/edit-office-form/models/editOfficeModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { call, CallEffect, put, PutEffect, takeLatest } from 'redux-saga/effects';

interface EditOfficeState {
  loading: boolean
  error?: string
  success?: boolean
}

const initialState : EditOfficeState = {
  loading: false,
};

const editOfficeSlice = createSlice({
  name: 'EditOfficeSlice',
  initialState,
  reducers: {
    editOfficeRequest: (state, action: PayloadAction<EditOfficeModel>) => {
      state.loading = !!action;
    },
    editOfficeSuccess: (state) => {
      state.loading = false;
      state.success = true;
    },
    editOfficeFailure: (state, action) => {
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

export const { editOfficeRequest, resetState } = editOfficeSlice.actions;
export const editOfficeSliceReducer = editOfficeSlice.reducer

function* editOffice(action: AnyAction) : Generator<CallEffect<AxiosResponse> | PutEffect, void, AxiosResponse> {
  try {
    const editOfficeModel: EditOfficeModel = action.payload
    yield call(officesApi.edit, editOfficeModel);
    yield put(editOfficeSlice.actions.editOfficeSuccess())
  } catch (error) {
    yield put(editOfficeSlice.actions.editOfficeFailure(handleError(error as ApiError)))
  }
}

function* watchEditOffice() {
  yield takeLatest(editOfficeRequest.type, editOffice);
}

export { watchEditOffice }