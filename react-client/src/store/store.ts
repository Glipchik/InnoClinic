import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
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

import fetchServicesReducer from './slices/services/fetchServicesSlice';
import createServiceReducer from './slices/services/createServiceSlice';
import editServiceReducer from './slices/services/editServiceSlice';
import deleteServiceReducer from './slices/services/deleteServiceSlice';

import fetchServiceCategoriesReducer from './slices/serviceCategories/fetchServiceCategoriesSlice';
import editServiceCategoriesReducer from './slices/serviceCategories/editServiceCategorySlice';
import deleteServiceCategoriesReducer from './slices/serviceCategories/deleteServiceCategorySlice';
import createServiceCategoriesReducer from './slices/serviceCategories/createServiceCategorySlice';

import fetchDoctorsReducer from './slices/doctors/fetchDoctorsSlice';
import editDoctorReducer from './slices/doctors/editDoctorSlice';
import deleteDoctorReducer from './slices/doctors/deleteDoctorSlice';
import createDoctorReducer from './slices/doctors/createDoctorSlice';

import fetchPatientsReducer from './slices/patients/fetchPatientsSlice';
import editPatientReducer from './slices/patients/editPatientSlice';
import deletePatientReducer from './slices/patients/deletePatientSlice';
import createPatientReducer from './slices/patients/createPatientSlice';

import fetchReceptionistsReducer from './slices/receptionists/fetchReceptionistsSlice';
import editReceptionistReducer from './slices/receptionists/editReceptionistSlice';
import deleteReceptionistReducer from './slices/receptionists/deleteReceptionistSlice';
import createReceptionistReducer from './slices/receptionists/createReceptionistSlice';

import fetchProfileReducer from './slices/profile/fetchProfileSlice';
import editProfileReducer from './slices/profile/editProfileSlice';
import deleteProfileReducer from './slices/profile/deleteProfileSlice';

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
    createServiceReducer: createServiceReducer,
    editServiceReducer: editServiceReducer,
    deleteServiceReducer: deleteServiceReducer,

    createAppointmentReducer: createAppointmentReducer,

    fetchDoctorScheduleReducer: fetchDoctorScheduleReducer,

    fetchDoctorsReducer: fetchDoctorsReducer,
    createDoctorReducer: createDoctorReducer,
    editDoctorReducer: editDoctorReducer,
    deleteDoctorReducer: deleteDoctorReducer,
    
    fetchPatientsReducer: fetchPatientsReducer,
    createPatientReducer: createPatientReducer,
    editPatientReducer: editPatientReducer,
    deletePatientReducer: deletePatientReducer,

    fetchReceptionistsReducer: fetchReceptionistsReducer,
    createReceptionistReducer: createReceptionistReducer,
    editReceptionistReducer: editReceptionistReducer,
    deleteReceptionistReducer: deleteReceptionistReducer,

    fetchServiceCategoriesReducer: fetchServiceCategoriesReducer,
    editServiceCategoriesReducer: editServiceCategoriesReducer,
    deleteServiceCategoriesReducer: deleteServiceCategoriesReducer,
    createServiceCategoriesReducer: createServiceCategoriesReducer,

    fetchProfileReducer: fetchProfileReducer,
    editProfileReducer: editProfileReducer,
    deleteProfileReducer: deleteProfileReducer
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;