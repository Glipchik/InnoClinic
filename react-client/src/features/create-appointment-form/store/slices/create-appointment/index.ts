import CreateAppointmentModel from '@features/create-appointment-form/models/createAppointmentModel';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface CreateAppointmentState {
  success: boolean
  loading: boolean
  error?: string
}

const initialState : CreateAppointmentState = {
  success: false,
  loading: false,
  error: undefined
};

const createAppointmentSlice = createSlice({
  name: 'CreateAppointmentSlice',
  initialState,
  reducers: {
    createAppointmentRequest: (state, action: PayloadAction<CreateAppointmentModel>) => {
      state.loading = !!action;
    },
    createAppointmentSuccess: (state) => {
      state.loading = false;
      state.success = true
    },
    createAppointmentFailure: (state, action) => {
      state.success = false
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createAppointmentRequest, createAppointmentFailure, createAppointmentSuccess } = createAppointmentSlice.actions;
export const createAppointmentSliceReducer = createAppointmentSlice.reducer