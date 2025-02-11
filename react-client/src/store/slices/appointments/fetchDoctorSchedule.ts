import { createSlice } from '@reduxjs/toolkit';
import TimeSlot from '../../../entities/timeSlot';

interface FetchDoctorScheduleState {
  loading: boolean;
  error: string | null;
  data: TimeSlot[] | null 
}

const initialState : FetchDoctorScheduleState = {
  loading: false,
  error: null,
  data: null
};

const DoctorScheduleDataSlice = createSlice({
  name: 'FetchDoctorScheduleData',
  initialState,
  reducers: {
    fetchDoctorScheduleRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchDoctorScheduleSuccess: (state, action) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchDoctorScheduleFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchDoctorScheduleRequest, fetchDoctorScheduleSuccess, fetchDoctorScheduleFailure } = DoctorScheduleDataSlice.actions;
export default DoctorScheduleDataSlice.reducer;