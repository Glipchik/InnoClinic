import { createSlice } from '@reduxjs/toolkit';
import Service from '../../entities/service';

interface AppointmentsState {
  loading: boolean;
  error: string | null;
  appointmentsData: Service | Service[] | null
}

const initialState : AppointmentsState = {
  loading: false,
  error: null,
  appointmentsData: null
};

const AppointmentsSlice = createSlice({
  name: 'Appointments',
  initialState,
  reducers: {
    fetchAppointmentsDataRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchAppointmentsDataSuccess: (state, action) => {
      state.loading = false;
      state.appointmentsData = action.payload;
    },
    fetchAppointmentsDataFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchAppointmentsDataRequest, fetchAppointmentsDataSuccess, fetchAppointmentsDataFailure } = AppointmentsSlice.actions;
export default AppointmentsSlice.reducer;