import TimeSlot from '@entities/timeSlot';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface DoctorScheduleState {
  loading: boolean
  error?: string
  data?: TimeSlot[]
}

const initialState : DoctorScheduleState = {
  loading: false
};

const fetchDoctorScheduleSlice = createSlice({
  name: 'FetchDoctorScheduleSlice',
  initialState,
  reducers: {
    fetchDoctorScheduleRequest: (state, action: PayloadAction<{ doctorId: string, date: string }>) => {
      state.loading = !!action;
    },
    fetchDoctorScheduleSuccess: (state, action: PayloadAction<TimeSlot[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchDoctorScheduleFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchDoctorScheduleRequest, fetchDoctorScheduleFailure, fetchDoctorScheduleSuccess } = fetchDoctorScheduleSlice.actions;
export const fetchDoctorScheduleSliceReducer = fetchDoctorScheduleSlice.reducer