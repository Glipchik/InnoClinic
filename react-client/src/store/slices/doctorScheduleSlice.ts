import { createSlice } from '@reduxjs/toolkit';
import TimeSlot from '../../entities/timeSlot';

interface doctorScheduleState {
  loading: boolean;
  error: string | null;
  doctorScheduleData: TimeSlot[] | null 
}

const initialState : doctorScheduleState = {
  loading: false,
  error: null,
  doctorScheduleData: null
};

const DoctorScheduleDataSlice = createSlice({
  name: 'DoctorScheduleData',
  initialState,
  reducers: {
    fetchDoctorScheduleDataDataRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchDoctorScheduleDataSuccess: (state, action) => {
      state.loading = false;
      state.doctorScheduleData = action.payload;
    },
    fetchDoctorScheduleDataFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchDoctorScheduleDataDataRequest, fetchDoctorScheduleDataSuccess, fetchDoctorScheduleDataFailure } = DoctorScheduleDataSlice.actions;
export default DoctorScheduleDataSlice.reducer;