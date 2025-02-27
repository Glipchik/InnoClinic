import ServiceModel from '@models/services/serviceModel';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface ServicesState {
  loading: boolean
  error?: string
  data?: ServiceModel[]
}

const initialState : ServicesState = {
  loading: false,
  error: undefined,
  data: undefined
};

const fetchServicesSlice = createSlice({
  name: 'FetchServicesSlice',
  initialState,
  reducers: {
    fetchServicesRequest: (state, action: PayloadAction<{ specializationId: string }>) => {
      state.loading = !!action;
    },
    fetchServicesSuccess: (state, action: PayloadAction<ServiceModel[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServicesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServicesRequest, fetchServicesFailure, fetchServicesSuccess } = fetchServicesSlice.actions;
export const fetchServicesSliceReducer = fetchServicesSlice.reducer