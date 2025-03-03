import servicesApi from '@features/edit-service-form/api/services';
import EditServiceModel from '@features/edit-service-form/models/EditServiceModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { call, CallEffect, put, PutEffect, takeLatest } from 'redux-saga/effects';

interface EditServiceState {
  loading: boolean
  error?: string
  success?: boolean
}

const initialState : EditServiceState = {
  loading: false,
};

const editServiceSlice = createSlice({
  name: 'EditServiceSlice',
  initialState,
  reducers: {
    editServiceRequest: (state, action: PayloadAction<EditServiceModel>) => {
      state.loading = !!action;
    },
    editServiceSuccess: (state) => {
      state.loading = false;
      state.success = true;
    },
    editServiceFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    },
    resetState: (state) => {
      state.loading = false
      state.success = undefined
      state.error = undefined
    }
  }
});

export const { editServiceRequest, resetState } = editServiceSlice.actions;
export const editServiceSliceReducer = editServiceSlice.reducer

function* editService(action: AnyAction) : Generator<CallEffect<AxiosResponse> | PutEffect, void, AxiosResponse> {
  try {
    const editServiceModel: EditServiceModel = action.payload
    yield call(servicesApi.edit, editServiceModel);
    yield put(editServiceSlice.actions.editServiceSuccess())
  } catch (error) {
    yield put(editServiceSlice.actions.editServiceFailure(handleError(error as ApiError)))
  }
}

function* watchEditService() {
  yield takeLatest(editServiceRequest.type, editService);
}

export { watchEditService }