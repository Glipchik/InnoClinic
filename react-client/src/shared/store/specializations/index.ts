import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from "axios";
import { AxiosResponse } from "axios";
import { call, CallEffect, put, PutEffect } from "redux-saga/effects";
import { GETAll } from '../../api/specializations';
import { takeLatest } from 'redux-saga/effects';
import SpecializationModel from '../../api/specializations/models/specializationModel';

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

type ApiResponse = AxiosResponse<SpecializationModel[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchSpecializations() : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const response : ApiResponse  = yield call(GETAll);
    yield put(fetchSpecializationsSlice.actions.fetchSpecializationsSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchSpecializationsSlice.actions.fetchSpecializationsFailure(errorMessage))
  }
}

function* watchFetchSpecializations() {
  yield takeLatest(fetchSpecializationsSlice.actions.fetchSpecializationsRequest.type, fetchSpecializations);
}

export const { fetchSpecializationsRequest } = fetchSpecializationsSlice.actions;
export const fetchSpecializationsSliceReducer = fetchSpecializationsSlice.reducer
export { watchFetchSpecializations }