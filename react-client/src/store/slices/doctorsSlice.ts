import { createSlice } from '@reduxjs/toolkit';
import Service from '../../entities/service';

interface DoctorsState {
  loading: boolean;
  error: string | null;
  doctorsData: Service | Service[] | null 
}

const initialState : DoctorsState = {
  loading: false,
  error: null,
  doctorsData: null
};

const DoctorsSlice = createSlice({
  name: 'Doctors',
  initialState,
  reducers: {
    fetchDoctorsDataRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchDoctorsDataSuccess: (state, action) => {
      state.loading = false;
      state.doctorsData = action.payload;
    },
    fetchDoctorsDataFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchDoctorsDataRequest, fetchDoctorsDataSuccess, fetchDoctorsDataFailure } = DoctorsSlice.actions;
export default DoctorsSlice.reducer;