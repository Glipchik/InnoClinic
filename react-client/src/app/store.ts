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
  createOffice: createOfficeSliceReducer
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

export default store;
export type RootState = ReturnType<typeof store.getState>;
