import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import fetchSpecializationsReducer from './slices/specializations/fetchSpecializationsSlice';
import fetchServicesReducer from './slices/services/fetchServicesSlice';
import fetchDoctorsReducer from './slices/doctors/fetchDoctorsSlice';
import createAppointmentReducer from './slices/appointments/createAppointmentSlice';
import fetchDoctorScheduleReducer from './slices/appointments/fetchDoctorSchedule';

import fetchOfficesReducer from './slices/offices/fetchOfficesSlice';
import editOfficeReducer from './slices/offices/editOfficeSlice';
import deleteOfficeReducer from './slices/offices/deleteOfficeSlice';
import createOfficeReducer from './slices/offices/createOfficeSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,

    fetchOfficesReducer: fetchOfficesReducer,
    editOfficeReducer: editOfficeReducer,
    createOfficeReducer: createOfficeReducer,
    deleteOfficeReducer: deleteOfficeReducer,

    fetchSpecializationsReducer: fetchSpecializationsReducer,

    fetchServicesReducer: fetchServicesReducer,

    fetchDoctorsReducer: fetchDoctorsReducer,

    createAppointmentReducer: createAppointmentReducer,
    fetchDoctorScheduleReducer: fetchDoctorScheduleReducer,
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;