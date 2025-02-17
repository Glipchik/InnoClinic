import { createSlice } from '@reduxjs/toolkit';

interface CreatePatientState {
  loading: boolean;
  error: string | null;
}

const initialState : CreatePatientState = {
  loading: false,
  error: null,
};

const CreatePatientSlice = createSlice({
  name: 'CreatePatient',
  initialState,
  reducers: {
    createPatientRequest: state => {
      state.loading = true;
      state.error = null;
    },
    createPatientSuccess: (state) => {
      state.loading = false;
    },
    createPatientFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createPatientRequest, createPatientSuccess, createPatientFailure } = CreatePatientSlice.actions;
export default CreatePatientSlice.reducer;