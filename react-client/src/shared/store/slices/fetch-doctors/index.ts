import DoctorModel from '@models/doctors/doctorModel';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

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
    fetchDoctorsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchDoctorsRequest, fetchDoctorsFailure, fetchDoctorsSuccess } = fetchDoctorsSlice.actions;
export const fetchDoctorsSliceReducer = fetchDoctorsSlice.reducer