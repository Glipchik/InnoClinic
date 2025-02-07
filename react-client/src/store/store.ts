import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import specializationsReducer from './slices/specializationsSlice';
import servicesReducer from './slices/servicesSlice';
import doctorsReducer from './slices/doctorsSlice';
import doctorScheduleReducer from './slices/doctorScheduleSlice';

const store = configureStore({
  reducer: {
    auth: authReducer,
    specializations: specializationsReducer,
    services: servicesReducer,
    doctorSchedule: doctorScheduleReducer,
    doctors: doctorsReducer
  }
});

export default store;
