import { combineReducers } from 'redux';
import createSagaMiddleware from 'redux-saga';
import { configureStore } from '@reduxjs/toolkit';
import { fetchDoctorScheduleSliceReducer, watchFetchDoctorSchedule } from '@features/create-appointment-form/store/fetch-doctor-schedule';
import { createAppointmentSliceReducer, watchCreateAppointment } from '@features/create-appointment-form/store/create-appointment';
import { fetchDoctorsSliceReducer, watchFetchDoctors } from '@features/create-appointment-form/store/fetch-doctors';
import { fetchServicesSliceReducer, watchFetchServices } from '@features/create-appointment-form/store/fetch-services';
import { fetchSpecializationsSliceReducer, watchFetchSpecializations } from '@shared/store/fetch-specializations';

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
