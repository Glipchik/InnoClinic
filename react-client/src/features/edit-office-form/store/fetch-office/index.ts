import officesApi from '@features/offices-list/api/offices';
import OfficeModel from '@models/offices/OfficeModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { CallEffect, PutEffect, call, put, takeLatest } from 'redux-saga/effects';

interface FetchOfficeByIdState {
  loading: boolean
  error?: string
  data?: OfficeModel
}

const initialState : FetchOfficeByIdState = {
  loading: false
};

const fetchOfficeByIdSlice = createSlice({
  name: 'FetchOfficeByIdSlice',
  initialState,
  reducers: {
    fetchOfficeByIdRequest: (state, action: PayloadAction<{ officeId: string }>) => {
      state.loading = !!action;
    },
    fetchOfficeByIdSuccess: (state, action: PayloadAction<OfficeModel>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchOfficeByIdFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchOfficeByIdRequest } = fetchOfficeByIdSlice.actions;
export const fetchOfficeByIdSliceReducer = fetchOfficeByIdSlice.reducer

type ApiResponse = AxiosResponse<OfficeModel>;

function* fetchOfficeById(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { officeId } = action.payload;
    const response: ApiResponse = yield call(officesApi.getById, officeId);
    yield put(fetchOfficeByIdSlice.actions.fetchOfficeByIdSuccess(response.data))
  } catch (error) {
    yield put(fetchOfficeByIdSlice.actions.fetchOfficeByIdFailure(handleError(error as ApiError)))
  }
}

function* watchFetchOfficeById() {
  yield takeLatest(fetchOfficeByIdRequest.type, fetchOfficeById);
}

export { watchFetchOfficeById }