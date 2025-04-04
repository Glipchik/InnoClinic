import { combineReducers } from 'redux';
import createSagaMiddleware from 'redux-saga';
import { configureStore } from '@reduxjs/toolkit';
import { fetchDoctorScheduleSliceReducer, watchFetchDoctorSchedule } from '@features/create-appointment-form/store/fetch-doctor-schedule';
import { createAppointmentSliceReducer, watchCreateAppointment } from '@features/create-appointment-form/store/create-appointment';
import { fetchDoctorsSliceReducer, watchFetchDoctors } from '@features/create-appointment-form/store/fetch-doctors';
import { fetchServicesSliceReducer, watchFetchServices } from '@features/create-appointment-form/store/fetch-services';
import { fetchSpecializationsSliceReducer, watchFetchSpecializations } from '@shared/store/fetch-specializations';
import { fetchOfficesSliceReducer, watchFetchOffices } from '@shared/store/fetch-offices';
import { deleteOfficeSliceReducer, watchDeleteOffice } from '@features/offices-list/store/delete-office';
import { editOfficeSliceReducer, watchEditOffice } from '@features/edit-office-form/store/edit-office';
import { fetchOfficeByIdSliceReducer, watchFetchOfficeById } from '@features/edit-office-form/store/fetch-office';
import { createOfficeSliceReducer, watchCreateOffice } from '@features/create-office-form/store/create-office';
import { deleteSpecializationSliceReducer, watchDeleteSpecialization } from '@features/specializations-list/store/delete-specialization';
import { fetchSpecializationsWithPaginationSliceReducer, watchFetchSpecializationsWithPagination } from '@shared/store/fetch-specializations-with-pagination';
import { editSpecializationSliceReducer, watchEditSpecialization } from '@features/edit-specialization-form/store/edit-specialization';
import { fetchSpecializationByIdSliceReducer, watchFetchSpecializationById } from '@features/edit-specialization-form/store/fetch-specialization';
import { createSpecializationSliceReducer, watchCreateSpecialization } from '@features/create-specialization-form/store/create-specialization';

const rootReducer = combineReducers({
  fetchSpecializations: fetchSpecializationsSliceReducer,
  fetchServices: fetchServicesSliceReducer,
  fetchDoctors: fetchDoctorsSliceReducer,
  fetchDoctorSchedule: fetchDoctorScheduleSliceReducer,
  createAppointment: createAppointmentSliceReducer,
  fetchOffices: fetchOfficesSliceReducer,
  deleteOffice: deleteOfficeSliceReducer,
  editOffice: editOfficeSliceReducer,
  fetchOfficeById: fetchOfficeByIdSliceReducer,
  createOffice: createOfficeSliceReducer,
  deleteSpecialization: deleteSpecializationSliceReducer,
  fetchSpecializationsWithPagination: fetchSpecializationsWithPaginationSliceReducer,
  editSpecialization: editSpecializationSliceReducer,
  fetchSpecializationById: fetchSpecializationByIdSliceReducer,
  createSpecialization: createSpecializationSliceReducer
});

const sagaMiddleware = createSagaMiddleware();

const store = configureStore({
  reducer: rootReducer,
  middleware: [sagaMiddleware]
});

sagaMiddleware.run(watchFetchSpecializations);
sagaMiddleware.run(watchFetchServices);
sagaMiddleware.run(watchFetchDoctors);
sagaMiddleware.run(watchFetchDoctorSchedule);
sagaMiddleware.run(watchCreateAppointment);
sagaMiddleware.run(watchFetchOffices);
sagaMiddleware.run(watchDeleteOffice);
sagaMiddleware.run(watchEditOffice);
sagaMiddleware.run(watchFetchOfficeById);
sagaMiddleware.run(watchCreateOffice);
sagaMiddleware.run(watchDeleteSpecialization);
sagaMiddleware.run(watchFetchSpecializationsWithPagination);
sagaMiddleware.run(watchEditSpecialization);
sagaMiddleware.run(watchFetchSpecializationById);
sagaMiddleware.run(watchCreateSpecialization);

export default store;
export type RootState = ReturnType<typeof store.getState>;
