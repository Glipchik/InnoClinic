import { createSlice } from '@reduxjs/toolkit';

interface CreateDoctorState {
  loading: boolean;
  error: string | null;
}

const initialState : CreateDoctorState = {
  loading: false,
  error: null,
};

const CreateDoctorSlice = createSlice({
  name: 'CreateDoctor',
  initialState,
  reducers: {
    createDoctorRequest: state => {
      state.loading = true;
      state.error = null;
    },
    createDoctorSuccess: (state) => {
      state.loading = false;
    },
    createDoctorFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createDoctorRequest, createDoctorSuccess, createDoctorFailure } = CreateDoctorSlice.actions;
export default CreateDoctorSlice.reducer;