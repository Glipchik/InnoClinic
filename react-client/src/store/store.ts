import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import fetchServicesReducer from './slices/services/fetchServicesSlice';
import fetchDoctorsReducer from './slices/doctors/fetchDoctorsSlice';
import createAppointmentReducer from './slices/appointments/createAppointmentSlice';
import fetchDoctorScheduleReducer from './slices/appointments/fetchDoctorSchedule';

import fetchOfficesReducer from './slices/offices/fetchOfficesSlice';
import editOfficeReducer from './slices/offices/editOfficeSlice';
import deleteOfficeReducer from './slices/offices/deleteOfficeSlice';
import createOfficeReducer from './slices/offices/createOfficeSlice';

import fetchSpecializationsReducer from './slices/specializations/fetchSpecializationsSlice';
import createSpecializationReducer from './slices/specializations/createSpecializationSlice';
import editSpecializationReducer from './slices/specializations/editSpecializationSlice';
import deleteSpecializationReducer from './slices/specializations/deleteSpecializationSlice';

import fetchServiceCategoriesReducer from './slices/serviceCategories/fetchServiceCategoriesSlice';
import editServiceCategoriesReducer from './slices/serviceCategories/editServiceCategorySlice';
import deleteServiceCategoriesReducer from './slices/serviceCategories/deleteServiceCategorySlice';
import createServiceCategoriesReducer from './slices/serviceCategories/createServiceCategorySlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,

    fetchOfficesReducer: fetchOfficesReducer,
    editOfficeReducer: editOfficeReducer,
    createOfficeReducer: createOfficeReducer,
    deleteOfficeReducer: deleteOfficeReducer,

    fetchSpecializationsReducer: fetchSpecializationsReducer,
    createSpecializationReducer: createSpecializationReducer,
    editSpecializationReducer: editSpecializationReducer,
    deleteSpecializationReducer: deleteSpecializationReducer,

    fetchServicesReducer: fetchServicesReducer,

    fetchDoctorsReducer: fetchDoctorsReducer,

    createAppointmentReducer: createAppointmentReducer,
    fetchDoctorScheduleReducer: fetchDoctorScheduleReducer,

    fetchServiceCategoriesReducer: fetchServiceCategoriesReducer,
    editServiceCategoriesReducer: editServiceCategoriesReducer,
    deleteServiceCategoriesReducer: deleteServiceCategoriesReducer,
    createServiceCategoriesReducer: createServiceCategoriesReducer,
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;