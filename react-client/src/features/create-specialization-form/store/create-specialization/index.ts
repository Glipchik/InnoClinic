import specializationsApi from '@features/create-specialization-form/api/specializations';
import CreateSpecializationModel from '@features/create-specialization-form/models/createSpecializationModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface CreateSpecializationState {
  success: boolean
  loading: boolean
  error?: string
}

const initialState : CreateSpecializationState = {
  success: false,
  loading: false,
  error: undefined
};

const createSpecializationSlice = createSlice({
  name: 'CreateSpecializationSlice',
  initialState,
  reducers: {
    createSpecializationRequest: (state, action: PayloadAction<CreateSpecializationModel>) => {
      state.loading = !!action;
    },
    createSpecializationSuccess: (state) => {
      state.loading = false;
      state.success = true
    },
    createSpecializationFailure: (state, action) => {
      state.success = false
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createSpecializationRequest } = createSpecializationSlice.actions;
export const createSpecializationSliceReducer = createSpecializationSlice.reducer

type ApiResponse = AxiosResponse<CreateSpecializationModel>;

function* createSpecialization(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const createSpecializationModel : CreateSpecializationModel = action.payload;
    yield call(specializationsApi.post, createSpecializationModel);
    yield put(createSpecializationSlice.actions.createSpecializationSuccess())
  } catch (error) {
    yield put(createSpecializationSlice.actions.createSpecializationFailure(handleError(error as ApiError)))
  }
}

function* watchCreateSpecialization() {
  yield takeLatest(createSpecializationRequest.type, createSpecialization);
}

export { watchCreateSpecialization }