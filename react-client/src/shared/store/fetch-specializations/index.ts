import SpecializationModel from '@shared/models/specializations/specializationModel';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import specializationsApi from '@shared/api/specializations';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { call, CallEffect, put, PutEffect, takeLatest } from 'redux-saga/effects';

interface FetchSpecializationsState {
  loading: boolean;
  error?: string;
  data?: SpecializationModel[]
}

const initialState : FetchSpecializationsState = {
  loading: false,
  error: undefined,
  data: undefined
};

const fetchSpecializationsSlice = createSlice({
  name: 'FetchSpecializationsSlice',
  initialState,
  reducers: {
    fetchSpecializationsRequest: state => {
      state.loading = true;
    },
    fetchSpecializationsSuccess: (state, action: PayloadAction<SpecializationModel[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchSpecializationsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchSpecializationsRequest } = fetchSpecializationsSlice.actions;
export const fetchSpecializationsSliceReducer = fetchSpecializationsSlice.reducer

type ApiResponse = AxiosResponse<SpecializationModel[]>;

function* fetchSpecializations() : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const response : ApiResponse  = yield call(specializationsApi.getAll);
    yield put(fetchSpecializationsSlice.actions.fetchSpecializationsSuccess(response.data))
  } catch (error) {
    yield put(fetchSpecializationsSlice.actions.fetchSpecializationsFailure(handleError(error as ApiError)))
  }
}

function* watchFetchSpecializations() {
  yield takeLatest(fetchSpecializationsRequest.type, fetchSpecializations);
}

export { watchFetchSpecializations }