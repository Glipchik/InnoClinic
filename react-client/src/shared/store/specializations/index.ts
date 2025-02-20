import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import Specialization from '../../../entities/specialization';
import { AxiosError } from "axios";
import { Action } from "redux";
import { takeLatest } from "redux-saga";
import { AxiosResponse } from "axios";
import { call, CallEffect, put, PutEffect } from "redux-saga/effects";
import { GETAll } from '../../api/specializations';

interface FetchSpecializationsState {
  loading: boolean;
  error?: string;
  data?: Specialization[]
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
    fetchSpecializationsSuccess: (state, action: PayloadAction<Specialization[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchSpecializationsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

type ApiResponse = AxiosResponse<Specialization[]>;

type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

function* fetchSpecializations() : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const response : ApiResponse  = yield call(GETAll);
    yield put(fetchSpecializationsSuccess(response.data))
  } catch (error) {
    let errorMessage = "An unknown error occurred";

    if ((error as ApiError).response && (error as ApiError).response?.data) {
      const problemDetails = (error as ApiError).response!.data;
      errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
    }
    yield put(fetchSpecializationsFailure(errorMessage))
  }
}

function* watchFetchSpecializations() {
  yield takeLatest(fetchSpecializationsSlice.actions.fetchSpecializationsRequest.type, fetchSpecializations);
}

export const { fetchSpecializationsRequest, fetchSpecializationsSuccess, fetchSpecializationsFailure } = fetchSpecializationsSlice.actions;
export const fetchSpecializationsSliceReducer = fetchSpecializationsSlice.reducer
export { watchFetchSpecializations }