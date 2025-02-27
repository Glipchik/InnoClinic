import { AxiosResponse, AxiosError } from 'axios';
import { takeLatest } from 'redux-saga/effects';
import { call, CallEffect, put, PutEffect } from 'redux-saga/effects';
import { AnyAction } from 'redux-saga';
import OfficeModel from '@models/offices/OfficeModel';
import PaginatedList from '@models/paginatedList';
import officesApi from '@features/offices-list/api/offices';
import { fetchOfficesSuccess, fetchOfficesFailure, fetchOfficesRequest } from '../../slices/fetch-offices';
import handleError, { ApiError } from '@shared/api/lib/errorHandler';

type ApiResponse = AxiosResponse<PaginatedList<OfficeModel>>;

function* fetchOffices(
  action: AnyAction
): Generator<CallEffect<ApiResponse> | PutEffect, void, ApiResponse> {
  try {
    const { pageIndex, pageSize } = action.payload;
    const response = yield call(officesApi.getAll, pageIndex, pageSize);
    yield put(fetchOfficesSuccess(response.data))
  } catch (error) {
    yield put(fetchOfficesFailure(handleError(error as ApiError)))
  }
}

function* watchFetchOffices() {
  yield takeLatest(fetchOfficesRequest.type, fetchOffices);
}

export { watchFetchOffices }