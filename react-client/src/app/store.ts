import { combineReducers } from 'redux';
import createSagaMiddleware from 'redux-saga';
import { configureStore } from '@reduxjs/toolkit';
import { fetchSpecializationsSliceReducer } from '@shared/store/slices/fetch-specializations';
import { fetchServicesSliceReducer } from '@shared/store/slices/fetch-services';
import { fetchDoctorsSliceReducer } from '@shared/store/slices/fetch-doctors';
import { fetchDoctorScheduleSliceReducer } from '@features/create-appointment-form/store/slices/fetch-doctor-schedule';
import { createAppointmentSliceReducer } from '@features/create-appointment-form/store/slices/create-appointment';
import { watchCreateAppointment } from '@features/create-appointment-form/store/sagas/create-appointment';
import { watchFetchDoctorSchedule } from '@features/create-appointment-form/store/sagas/fetch-doctor-schedule';
import { watchFetchDoctors } from '@shared/store/sagas/fetch-doctors';
import { watchFetchServices } from '@shared/store/sagas/fetch-services';
import { watchFetchSpecializations } from '@shared/store/sagas/fetch-specializations';
import { fetchOfficeSliceReducer } from '@features/offices-list/store/slices/fetch-offices';
import { watchFetchOffices } from '@features/offices-list/store/sagas/fetch-offices';

const rootReducer = combineReducers({
  fetchSpecializations: fetchSpecializationsSliceReducer,
  fetchServices: fetchServicesSliceReducer,
  fetchDoctors: fetchDoctorsSliceReducer,
  fetchDoctorSchedule: fetchDoctorScheduleSliceReducer,
  createAppointment: createAppointmentSliceReducer,
  fetchOffices: fetchOfficeSliceReducer
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

export default store;
export type RootState = ReturnType<typeof store.getState>;
