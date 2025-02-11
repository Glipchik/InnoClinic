import { createSlice } from '@reduxjs/toolkit';

interface CreateAppointmentState {
  loading: boolean;
  error: string | null;
}

const initialState : CreateAppointmentState = {
  loading: false,
  error: null
};

const CreateAppointmentSlice = createSlice({
  name: 'CreateAppointment',
  initialState,
  reducers: {
    createAppointmentRequest: state => {
      state.loading = true;
      state.error = null;
    },
    createAppointmentSuccess: (state) => {
      state.loading = false;
    },
    createAppointmentFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createAppointmentRequest, createAppointmentSuccess, createAppointmentFailure } = CreateAppointmentSlice.actions;
export default CreateAppointmentSlice.reducer;