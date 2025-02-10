import { createSlice } from '@reduxjs/toolkit';
import Office from '../../entities/office';
import PaginatedList from '../../models/paginatedList';

interface OfficesState {
  loading: boolean;
  error: string | null;
  officesData: PaginatedList<Office> | null
}

const initialState : OfficesState = {
  loading: false,
  error: null,
  officesData: null
};

const OfficesSlice = createSlice({
  name: 'Offices',
  initialState,
  reducers: {
    fetchOfficesDataRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchOfficesDataSuccess: (state, action) => {
      state.loading = false;
      state.officesData = action.payload;
    },
    fetchOfficesDataFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchOfficesDataRequest, fetchOfficesDataSuccess, fetchOfficesDataFailure } = OfficesSlice.actions;
export default OfficesSlice.reducer;