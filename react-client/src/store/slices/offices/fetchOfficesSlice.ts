import { createSlice } from '@reduxjs/toolkit';
import Office from '../../../entities/office';
import PaginatedList from '../../../models/paginatedList';

interface FetchOfficesState {
  loading: boolean;
  error: string | null;
  officesData: PaginatedList<Office> | null
}

const initialState : FetchOfficesState = {
  loading: false,
  error: null,
  officesData: null
};

const FetchOfficesSlice = createSlice({
  name: 'FetchOffices',
  initialState,
  reducers: {
    fetchOfficesRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchOfficesSuccess: (state, action) => {
      state.loading = false;
      state.officesData = action.payload;
    },
    fetchOfficesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchOfficesRequest, fetchOfficesSuccess, fetchOfficesFailure } = FetchOfficesSlice.actions;
export default FetchOfficesSlice.reducer;