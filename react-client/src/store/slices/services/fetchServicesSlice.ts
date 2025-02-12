import { createSlice } from '@reduxjs/toolkit';
import Service from '../../../entities/service';
import PaginatedList from '../../../models/paginatedList';

interface FetchServicesState {
  loading: boolean;
  error: string | null;
  data: Service | Service[] | PaginatedList<Service> | null 
}

const initialState : FetchServicesState = {
  loading: false,
  error: null,
  data: null
};

const FetchServicesSlice = createSlice({
  name: 'FetchServices',
  initialState,
  reducers: {
    fetchServicesRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchServicesSuccess: (state, action) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServicesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServicesRequest, fetchServicesSuccess, fetchServicesFailure } = FetchServicesSlice.actions;
export default FetchServicesSlice.reducer;