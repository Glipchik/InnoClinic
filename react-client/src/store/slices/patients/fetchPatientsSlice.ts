import { createSlice } from '@reduxjs/toolkit';
import Patient from '../../../entities/patient';
import PaginatedList from '../../../models/paginatedList';
import PatientModel from '../../../models/patients/PatientModel';

interface FetchPatientsState {
  loading: boolean;
  error: string | null;
  data: Patient | Patient[] | null | PaginatedList<PatientModel>
}

const initialState : FetchPatientsState = {
  loading: false,
  error: null,
  data: null
};

const FetchPatientsSlice = createSlice({
  name: 'FetchPatients',
  initialState,
  reducers: {
    fetchPatientsRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchPatientsSuccess: (state, action) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchPatientsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchPatientsRequest, fetchPatientsSuccess, fetchPatientsFailure } = FetchPatientsSlice.actions;
export default FetchPatientsSlice.reducer;