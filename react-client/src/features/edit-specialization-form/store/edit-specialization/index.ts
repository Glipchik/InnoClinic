import specializationsApi from '@features/edit-specialization-form/api/specializations';
import EditSpecializationModel from '@features/edit-specialization-form/models/editSpecializationModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { call, CallEffect, put, PutEffect, takeLatest } from 'redux-saga/effects';

interface EditSpecializationState {
  loading: boolean
  error?: string
  success?: boolean
}

const initialState : EditSpecializationState = {
  loading: false,
};

const editSpecializationSlice = createSlice({
  name: 'EditSpecializationSlice',
  initialState,
  reducers: {
    editSpecializationRequest: (state, action: PayloadAction<EditSpecializationModel>) => {
      state.loading = !!action;
    },
    editSpecializationSuccess: (state) => {
      state.loading = false;
      state.success = true;
    },
    editSpecializationFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    },
    resetState: (state) => {
      state.loading = false;
      state.error = undefined
      state.success = undefined
    }
  }
});

export const { editSpecializationRequest, resetState } = editSpecializationSlice.actions;
export const editSpecializationSliceReducer = editSpecializationSlice.reducer

function* editSpecialization(action: AnyAction) : Generator<CallEffect<AxiosResponse> | PutEffect, void, AxiosResponse> {
  try {
    const editSpecializationModel: EditSpecializationModel = action.payload
    yield call(specializationsApi.edit, editSpecializationModel);
    yield put(editSpecializationSlice.actions.editSpecializationSuccess())
  } catch (error) {
    yield put(editSpecializationSlice.actions.editSpecializationFailure(handleError(error as ApiError)))
  }
}

function* watchEditSpecialization() {
  yield takeLatest(editSpecializationRequest.type, editSpecialization);
}

export { watchEditSpecialization }