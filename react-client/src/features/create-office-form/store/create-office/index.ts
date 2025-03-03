import officesApi from '@features/create-office-form/api/offices';
import { CreateOfficeModel } from '@features/create-office-form/models/createOfficeModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface CreateOfficeState {
  success?: boolean
  loading: boolean
  error?: string
}

const initialState : CreateOfficeState = {
  success: false,
  loading: false,
  error: undefined
};

const createOfficeSlice = createSlice({
  name: 'CreateOfficeSlice',
  initialState,
  reducers: {
    createOfficeRequest: (state, action: PayloadAction<CreateOfficeModel>) => {
      state.loading = !!action;
    },
    createOfficeSuccess: (state) => {
      state.loading = false;
      state.success = true
    },
    createOfficeFailure: (state, action) => {
      state.success = false
      state.loading = false;
      state.error = action.payload;
    },
    resetState: (state) => {
      state.success = undefined
      state.loading = false;
      state.error = undefined;
    }
  }
});

export const { createOfficeRequest, resetState } = createOfficeSlice.actions;
export const createOfficeSliceReducer = createOfficeSlice.reducer

type ApiResponse = AxiosResponse<CreateOfficeModel>;

function* createOffice(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const createOfficeModel : CreateOfficeModel = action.payload;
    yield call(officesApi.post, createOfficeModel);
    yield put(createOfficeSlice.actions.createOfficeSuccess())
  } catch (error) {
    yield put(createOfficeSlice.actions.createOfficeFailure(handleError(error as ApiError)))
  }
}

function* watchCreateOffice() {
  yield takeLatest(createOfficeRequest.type, createOffice);
}

export { watchCreateOffice }