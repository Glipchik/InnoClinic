import { combineReducers } from 'redux';
import createSagaMiddleware from 'redux-saga';
import { watchFetchSpecializations, fetchSpecializationsSliceReducer } from '@shared/store/specializations';
import { configureStore } from '@reduxjs/toolkit';
import { fetchServicesSliceReducer, watchFetchServices } from '@shared/store/services';
import { fetchDoctorsSliceReducer, watchFetchDoctors } from '@shared/store/doctors';
import { fetchDoctorScheduleSliceReducer, watchFetchDoctorSchedule } from '@shared/store/doctor-schedule';
import { createAppointmentSliceReducer, watchCreateAppointment } from '@features/create-appointment-form/store';

const rootReducer = combineReducers({
  fetchSpecializations: fetchSpecializationsSliceReducer,
  fetchServices: fetchServicesSliceReducer,
  fetchDoctors: fetchDoctorsSliceReducer,
  fetchDoctorSchedule: fetchDoctorScheduleSliceReducer,
  createAppointment: createAppointmentSliceReducer
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

export default store;
export type RootState = ReturnType<typeof store.getState>;
