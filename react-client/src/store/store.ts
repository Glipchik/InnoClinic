import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import specializationsReducer from './slices/specializationsSlice';
import servicesReducer from './slices/servicesSlice';
import doctorsReducer from './slices/doctorsSlice';
import appointmentsReducer from './slices/appointmentsSlice';
import doctorScheduleReducer from './slices/doctorScheduleSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    specializations: specializationsReducer,
    services: servicesReducer,
    doctorSchedule: doctorScheduleReducer,
    doctors: doctorsReducer,
    appointments: appointmentsReducer,
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;