import doctorsApi from '@features/create-appointment-form/api/doctors';
import DoctorModel from '@models/doctors/doctorModel';
import { AnyAction, createSlice, PayloadAction } from '@reduxjs/toolkit';
import handleError, { ApiError } from '@shared/lib/errorHandler';
import { AxiosResponse } from 'axios';
import { call, CallEffect, put, PutEffect, takeLatest } from 'redux-saga/effects';

interface DoctorsState {
  loading: boolean
  error?: string
  data?: DoctorModel[]
}

const initialState : DoctorsState = {
  loading: false,
  error: undefined,
  data: undefined
};

const fetchDoctorsSlice = createSlice({
  name: 'FetchDoctorsSlice',
  initialState,
  reducers: {
    fetchDoctorsRequest: (state, action: PayloadAction<{ specializationId: string }>) => {
      state.loading = !!action;
    },
    fetchDoctorsSuccess: (state, action: PayloadAction<DoctorModel[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchDoctorsFailure: (state, action: PayloadAction<string>) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchDoctorsRequest } = fetchDoctorsSlice.actions;
export const fetchDoctorsSliceReducer = fetchDoctorsSlice.reducer

type ApiResponse = AxiosResponse<DoctorModel[]>;

function* fetchDoctors(action: AnyAction) : Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const {specializationId} = action.payload
    const response : ApiResponse  = yield call(doctorsApi.getAll, specializationId);
    yield put(fetchDoctorsSlice.actions.fetchDoctorsSuccess(response.data))
  } catch (error) {
    yield put(fetchDoctorsSlice.actions.fetchDoctorsFailure(handleError(error as ApiError)))
  }
}

function* watchFetchDoctors() {
  yield takeLatest(fetchDoctorsRequest.type, fetchDoctors);
}

export { watchFetchDoctors }