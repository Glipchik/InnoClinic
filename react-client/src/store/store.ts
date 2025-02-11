import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import fetchSpecializationsReducer from './slices/specializations/fetchSpecializationsSlice';
import fetchServicesReducer from './slices/services/fetchServicesSlice';
import fetchDoctorsReducer from './slices/doctors/fetchDoctorsSlice';
import createAppointmentReducer from './slices/appointments/createAppointmentSlice';
import fetchDoctorScheduleReducer from './slices/appointments/fetchDoctorSchedule';

export const store = configureStore({
  reducer: {
    auth: authReducer,

    fetchSpecializationsReducer: fetchSpecializationsReducer,

    fetchServicesReducer: fetchServicesReducer,

    fetchDoctorsReducer: fetchDoctorsReducer,

    createAppointmentReducer: createAppointmentReducer,
    fetchDoctorScheduleReducer: fetchDoctorScheduleReducer,
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;