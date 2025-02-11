import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import specializationsReducer from './slices/specializationsSlice';
import servicesReducer from './slices/servicesSlice';
import doctorsReducer from './slices/doctorsSlice';
import appointmentsReducer from './slices/appointmentsSlice';
import doctorScheduleReducer from './slices/doctorScheduleSlice';

import fetchOfficesReducer from './slices/offices/fetchOfficesSlice';
import editOfficeReducer from './slices/offices/editOfficeSlice';
import deleteOfficeReducer from './slices/offices/deleteOfficeSlice';
import createOfficeReducer from './slices/offices/createOfficeSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    specializations: specializationsReducer,
    services: servicesReducer,
    doctorSchedule: doctorScheduleReducer,
    doctors: doctorsReducer,
    appointments: appointmentsReducer,

    fetchOfficesReducer: fetchOfficesReducer,
    editOfficeReducer: editOfficeReducer,
    createOfficeReducer: createOfficeReducer,
    deleteOfficeReducer: deleteOfficeReducer
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;