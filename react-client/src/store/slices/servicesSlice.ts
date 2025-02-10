import { createSlice } from '@reduxjs/toolkit';
import Service from '../../entities/service';

interface ServicesState {
  loading: boolean;
  error: string | null;
  servicesData: Service | Service[] | null 
}

const initialState : ServicesState = {
  loading: false,
  error: null,
  servicesData: null
};

const ServicesSlice = createSlice({
  name: 'Services',
  initialState,
  reducers: {
    fetchServicesDataRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchServicesDataSuccess: (state, action) => {
      state.loading = false;
      state.servicesData = action.payload;
    },
    fetchServicesDataFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServicesDataRequest, fetchServicesDataSuccess, fetchServicesDataFailure } = ServicesSlice.actions;
export default ServicesSlice.reducer;