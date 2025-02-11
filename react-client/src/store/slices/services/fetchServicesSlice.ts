import { createSlice } from '@reduxjs/toolkit';
import Service from '../../../entities/service';

interface ServicesState {
  loading: boolean;
  error: string | null;
  data: Service | Service[] | null 
}

const initialState : ServicesState = {
  loading: false,
  error: null,
  data: null
};

const FetchServicesSlice = createSlice({
  name: 'Services',
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