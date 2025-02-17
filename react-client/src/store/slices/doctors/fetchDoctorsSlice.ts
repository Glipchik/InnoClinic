import { createSlice } from '@reduxjs/toolkit';
import Doctor from '../../../entities/doctor';
import PaginatedList from '../../../models/paginatedList';
import DoctorModel from '../../../models/doctors/DoctorModel';

interface FetchDoctorsState {
  loading: boolean;
  error: string | null;
  data: Doctor | Doctor[] | null | PaginatedList<DoctorModel>;
}

const initialState : FetchDoctorsState = {
  loading: false,
  error: null,
  data: null
};

const FetchDoctorsSlice = createSlice({
  name: 'FetchDoctors',
  initialState,
  reducers: {
    fetchDoctorsRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchDoctorsSuccess: (state, action) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchDoctorsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchDoctorsRequest, fetchDoctorsSuccess, fetchDoctorsFailure } = FetchDoctorsSlice.actions;
export default FetchDoctorsSlice.reducer;