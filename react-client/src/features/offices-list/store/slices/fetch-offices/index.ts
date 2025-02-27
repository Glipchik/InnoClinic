import OfficeModel from '@models/offices/OfficeModel';
import PaginatedList from '@models/paginatedList';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface FetchOfficeState {
  loading: boolean
  error?: string
  data?: PaginatedList<OfficeModel>
}

const initialState : FetchOfficeState = {
  loading: false
};

const fetchOfficesSlice = createSlice({
  name: 'FetchOfficesSlice',
  initialState,
  reducers: {
    fetchOfficesRequest: (state, action: PayloadAction<{pageIndex: number, pageSize: number}>) => {
      state.loading = !!action;
    },
    fetchOfficesSuccess: (state, action: PayloadAction<PaginatedList<OfficeModel>>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchOfficesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchOfficesRequest, fetchOfficesFailure, fetchOfficesSuccess } = fetchOfficesSlice.actions;
export const fetchOfficeSliceReducer = fetchOfficesSlice.reducer