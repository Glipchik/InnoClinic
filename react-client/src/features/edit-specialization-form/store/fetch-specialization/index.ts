import specializationsApi from '@features/edit-specialization-form/api/specializations';
import SpecializationModel from '@models/specializations/specializationModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchSpecializationByIdState {
  loading: boolean
  error?: string
  data?: SpecializationModel
}

const initialState : FetchSpecializationByIdState = {
  loading: false
};

const fetchSpecializationByIdSlice = createSlice({
  name: 'FetchSpecializationByIdSlice',
  initialState,
  reducers: {
    fetchSpecializationByIdRequest: (state, action: PayloadAction<{ specializationId: string }>) => {
      state.loading = !!action;
    },
    fetchSpecializationByIdSuccess: (state, action: PayloadAction<SpecializationModel>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchSpecializationByIdFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    },
    resetState: (state) => {
      state.loading = false;
      state.error = undefined;
      state.data = undefined;
    }
  }
});

export const { fetchSpecializationByIdRequest, resetState } = fetchSpecializationByIdSlice.actions;
export const fetchSpecializationByIdSliceReducer = fetchSpecializationByIdSlice.reducer

type ApiResponse = AxiosResponse<SpecializationModel>;

function* fetchSpecializationById(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { specializationId } = action.payload;
    const response: ApiResponse = yield call(specializationsApi.getById, specializationId);
    yield put(fetchSpecializationByIdSlice.actions.fetchSpecializationByIdSuccess(response.data))
  } catch (error) {
    yield put(fetchSpecializationByIdSlice.actions.fetchSpecializationByIdFailure(handleError(error as ApiError)))
  }
}

function* watchFetchSpecializationById() {
  yield takeLatest(fetchSpecializationByIdRequest.type, fetchSpecializationById);
}

export { watchFetchSpecializationById }