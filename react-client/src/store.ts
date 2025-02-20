import { combineReducers } from 'redux';
import createSagaMiddleware from 'redux-saga';
import { watchFetchSpecializations, fetchSpecializationsSliceReducer } from './shared/store/specializations';
import { configureStore } from '@reduxjs/toolkit';
import { fetchServicesSliceReducer, watchFetchServices } from './shared/store/services';
import { fetchDoctorsSliceReducer, watchFetchDoctors } from './shared/store/doctors';
import { fetchDoctorScheduleSliceReducer, watchFetchDoctorSchedule } from './shared/store/doctor-schedule';

const rootReducer = combineReducers({
  fetchSpecializations: fetchSpecializationsSliceReducer,
  fetchServices: fetchServicesSliceReducer,
  fetchDoctors: fetchDoctorsSliceReducer,
  fetchDoctorSchedule: fetchDoctorScheduleSliceReducer
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

export default store;
